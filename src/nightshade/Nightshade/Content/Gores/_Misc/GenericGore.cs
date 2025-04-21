using Terraria.ModLoader;

namespace Nightshade.Content.Gores;

internal sealed class GenericGore : ModGore
{
    public GenericGore(string name, string texture)
    {
        nameOverride = name;
        textureOverride = texture;
    }
}