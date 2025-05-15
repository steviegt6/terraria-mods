using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria.Localization;
using Terraria.ModLoader;

namespace Daybreak.Common.Features.Achievements;

/// <summary>
///     A modded achievement.  Unrelated to the vanilla
///     <see cref="Terraria.Achievements.Achievement"/> type, which this
///     implementation wholly replaces.
/// </summary>
public abstract class Achievement : ModTexturedType, ILocalizedModType
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

    /// <summary>
    ///     The order of this achievement in the context of the achievement
    ///     advisor cards.  Return a value below <c>0f</c> to omit it from the
    ///     advisor card system.
    /// </summary>
    public virtual float AdvisorOrder { get; } = -1f;

    /// <summary>
    ///     Whether this achievement has been completed.
    /// </summary>
    public bool IsCompleted => AchievementImpl.GetCompletedStatus(this);

    internal int Id { get; set; }

    protected sealed override void Register()
    {
        AchievementImpl.Register(this);
        ModTypeLookup<Achievement>.Register(this);
    }

    protected sealed override void InitTemplateInstance() { }

    public void Complete()
    {
        AchievementImpl.Complete(this);
    }

    /// <summary>
    ///     Gets the categories this achievement is in.  Achievements must
    ///     belong to at least one category, but may belong to an arbitrary
    ///     amount past that.
    /// </summary>
    public abstract IEnumerable<AchievementCategory> GetCategories();

    /// <summary>
    ///     Whether this achievement is available to be earned in the current
    ///     world.  Used for the achievement advisor cards.
    /// </summary>
    public virtual bool IsPresentlyAvailable()
    {
        return true;
    }

    /// <summary>
    ///     Gets the texture and frame used for the advisor card icon.
    /// </summary>
    public virtual Asset<Texture2D> GetIcon(out Rectangle frame, out int lockedOffset)
    {
        frame = new Rectangle(0, 0, 64, 64);
        lockedOffset = 66;
        return ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad);
    }
}