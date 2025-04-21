using Terraria.ModLoader;

namespace Nightshade.Content.Gores;
public class GenericGore : ModGore
{
    public override string Texture => _privTexture;
    public override string Name => _privName;
    protected string _privTexture { get; private set; }
    protected string _privName { get; private set; }

    public GenericGore(string Name, string texPath) : base()
    {
        _privName = Name;
        _privTexture = texPath;
    }
}
