using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

public sealed class DriftersBoots : ModItem
{
	public sealed class DrifterBootsJump : ExtraJump
	{
        public override Position GetDefaultPosition() => BeforeBottleJumps;

        public override float GetDurationMultiplier(Player player) => 1f;

		public override void OnStarted(Player player, ref bool playSound)
		{
			base.OnStarted(player, ref playSound);

            player.velocity.X += Math.Sign(player.velocity.X);
            player.runAcceleration *= 5f;

			if (--player.GetModPlayer<DrifterBootsPlayer>().JumpCount > 0)
			{
				player.GetJumpState(this).Available = true;
			}
		}

		public override void OnRefreshed(Player player)
		{
			base.OnRefreshed(player);

			// lmao thank god example mod basically had an example that exactly
			// matched everything we wanted
			player.GetModPlayer<DrifterBootsPlayer>().JumpCount = 4;
		}

		public override void ShowVisuals(Player player)
		{
			base.ShowVisuals(player);
		}

		public override void UpdateHorizontalSpeeds(Player player)
		{
            base.UpdateHorizontalSpeeds(player);

            player.maxRunSpeed *= 2f;
            player.runAcceleration *= 2f;
		}
	}

	public sealed class DrifterBootsPlayer : ModPlayer
    {
        public int JumpCount { get; set; }

		public override void Load()
		{
			On_Player.JumpMovement += CustomDrifterBootsJump;
		}

        // Run our custom motion after jump code
		private void CustomDrifterBootsJump(On_Player.orig_JumpMovement orig, Player self)
		{
			if (self.GetJumpState<DrifterBootsJump>().Active && self.controlJump)
			{
				self.gravity = 0.1f;

                if (self.TouchedTiles.Count > 0)
                {
					self.jump = 0;
                    return;
				}

				float maxJump = Player.jumpHeight * ModContent.GetInstance<DrifterBootsJump>().GetDurationMultiplier(self);

				if (self.controlUp)
				{
					self.velocity.Y -= 0.4f;
				}
				else if (self.controlDown)
                {
					if (self.jump >= maxJump - 1)
					{
						self.velocity.Y = 0f;
					}
					self.velocity.Y += 0.7f;
                }

				self.jump--;
				self.velocity.Y = MathHelper.Lerp(self.velocity.Y, -4f, 0.05f);

				for (int i = 0; i < 12; i++)
				{
					Vector2 dustPos = self.RotatedRelativePoint(self.Bottom + Main.rand.NextVector2Circular(16, 8));
					Dust d = Dust.NewDustPerfect(dustPos, DustID.SandstormInABottle, self.velocity * Main.rand.NextFloat(), Scale: Main.rand.NextFloat(0.5f, 1f));
					d.noGravity = true;
				}
			}
			else
				orig(self);
		}
	}

    public override string Texture => Assets.Images.Items.Accessories.DriftersBoots.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (34, 32);

        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        // TODO: other effects
        player.accRunSpeed = 6f;
        player.desertBoots = true;
        player.GetJumpState<DrifterBootsJump>().Enable();

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
           .AddIngredient(ItemID.RocketBoots)
           .AddIngredient(ItemID.SandBoots)
           .AddIngredient<TemporalVestige>()
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}