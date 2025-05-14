using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria.Localization;
using Terraria.ModLoader;

namespace Daybreak.Common.Features.Achievements;

/// <summary>
///     An achievement category, which an achievement may belong to an arbitrary
///     number of.
/// </summary>
public abstract class AchievementCategory : ModType, ILocalizedModType
{
    /// <inheritdoc cref="ILocalizedModType.LocalizationCategory"/>
    public virtual string LocalizationCategory => "AchievementCategories";

    /// <summary>
    ///     The display name (friendly name) of this achievement category.
    /// </summary>
    public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);

    // TODO
    public virtual bool IsCompleted { get; set; } = false;

    internal int Id { get; set; }

    protected sealed override void Register()
    {
        AchievementImpl.RegisterCategory(this);
    }

    protected sealed override void InitTemplateInstance() { }

    public abstract Asset<Texture2D> GetIcon(out Rectangle frame);
}