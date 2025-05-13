using System.Collections.Generic;

using Terraria.Localization;
using Terraria.ModLoader;

namespace Daybreak.Common.Features.Achievements;

/// <summary>
///     A modded achievement.  Unrelated to the vanilla
///     <see cref="Terraria.Achievements.Achievement"/> type, which this
///     implementation wholly replaces.
/// </summary>
public abstract class Achievement : ModType, ILocalizedModType
{
    /// <inheritdoc cref="ILocalizedModType.LocalizationCategory"/>
    public virtual string LocalizationCategory => "Achievements";

    /// <summary>
    ///     The display name (friendly name) of this achievement.
    /// </summary>
    public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);

    /// <summary>
    ///     The description of this achievement if it has one.
    /// </summary>
    public virtual LocalizedText Description => this.GetLocalization(nameof(Description), static () => "");

    protected sealed override void Register()
    {
        AchievementImpl.Register(this);
    }

    protected sealed override void InitTemplateInstance() { }

    /// <summary>
    ///     Gets the categories this achievement is in.  Achievements must
    ///     belong to at least one category, but may belong to an arbitrary
    ///     amount past that.
    /// </summary>
    public abstract IEnumerable<AchievementCategory> GetCategories();
}