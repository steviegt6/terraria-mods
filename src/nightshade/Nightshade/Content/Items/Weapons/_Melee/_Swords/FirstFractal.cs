using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Weapons;

internal sealed class FirstFractal : GlobalItem
{
    private sealed class ChangeZenithRecipeToIncludeFirstFractal : ModSystem
    {
        public override void PostSetupRecipes()
        {
            base.PostSetupRecipes();

            foreach (var recipe in Main.recipe)
            {
                if (!recipe.HasResult(ItemID.Zenith))
                {
                    continue;
                }

                recipe.RemoveIngredient(ItemID.StarWrath);
                recipe.RemoveIngredient(ItemID.TerraBlade);
                recipe.AddIngredient(item_id);
            }
        }
    }

    private const int item_id = ItemID.FirstFractal;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        ItemID.Sets.Deprecated[item_id] = false;

        Lang._itemNameCache[item_id] = ItemName.FirstFractal.GetText();
    }

    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == item_id;
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        Recipe.Create(item_id)
              .AddIngredient(ItemID.StarWrath)
              .AddIngredient(ItemID.TerraBlade)
              .AddIngredient(ItemID.LunarBar, 10)
              .AddTile(TileID.LunarCraftingStation)
              .Register();
    }
}