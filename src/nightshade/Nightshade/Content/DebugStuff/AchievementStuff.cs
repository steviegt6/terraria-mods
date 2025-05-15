using System.Collections.Generic;

using Daybreak.Common.Features.Achievements;
using Daybreak.Common.IDs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Content.DebugStuff;

internal abstract class AbstractCategory(int key) : AchievementCategory
{
    public override Asset<Texture2D> GetIcon(out Rectangle frame)
    {
        Assets.Images.Categories.Asset.Wait();
        frame = Assets.Images.Categories.Asset.Frame(3, 2, key);
        return Assets.Images.Categories.Asset;
    }
}

internal sealed class Category1() : AbstractCategory(0);

internal sealed class Category2() : AbstractCategory(1);

internal sealed class Category3() : AbstractCategory(2);

internal sealed class Achievement1 : Achievement
{
    public override string Texture => Assets.Images.Achievement1.KEY;

    public override IEnumerable<AchievementCategory> GetCategories()
    {
        yield return VanillaAchievements.Categories.Challenger;
    }
}

internal sealed class Achievement2 : Achievement
{
    public override string Texture => Assets.Images.Achievement2.KEY;

    public override IEnumerable<AchievementCategory> GetCategories()
    {
        yield return VanillaAchievements.Categories.Explorer;
        yield return ModContent.GetInstance<Category1>();
        yield return ModContent.GetInstance<Category2>();
    }
}

internal sealed class Achievement3 : Achievement
{
    public override string Texture => Assets.Images.Achievement3.KEY;

    public override IEnumerable<AchievementCategory> GetCategories()
    {
        yield return ModContent.GetInstance<Category3>();
    }
}

internal sealed class Test : ModPlayer
{
    public override void OnEnterWorld()
    {
        base.OnEnterWorld();

        ModContent.GetInstance<Achievement1>().Complete();
    }
}