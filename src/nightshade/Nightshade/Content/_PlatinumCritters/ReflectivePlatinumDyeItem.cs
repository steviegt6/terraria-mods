using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Dyes;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content;

public sealed class ReflectivePlatinumDyeItem : ModItem
{
    // copied from ReflectiveArmorShaderData
    private sealed class Shader(Asset<Effect> shader, string passName) : ArmorShaderData(shader, passName)
    {
        public override void Apply(Entity? entity, DrawData? drawData = null)
        {
            if (entity == null)
            {
                Shader.Parameters["uLightSource"]?.SetValue(Vector3.Zero);
            }
            else
            {
                var rotation = 0f;
                if (drawData.HasValue)
                {
                    rotation = drawData.Value.rotation;
                }

                var position = entity.position;
                float width = entity.width;
                float height = entity.height;
                var lightPos = position + new Vector2(width, height) * 0.1f;
                width *= 0.8f;
                height *= 0.8f;
                var subLight1 = Lighting.GetSubLight(lightPos + new Vector2(width * 0.5f, 0f));
                var subLight2 = Lighting.GetSubLight(lightPos + new Vector2(0f, height * 0.5f));
                var subLight3 = Lighting.GetSubLight(lightPos + new Vector2(width, height * 0.5f));
                var subLight4 = Lighting.GetSubLight(lightPos + new Vector2(width * 0.5f, height));
                var total1 = subLight1.X + subLight1.Y + subLight1.Z;
                var total2 = subLight2.X + subLight2.Y + subLight2.Z;
                var total3 = subLight3.X + subLight3.Y + subLight3.Z;
                var total4 = subLight4.X + subLight4.Y + subLight4.Z;
                var midTone = new Vector2(total3 - total2, total4 - total1);
                var midToneLength = midTone.Length();
                if (midToneLength > 1f)
                {
                    midToneLength = 1f;
                    midTone /= midToneLength;
                }

                if (entity.direction == -1)
                {
                    midTone.X *= -1f;
                }

                midTone = midTone.RotatedBy(0f - rotation);
                var lightSource = new Vector3(midTone, 1f - (midTone.X * midTone.X + midTone.Y * midTone.Y));
                lightSource.X *= 2f;
                lightSource.Y -= 0.15f;
                lightSource.Y *= 2f;
                lightSource.Normalize();
                lightSource.Z *= 0.6f;
                Shader.Parameters["uLightSource"]?.SetValue(lightSource);
            }

            UseColor(
                new Color(135, 135, 225) * 1.6f
            );

            base.Apply(entity, drawData);
        }
    }

    public override string Texture => Assets.Images.Items.Misc.PlatinumDye.KEY;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
        {
            Main.QueueMainThreadAction(
                () =>
                {
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new Shader(Assets.Shaders.Misc.ReflectivePlatinumShader.Asset, "ArmorReflectiveColor")
                    );
                }
            );
        }
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        var dye = Item.dye;
        Item.CloneDefaults(ItemID.ReflectiveGoldDye);
        Item.dye = dye;
    }
}