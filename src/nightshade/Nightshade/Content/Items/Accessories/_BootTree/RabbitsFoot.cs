using System.Linq;

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class RabbitsFoot : ModItem
{
    private sealed class RfGlobalNpc : GlobalNPC
    {
        private static readonly int[] bunnies_normal =
        [
            NPCID.Bunny,
            NPCID.BunnySlimed,
            NPCID.BunnyXmas,
            NPCID.PartyBunny,
        ];

        private static readonly int[] bunnies_gem =
        [
            NPCID.GemBunnyAmethyst,
            NPCID.GemBunnyTopaz,
            NPCID.GemBunnySapphire,
            NPCID.GemBunnyEmerald,
            NPCID.GemBunnyRuby,
            NPCID.GemBunnyDiamond,
            NPCID.GemBunnyAmber,
        ];

        // TODO: we should add platinum critters lol
        private static readonly int[] bunnies_gold =
        [
            NPCID.GoldBunny,
        ];

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            base.ModifyNPCLoot(npc, npcLoot);
            
            if (bunnies_normal.Contains(npc.type))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RabbitsFoot>(), 50));
            }
            else if (bunnies_gem.Contains(npc.type))
            {
                // Gem bunnies are infrequent but easily farm-able I think?
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RabbitsFoot>(), 25));
            }
            else if (bunnies_gold.Contains(npc.type))
            {
                // Guaranteed drop for gold critters because you earned it.
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RabbitsFoot>()));
            }
        }
    }

    public sealed class RfPlayer : ModPlayer
    {
        public bool HasLuck { get; set; }

        public override void ResetEffects()
        {
            base.ResetEffects();

            HasLuck = false;
        }

        public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer)
        {
            base.RefreshInfoAccessoriesFromTeamPlayers(otherPlayer);

            if (otherPlayer.GetModPlayer<RfPlayer>().HasLuck)
            {
                HasLuck = true;
            }

            // Because this code is normally ran right after
            // RefreshInfoAccsFromTeamPlayers.
            if (HasLuck)
            {
                Player.equipmentBasedLuckBonus += 0.05f;
            }
        }
    }

    public override string Texture => Assets.Images.Items.Accessories.RabbitsFoot.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.accessory = true;
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.GetModPlayer<RfPlayer>().HasLuck = true;
        player.moveSpeed += 0.05f; // Same as Aglet.
    }
}