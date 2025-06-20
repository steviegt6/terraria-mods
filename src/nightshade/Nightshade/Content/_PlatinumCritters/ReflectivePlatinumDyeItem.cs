using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Dyes;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content;

public sealed class ReflectivePlatinumDyeItem : ModItem
{
    private sealed class Shader : ReflectiveArmorShaderData
    {
        public Shader(Ref<Effect> shader, string passName) : base(shader, passName) { }

        public override void Apply(Entity entity, DrawData? drawData)
        {
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
                    var effect = Assets.Shaders.Misc.ReflectivePlatinumShader.Asset;
                    effect.Wait();
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new Shader(new Ref<Effect>(effect.Value), "ArmorReflectiveColor")
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