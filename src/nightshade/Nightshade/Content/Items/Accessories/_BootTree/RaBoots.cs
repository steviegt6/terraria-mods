using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class RaBoots : ModItem
{
    private sealed class RaBootsJump : ExtraJump
    {
        public override Position GetDefaultPosition()
        {
            return BeforeBottleJumps;
        }

        public override float GetDurationMultiplier(Player player)
        {
            return 1f;
        }

        public override void OnStarted(Player player, ref bool playSound)
        {
            base.OnStarted(player, ref playSound);

            // TODO: Unique effect?
            
            // Show dust visuals.
            {
                var heightOffset = player.height;
                if (player.gravDir <= -1f)
                {
                    heightOffset = -6;
                }

                var intensity = (player.jump / 75f + 1f) / 2f;
                for (var i = 0; i < 12; i++)
                {
                    var dust = Dust.NewDustDirect(
                        new Vector2(player.position.X, player.position.Y + heightOffset / 2f),
                        player.width,
                        32,
                        DustID.SandstormInABottle,
                        player.velocity.X * 0.3f,
                        player.velocity.Y * 0.3f,
                        150,
                        default(Color),
                        1f * intensity
                    );
                    dust.velocity *= 0.5f * intensity;
                    dust.fadeIn = 1.5f * intensity;
                }

                var gore = Gore.NewGoreDirect(
                    new Vector2(player.position.X + player.width / 2f - 18f, player.position.Y + heightOffset / 2f),
                    new Vector2(0f - player.velocity.X, 0f - player.velocity.Y),
                    Main.rand.Next(220, 223),
                    intensity
                );
                gore.velocity = player.velocity * 0.3f * intensity;
                gore.alpha = 100;
            }

            // So long as there are jumps left, let this jump remain usable.
            if (--player.GetModPlayer<RbPlayer>().JumpCount > 0)
            {
                player.GetJumpState(this).Available = true;
            }
        }

        public override void OnRefreshed(Player player)
        {
            base.OnRefreshed(player);

            player.GetModPlayer<RbPlayer>().JumpCount = 5;
        }
    }

    public sealed class RbPlayer : ModPlayer
    {
        public int JumpCount { get; set; }
    }
    
    public override string Texture => "ModLoader/UnloadedItem";

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.accessory = true;
    }
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        // TODO: other effects
        player.accRunSpeed = 8.5f;
        player.desertBoots = true;

        player.GetJumpState<RaBootsJump>().Enable();

        // Purposefully no Rocket Boots at this level.
        // player.rocketBoots = player.vanityRocketBoots = 2;

        if (!hideVisual)
        {
            DoVanity(player);
        }
    }

    public override void UpdateVanity(Player player)
    {
        base.UpdateVanity(player);

        DoVanity(player);
    }

    private static void DoVanity(Player player)
    {
        player.CancelAllBootRunVisualEffects();
        player.desertDash = true;
    }
    
    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
           .AddIngredient<DriftersBoots>()
           .AddIngredient<SunGodEye>()
           .AddIngredient(ItemID.SoulofFlight, 10)
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}