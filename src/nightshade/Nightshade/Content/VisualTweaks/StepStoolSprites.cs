using Daybreak.Common.Assets;

using JetBrains.Annotations;

using Nightshade.Common.Loading;

using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.VisualTweaks;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class StepStoolSprites : ILoad
{
    private sealed class TrackHoC : ModPlayer
    {
        public bool IsEquipped { get; set; }

        public override void ResetEffects()
        {
            base.ResetEffects();

            IsEquipped = false;
        }
    }

    private sealed class TrackHoCItem : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            base.UpdateAccessory(item, player, hideVisual);

            if (item.type == ItemID.HandOfCreation)
            {
                player.GetModPlayer<TrackHoC>().IsEquipped = true;
            }
        }
    }

    void ILoad.Load()
    {
        // Replace the stool texture for the entire draw player routine because
        // other mods may modify the method strangely (CalRemix).
        On_LegacyPlayerRenderer.DrawPlayer += (orig, self, camera, player, position, rotation, origin, shadow, scale) =>
        {
            if (!player.GetModPlayer<TrackHoC>().IsEquipped)
            {
                orig(self, camera, player, position, rotation, origin, shadow, scale);
                return;
            }

            using (AssetReplacer.Extra(ExtrasID.PortableStool, Assets.Images.Items.Accessories.HandOfCreationStool.Asset.Value))
            {
                orig(self, camera, player, position, rotation, origin, shadow, scale);
            }
        };
    }
}