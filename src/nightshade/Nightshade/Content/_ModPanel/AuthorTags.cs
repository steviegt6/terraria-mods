using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace Nightshade.Content;

internal abstract class CommonAuthorTag : AuthorTag
{
    private const string suffix = "Tag";

    public override string Name => base.Name.EndsWith(suffix) ? base.Name[..^suffix.Length] : base.Name;

    public override string Texture => string.Join('/', Assets.Images.UI.AuthorTags.Tomat.KEY.Split('/')[..^1]) + '/' + Name;
}

internal sealed class TomatTag : CommonAuthorTag;

internal sealed class BabyBlueSheepTag : CommonAuthorTag;

internal sealed class CitrusTag : CommonAuthorTag;

internal sealed class EbonflyTag : CommonAuthorTag;

internal sealed class BlockarozTag : CommonAuthorTag
{
    public override void DrawIcon(SpriteBatch sb, Vector2 position)
    {
        base.DrawIcon(sb, position);

        sb.Draw(
            Assets.Images.UI.AuthorTags.Blockaroz_Overlay.Asset.Value,
            new Rectangle((int)position.X, (int)position.Y - 2, 26, 26),
            Color.Red with { A = 20 } * Math.Abs(MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi * 0.1f))
        );
    }
}

internal sealed class TyeskiTag : CommonAuthorTag;

internal sealed class TyrantTag : CommonAuthorTag;

internal sealed class FredTag : CommonAuthorTag;

internal sealed class DreitoneTag : CommonAuthorTag;

internal sealed class Haram64Tag : CommonAuthorTag;

internal sealed class SixtyDegreesTag : CommonAuthorTag;

internal sealed class TriangleTag : CommonAuthorTag;

internal sealed class WymsicalTag : CommonAuthorTag;