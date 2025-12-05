using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Daybreak.Common.Assets;
using Daybreak.Common.Features.Hooks;
using Daybreak.Common.Features.Models;
using Daybreak.Common.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace SonarIcons;

internal sealed class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(true)]
    public bool GrayscaleItems { get; set; } = true;
}

internal static class SonarText
{
    private sealed class GrayscaleItemCache : IStatic<GrayscaleItemCache>
    {
        public required Asset<Effect> GrayscaleEffect { get; init; }

        private readonly Dictionary<int, RenderTarget2D> targets = [];

        public Texture2D GetGrayscaleItem(int itemType)
        {
            if (targets.TryGetValue(itemType, out var target))
            {
                return target;
            }

            Main.spriteBatch.End(out var ss);

            Main.instance.LoadItem(itemType);
            var asset = TextureAssets.Item[itemType].Value;

            target = RenderTargetDescriptor.DefaultPreserveContents.Create(
                Main.instance.GraphicsDevice,
                asset.Width,
                asset.Height
            );
            using (target.Scope(clearColor: Color.Transparent))
            {
                Main.spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone,
                    GrayscaleEffect.Value,
                    Matrix.Identity
                );
                Main.spriteBatch.Draw(asset, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }

            Main.spriteBatch.Begin(ss);

            return targets[itemType] = target;
        }

        public static GrayscaleItemCache LoadData(Mod mod)
        {
            return new GrayscaleItemCache
            {
                GrayscaleEffect = Assets.Shaders.ItemGrayscale.Asset,
            };
        }

        public static void UnloadData(GrayscaleItemCache data)
        {
            Main.QueueMainThreadAction(
                () =>
                {
                    foreach (var target in data.targets.Values)
                    {
                        target.Dispose();
                    }

                    data.targets.Clear();
                }
            );
        }
    }

    private sealed class PopupTextSonar
    {
        public required int SonarItemType { get; init; } = -1;
    }

    private static readonly ConditionalWeakTable<PopupText, PopupTextSonar> sonar_texts = [];

    [OnLoad]
    private static void ApplyEdits()
    {
        On_PopupText.GetTextHitbox += GetTextHitbox_AddItemHitbox;
        On_PopupText.ResetText += ResetText_ResetSonarItem;
        On_Main.DrawItemTextPopups += DrawItemTextPopups_DrawSonarItemIcon;
        IL_Projectile.FishingCheck += FishingCheck_WrapAssignAsSonarText;
    }

    private static Vector2 GetTextHitbox_AddItemHitbox(On_PopupText.orig_GetTextHitbox orig, PopupText self)
    {
        var wh = orig(self);

        if (sonar_texts.TryGetValue(self, out var sonarText) && sonarText.SonarItemType != -1)
        {
            wh.Y += 32f * self.scale;
        }

        return wh;
    }

    private static void ResetText_ResetSonarItem(On_PopupText.orig_ResetText orig, PopupText text)
    {
        orig(text);

        _ = sonar_texts.Remove(text);
    }

    private static void DrawItemTextPopups_DrawSonarItemIcon(On_Main.orig_DrawItemTextPopups orig, float scaleTarget)
    {
        orig(scaleTarget);

        foreach (var popupText in Main.popupText)
        {
            if (!popupText.active || !popupText.sonar || !sonar_texts.TryGetValue(popupText, out var sonarText) || sonarText.SonarItemType == -1)
            {
                continue;
            }

            var text = popupText.name;
            if (popupText.stack > 1)
            {
                text = text + " (" + popupText.stack + ")";
            }

            var halfSize = FontAssets.MouseText.Value.MeasureString(text).X / 2f;
            var pos = new Vector2(
                popupText.position.X - Main.screenPosition.X + halfSize,
                popupText.position.Y - Main.screenPosition.Y - 20
            );

            var item = ContentSamples.ItemsByType[sonarText.SonarItemType];
            if (ModContent.GetInstance<Config>().GrayscaleItems)
            {
                using (AssetReplacer.Replace(TextureAssets.Item[item.type], IStatic<GrayscaleItemCache>.Instance.GetGrayscaleItem(item.type)))
                {
                    for (var i = 0; i < 4; i++)
                    {
                        var offsetPos = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 2;
                        DrawItem(item, pos + offsetPos, popupText.scale, Color.Blue * 0.4f);
                    }

                    DrawItem(item, pos, popupText.scale, popupText.color);
                }
            }
            else
            {
                DrawItem(item, pos, popupText.scale, Color.White);
            }
        }

        return;

        static void DrawItem(Item item, Vector2 position, float scale, Color color)
        {
            var itemType = item.type;

            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                return;
            }

            Main.instance.LoadItem(itemType);
            var texture = TextureAssets.Item[itemType].Value;
            var frame = Main.itemAnimations[item.type]?.GetFrame(texture) ?? texture.Frame();

            scale *= 0.75f;

            position.Y += frame.Height / 2f * (1f - scale);
            ItemSlot.DrawItemIcon(item, 14, Main.spriteBatch, position, scale, 32f, color);
        }
    }

    private static void FishingCheck_WrapAssignAsSonarText(ILContext il)
    {
        var c = new ILCursor(il);

        var attemptIdx = -1;
        c.GotoNext(x => x.MatchInitobj<FishingAttempt>());
        c.GotoPrev(x => x.MatchLdloca(out attemptIdx));
        Debug.Assert(attemptIdx != -1);

        c.Index = 0;

        while (c.TryGotoNext(MoveType.Before, x => x.MatchCall<PopupText>(nameof(PopupText.AssignAsSonarText))))
        {
            c.Remove();

            c.EmitLdloca(attemptIdx);

            c.EmitDelegate((int sonarTextIndex, ref FishingAttempt attempt) => Assign(sonarTextIndex, ref attempt));
        }
    }

    public static void Assign(int sonarTextIndex, ref FishingAttempt attempt)
    {
        PopupText.AssignAsSonarText(sonarTextIndex);

        if (sonarTextIndex < 0)
        {
            return;
        }

        // Three cases in which this is called:
        // - PlayerLoader.CatchFish sets Text to a non-null value
        // - rolledItemDrop > 0
        // - rolledEnemySpawn > 0
        // We will assume if `rolledItemDrop > 0` then we should just add the
        // icon.

        var popupText = Main.popupText[sonarTextIndex];

        // Sanity check?
        if (!popupText.sonar)
        {
            return;
        }

        if (attempt.rolledItemDrop <= 0)
        {
            return;
        }

        // popupText.name = $"[i:{attempt.rolledItemDrop}] " + popupText.name;
        sonar_texts.AddOrUpdate(
            popupText,
            new PopupTextSonar
            {
                SonarItemType = attempt.rolledItemDrop,
            }
        );
    }
}
