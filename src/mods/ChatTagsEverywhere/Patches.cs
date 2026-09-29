using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace ChatTagsEverywhere;

public sealed class Patches : ModSystem
{
    public override void Load()
    {
        // TODO
        /*
        IL_Main.GUIHotbarDrawInner += il =>
        {
            // public static void DrawString(this SpriteBatch spriteBatch, DynamicSpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        };
        */

        IL_NewMultiplayerClosePlayersOverlay.PlayerOnScreenCache.DrawPlayerName_WhenPlayerIsOnScreen += il =>
        {
            var c = new ILCursor(il);

            c.EmitLdarg0();
            c.EmitLdarg1();
            c.EmitDelegate(
                (ref NewMultiplayerClosePlayersOverlay.PlayerOnScreenCache self, SpriteBatch sb) =>
                {
                    self._pos = self._pos.Floor();
                    ChatManager.DrawColorCodedStringWithShadow(
                        sb,
                        FontAssets.MouseText.Value,
                        self._name,
                        self._pos,
                        self._color,
                        0f,
                        Vector2.Zero,
                        Vector2.One
                    );
                }
            );
            c.EmitRet();
        };

        IL_NewMultiplayerClosePlayersOverlay.PlayerOffScreenCache.DrawPlayerDistance += il =>
        {
            var c = new ILCursor(il);

            c.EmitLdarg0();
            c.EmitLdarg1();
            c.EmitDelegate(
                (ref NewMultiplayerClosePlayersOverlay.PlayerOffScreenCache self, SpriteBatch sb) =>
                {
                    ChatManager.DrawColorCodedStringWithShadow(
                        sb,
                        FontAssets.MouseText.Value,
                        self.distanceString,
                        self.distanceDrawPosition,
                        self.namePlateColor,
                        0f,
                        Vector2.Zero,
                        new Vector2(0.85f)
                    );
                }
            );
            c.EmitRet();
        };

        IL_NewMultiplayerClosePlayersOverlay.Draw += _ => { };
    }
}
