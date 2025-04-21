using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Nightshade.Common.Loading;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

using Hook = Nightshade.Common.Hooks.ItemRendering.IModifyInWorldRender;

namespace Nightshade.Common.Hooks.ItemRendering;

/// <summary>
///     Provides a way to reliably modify the way an item is drawn according to
///     vanilla logic by allowing one to override the texture and position.
///     <br />
///     This hook should generally be avoided in favor of
///     <see cref="IModifyItemDrawBasics"/>.
/// </summary>
public interface IModifyInWorldRender
{
    private sealed class ModifyInWorldRenderImplementation : ILoad
    {
        void ILoad.Load()
        {
            IL_Main.DrawItem += il =>
            {
                var c = new ILCursor(il);

                c.GotoNext(x => x.MatchLdfld<Item>(nameof(Item.glowMask)));

                var itemArg = 0;
                c.GotoPrev(MoveType.Before, x => x.MatchLdarg(out itemArg));

                var earliestIndexInCondition = c.Index;

                c.Index = 0;

                var textureLoc = 0;
                c.GotoNext(x => x.MatchCallvirt<Main>(nameof(Main.LoadItem)));
                c.GotoNext(x => x.MatchLdloca(out textureLoc));

                var positionLoc = 0;
                c.GotoNext(x => x.MatchLdfld<Entity>(nameof(Entity.position)));
                c.GotoNext(x => x.MatchStloc(out positionLoc));

                c.Index = earliestIndexInCondition;

                c.EmitLdarg(itemArg);
                c.EmitLdloca(textureLoc);
                c.EmitLdloca(positionLoc);
                c.EmitDelegate(
                    static (Item item, ref Texture2D texture, ref Vector2 position) =>
                    {
                        Invoke(item, ref texture, ref position);
                    }
                );
            };
        }
    }

    public static readonly GlobalHookList<GlobalItem> HOOK = ItemLoader.AddModHook(
        GlobalHookList<GlobalItem>.Create(x => ((Hook)x).ModifyInWorldRender)
    );

    void ModifyInWorldRender(Item item, ref Texture2D texture, ref Vector2 position);

    public static void Invoke(Item item, ref Texture2D texture, ref Vector2 position)
    {
        foreach (var g in HOOK.Enumerate(item))
        {
            if (g is not Hook hook)
            {
                continue;
            }

            hook.ModifyInWorldRender(item, ref texture, ref position);
        }
    }
}