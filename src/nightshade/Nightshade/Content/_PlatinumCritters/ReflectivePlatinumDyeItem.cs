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
        private static readonly Color platinum_color_one = new Color(255, 220, 255);
        private static readonly Color platinum_color_two = new Color(220, 255, 255);

        public Shader(Ref<Effect> shader, string passName) : base(shader, passName) { }

        public override void Apply(Entity entity, DrawData? drawData)
        {
            UseColor(
                Color.SlateBlue * 2.65f
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
            GameShaders.Armor.BindShader(Item.type, new Shader(Main.PixelShaderRef, "ArmorReflectiveColor"));
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