using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content;

internal sealed class GoldCritterShader : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return NPCID.Sets.GoldCrittersCollection.Contains(entity.type);
    }

    public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (!npc.IsABestiaryIconDummy)
        {
            spriteBatch.End();
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                Main.Transform
            );
        }

        GameShaders.Armor.GetShaderFromItemId(ItemID.ReflectiveGoldDye)
                   .Apply(npc, new DrawData(TextureAssets.Npc[npc.type].Value, npc.position, drawColor));

        return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
    }

    public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();

        if (!npc.IsABestiaryIconDummy)
        {
            spriteBatch.End();
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.Transform
            );
        }
        base.PostDraw(npc, spriteBatch, screenPos, drawColor);
    }
}