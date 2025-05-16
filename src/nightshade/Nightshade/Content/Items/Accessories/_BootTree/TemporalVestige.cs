using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class TemporalVestige : ModItem
{
    public override string Texture => Assets.Images.Items.Accessories.TemporalVestige.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.accessory = true;
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        // TODO: other effects
        player.GetJumpState<SandstormInABottleJump>().Enable();
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        // TODO: other items
        CreateRecipe()
           .AddIngredient(ItemID.SandstorminaBottle)
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}