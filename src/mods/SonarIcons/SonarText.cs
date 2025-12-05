using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SonarIcons;

public sealed class SonarText : ModSystem
{
    private static int sonarItemType = -1;

    public override void Load()
    {
        base.Load();

        On_PopupText.ClearSonarText += ClearSonarText_ClearSonarItem;
        On_Main.DrawItemTextPopups += DrawItemTextPopups_DrawSonarItemIcon;
        IL_Projectile.FishingCheck += FishingCheck_WrapAssignAsSonarText;
    }

    private static void ClearSonarText_ClearSonarItem(On_PopupText.orig_ClearSonarText orig)
    {
        sonarItemType = -1;
    }

    private static void DrawItemTextPopups_DrawSonarItemIcon(On_Main.orig_DrawItemTextPopups orig, float scaleTarget)
    {
        orig(scaleTarget);

        if (sonarItemType == -1 || PopupText.sonarText == -1 || !Main.popupText[PopupText.sonarText].sonar)
        {
            return;
        }

        var popupText = Main.popupText[PopupText.sonarText];
        if (!popupText.active)
        {
            return;
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

        // TODO: An option to draw the item like a text, using a shader to
        //       grayscale the texture and tint the body and outlines
        //       accordingly (blue outline, rarity-colored body).
        /*if (true)
        {
            for (var i = 0; i < 4; i++)
            {
                var offsetPos = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 2;
                DrawItem(ContentSamples.ItemsByType[sonarItemType], pos + offsetPos, popupText.scale, Color.White);
            }
        }*/

        DrawItem(ContentSamples.ItemsByType[sonarItemType], pos, popupText.scale, Color.White);

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
        sonarItemType = attempt.rolledItemDrop;
    }
}
