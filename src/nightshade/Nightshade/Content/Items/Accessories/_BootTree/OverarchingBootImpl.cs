using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

/// <summary>
///     Implementation-relevant details for implementing our revised boot trees.
/// </summary>
internal sealed class OverarchingBootImpl : ModSystem
{
    public override void PostAddRecipes()
    {
        base.PostAddRecipes();
        
        // Remove Dunerider Boots -> Spectre Boots recipe since the trees
        // diverge.
        foreach (var recipe in Main.recipe)
        {
            if (!recipe.HasResult(ItemID.SpectreBoots))
            {
                continue;
            }

            if (recipe.HasIngredient(ItemID.SandBoots))
            {
                recipe.DisableRecipe();
            }
        }
    }
}