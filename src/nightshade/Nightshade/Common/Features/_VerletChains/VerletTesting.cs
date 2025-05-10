
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Common.Features._VerletChains
{
    internal class VerletTesting : ModItem
    {
        public override string Texture => Assets.Images.Items.Furniture.CoconutChest.KEY;

        private VerletIntegratedBody chain = new VerletIntegratedBody(Vector2.Zero, Vector2.One, 10, 20);

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                chain.SetLastPosition(Main.MouseScreen);
            }
            else
            {
                chain.SetFirstPosition(Main.MouseScreen);
            }
            return base.UseItem(player);
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            chain.Update();
            foreach (Vector2 point in chain.GetPointPositions())
            {
                Texture2D texture = TextureAssets.Chains[ChainID.LunarSolar].Value;
                spriteBatch.Draw(texture, point, texture.Frame(), Color.White, 0, texture.Size() / 2f, 1f, SpriteEffects.None, 0);
            }
        }
    }
}
