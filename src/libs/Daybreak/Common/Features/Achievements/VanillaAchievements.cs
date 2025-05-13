using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Daybreak.Common.Features.Achievements;

// TODO: [DependsOn<AchievementImpl>]
internal sealed class VanillaAchievements : ModSystem
{
    private sealed class VanillaCategory : AchievementCategory
    {
        public override string Name { get; }

        public VanillaCategory(string name)
        {
            Name = "Terraria/" + name;
            VANILLA_CATEGORIES_BY_NAME[name] = this;
        }
    }

    private sealed class VanillaAchievement : Achievement
    {
        public override string Name { get; }

        public override LocalizedText DisplayName => Language.GetText($"Achievements.{key}_Name");

        public override LocalizedText Description => Language.GetText($"Achievements.{key}_Description");

        public override float AdvisorOrder => orders.GetValueOrDefault(key, -1f);

        private readonly string key;

        private const int icon_size = 64;
        private const int icon_size_with_space = 66;
        private const int icons_per_row = 8;

        public VanillaAchievement(string name)
        {
            Name = "Terraria/" + name;
            VANILLA_ACHIEVEMENTS_BY_NAME[key = name] = this;
        }

        public override IEnumerable<AchievementCategory> GetCategories()
        {
            yield return categories[key] switch
            {
                Terraria.Achievements.AchievementCategory.None => throw new InvalidOperationException("Vanilla category '" + key + "' is none."),
                Terraria.Achievements.AchievementCategory.Slayer => IDs.VanillaAchievements.Categories.Slayer,
                Terraria.Achievements.AchievementCategory.Collector => IDs.VanillaAchievements.Categories.Collector,
                Terraria.Achievements.AchievementCategory.Explorer => IDs.VanillaAchievements.Categories.Explorer,
                Terraria.Achievements.AchievementCategory.Challenger => IDs.VanillaAchievements.Categories.Challenger,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public override bool IsPresentlyAvailable()
        {
            return key switch
            {
                "MASTERMIND" => WorldGen.crimson,
                "WORM_FODDER" => !WorldGen.crimson,
                "PLAY_ON_A_SPECIAL_SEED" => Main.specialSeedWorld,
                _ => true,
            };
        }

        public override Asset<Texture2D> GetIcon(out Rectangle frame, out int lockedOffset)
        {
            var idx = icon_indices[key];
            frame = new Rectangle(
                idx % icons_per_row * icon_size_with_space,
                idx / icons_per_row * icon_size_with_space,
                icon_size,
                icon_size
            );
            lockedOffset = 528;
            return Main.Assets.Request<Texture2D>("Images/UI/Achievements");
        }
    }

    internal static readonly Dictionary<string, AchievementCategory> VANILLA_CATEGORIES_BY_NAME = [];
    internal static readonly Dictionary<string, Achievement> VANILLA_ACHIEVEMENTS_BY_NAME = [];

    private static readonly Dictionary<string, int> icon_indices = [];
    private static readonly Dictionary<string, float> orders = [];
    private static readonly Dictionary<string, Terraria.Achievements.AchievementCategory> categories = [];

    private static readonly string[] vanilla_categories =
    [
        "Slayer", "Collector", "Explorer", "Challenger",
    ];

    public override void Load()
    {
        base.Load();

        foreach (var category in vanilla_categories)
        {
            Mod.AddContent(new VanillaCategory(category));
        }

        var num = 0;
        icon_indices["TIMBER"] = num++;
        icon_indices["NO_HOBO"] = num++;
        icon_indices["OBTAIN_HAMMER"] = num++;
        icon_indices["HEART_BREAKER"] = num++;
        icon_indices["OOO_SHINY"] = num++;
        icon_indices["HEAVY_METAL"] = num++;
        icon_indices["I_AM_LOOT"] = num++;
        icon_indices["STAR_POWER"] = num++;
        icon_indices["HOLD_ON_TIGHT"] = num++;
        icon_indices["EYE_ON_YOU"] = num++;
        icon_indices["SMASHING_POPPET"] = num++;
        icon_indices["WORM_FODDER"] = num++;
        icon_indices["MASTERMIND"] = num++;
        icon_indices["WHERES_MY_HONEY"] = num++;
        icon_indices["STING_OPERATION"] = num++;
        icon_indices["BONED"] = num++;
        icon_indices["DUNGEON_HEIST"] = num++;
        icon_indices["ITS_GETTING_HOT_IN_HERE"] = num++;
        icon_indices["MINER_FOR_FIRE"] = num++;
        icon_indices["STILL_HUNGRY"] = num++;
        icon_indices["ITS_HARD"] = num++;
        icon_indices["BEGONE_EVIL"] = num++;
        icon_indices["EXTRA_SHINY"] = num++;
        icon_indices["HEAD_IN_THE_CLOUDS"] = num++;
        icon_indices["LIKE_A_BOSS"] = num++;
        icon_indices["BUCKETS_OF_BOLTS"] = num++;
        icon_indices["DRAX_ATTAX"] = num++;
        icon_indices["PHOTOSYNTHESIS"] = num++;
        icon_indices["GET_A_LIFE"] = num++;
        icon_indices["THE_GREAT_SOUTHERN_PLANTKILL"] = num++;
        icon_indices["TEMPLE_RAIDER"] = num++;
        icon_indices["LIHZAHRDIAN_IDOL"] = num++;
        icon_indices["ROBBING_THE_GRAVE"] = num++;
        icon_indices["BIG_BOOTY"] = num++;
        icon_indices["FISH_OUT_OF_WATER"] = num++;
        icon_indices["OBSESSIVE_DEVOTION"] = num++;
        icon_indices["STAR_DESTROYER"] = num++;
        icon_indices["CHAMPION_OF_TERRARIA"] = num++;
        icon_indices["BLOODBATH"] = num++;
        icon_indices["GOBLIN_PUNTER"] = num++;
        icon_indices["KILL_THE_SUN"] = num++;
        icon_indices["WALK_THE_PLANK"] = num++;
        icon_indices["DO_YOU_WANT_TO_SLAY_A_SNOWMAN"] = num++;
        icon_indices["TIN_FOIL_HATTER"] = num++;
        icon_indices["BALEFUL_HARVEST"] = num++;
        icon_indices["ICE_SCREAM"] = num++;
        icon_indices["SLIPPERY_SHINOBI"] = num++;
        icon_indices["STICKY_SITUATION"] = num++;
        icon_indices["REAL_ESTATE_AGENT"] = num++;
        icon_indices["NOT_THE_BEES"] = num++;
        icon_indices["JEEPERS_CREEPERS"] = num++;
        icon_indices["FUNKYTOWN"] = num++;
        icon_indices["INTO_ORBIT"] = num++;
        icon_indices["ROCK_BOTTOM"] = num++;
        icon_indices["MECHA_MAYHEM"] = num++;
        icon_indices["GELATIN_WORLD_TOUR"] = num++;
        icon_indices["FASHION_STATEMENT"] = num++;
        icon_indices["VEHICULAR_MANSLAUGHTER"] = num++;
        icon_indices["BULLDOZER"] = num++;
        icon_indices["THERE_ARE_SOME_WHO_CALL_HIM"] = num++;
        icon_indices["DECEIVER_OF_FOOLS"] = num++;
        icon_indices["SWORD_OF_THE_HERO"] = num++;
        icon_indices["LUCKY_BREAK"] = num++;
        icon_indices["THROWING_LINES"] = num++;
        icon_indices["DYE_HARD"] = num++;
        icon_indices["FREQUENT_FLYER"] = num++;
        icon_indices["THE_CAVALRY"] = num++;
        icon_indices["COMPLETELY_AWESOME"] = num++;
        icon_indices["TIL_DEATH"] = num++;
        icon_indices["ARCHAEOLOGIST"] = num++;
        icon_indices["PRETTY_IN_PINK"] = num++;
        icon_indices["RAINBOWS_AND_UNICORNS"] = num++;
        icon_indices["YOU_AND_WHAT_ARMY"] = num++;
        icon_indices["PRISMANCER"] = num++;
        icon_indices["IT_CAN_TALK"] = num++;
        icon_indices["WATCH_YOUR_STEP"] = num++;
        icon_indices["MARATHON_MEDALIST"] = num++;
        icon_indices["GLORIOUS_GOLDEN_POLE"] = num++;
        icon_indices["SERVANT_IN_TRAINING"] = num++;
        icon_indices["GOOD_LITTLE_SLAVE"] = num++;
        icon_indices["TROUT_MONKEY"] = num++;
        icon_indices["FAST_AND_FISHIOUS"] = num++;
        icon_indices["SUPREME_HELPER_MINION"] = num++;
        icon_indices["TOPPED_OFF"] = num++;
        icon_indices["SLAYER_OF_WORLDS"] = num++;
        icon_indices["YOU_CAN_DO_IT"] = num++;
        icon_indices["SICK_THROW"] = num++;
        icon_indices["MATCHING_ATTIRE"] = num++;
        icon_indices["BENCHED"] = num++;
        icon_indices["DEFEAT_QUEEN_SLIME"] = num++;
        icon_indices["DEFEAT_EMPRESS_OF_LIGHT"] = num++;
        icon_indices["GET_ZENITH"] = num++;
        icon_indices["FIND_A_FAIRY"] = num++;
        icon_indices["DEFEAT_DREADNAUTILUS"] = num++;
        icon_indices["DEFEAT_OLD_ONES_ARMY_TIER3"] = num++;
        icon_indices["FLY_A_KITE_ON_A_WINDY_DAY"] = num++;
        icon_indices["TURN_GNOME_TO_STATUE"] = num++;
        icon_indices["TALK_TO_NPC_AT_MAX_HAPPINESS"] = num++;
        icon_indices["GET_TERRASPARK_BOOTS"] = num++;
        icon_indices["THROW_A_PARTY"] = num++;
        icon_indices["PET_THE_PET"] = num++;
        icon_indices["GO_LAVA_FISHING"] = num++;
        icon_indices["FOUND_GRAVEYARD"] = num++;
        icon_indices["DIE_TO_DEAD_MANS_CHEST"] = num++;
        icon_indices["DEFEAT_DEERCLOPS"] = num++;
        icon_indices["GET_GOLDEN_DELIGHT"] = num++;
        icon_indices["DRINK_BOTTLED_WATER_WHILE_DROWNING"] = num++;
        icon_indices["GET_CELL_PHONE"] = num++;
        icon_indices["GET_ANKH_SHIELD"] = num++;
        icon_indices["GAIN_TORCH_GODS_FAVOR"] = num++;
        icon_indices["PLAY_ON_A_SPECIAL_SEED"] = num++;
        icon_indices["ALL_TOWN_SLIMES"] = num++;
        icon_indices["TRANSMUTE_ITEM"] = num++;
        icon_indices["PURIFY_ENTIRE_WORLD"] = num++;
        icon_indices["TO_INFINITY_AND_BEYOND"] = num++;
        _ = num;

        categories["EYE_ON_YOU"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["SLIPPERY_SHINOBI"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["WORM_FODDER"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["MASTERMIND"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["STING_OPERATION"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["DEFEAT_DEERCLOPS"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["BONED"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["STILL_HUNGRY"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["DEFEAT_DREADNAUTILUS"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["DEFEAT_QUEEN_SLIME"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["BUCKETS_OF_BOLTS"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["THE_GREAT_SOUTHERN_PLANTKILL"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["LIHZAHRDIAN_IDOL"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["FISH_OUT_OF_WATER"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["DEFEAT_EMPRESS_OF_LIGHT"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["OBSESSIVE_DEVOTION"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["STAR_DESTROYER"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["CHAMPION_OF_TERRARIA"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["GOBLIN_PUNTER"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["DO_YOU_WANT_TO_SLAY_A_SNOWMAN"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["WALK_THE_PLANK"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["BALEFUL_HARVEST"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["ICE_SCREAM"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["TIN_FOIL_HATTER"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["DEFEAT_OLD_ONES_ARMY_TIER3"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["TIL_DEATH"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["THERE_ARE_SOME_WHO_CALL_HIM"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["ARCHAEOLOGIST"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["PRETTY_IN_PINK"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["DECEIVER_OF_FOOLS"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["VEHICULAR_MANSLAUGHTER"] = Terraria.Achievements.AchievementCategory.Slayer;
        categories["SMASHING_POPPET"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["BEGONE_EVIL"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["FOUND_GRAVEYARD"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["ITS_HARD"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["FUNKYTOWN"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["WATCH_YOUR_STEP"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["YOU_CAN_DO_IT"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["BLOODBATH"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["KILL_THE_SUN"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["STICKY_SITUATION"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["NO_HOBO"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["IT_CAN_TALK"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["HEART_BREAKER"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["I_AM_LOOT"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["ROBBING_THE_GRAVE"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["GET_A_LIFE"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["FIND_A_FAIRY"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["TRANSMUTE_ITEM"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["JEEPERS_CREEPERS"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["WHERES_MY_HONEY"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["DUNGEON_HEIST"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["BIG_BOOTY"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["ITS_GETTING_HOT_IN_HERE"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["INTO_ORBIT"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["ROCK_BOTTOM"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["OOO_SHINY"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["EXTRA_SHINY"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["PHOTOSYNTHESIS"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["PLAY_ON_A_SPECIAL_SEED"] = Terraria.Achievements.AchievementCategory.Explorer;
        categories["GELATIN_WORLD_TOUR"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["SLAYER_OF_WORLDS"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["REAL_ESTATE_AGENT"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["ALL_TOWN_SLIMES"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["YOU_AND_WHAT_ARMY"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["TOPPED_OFF"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["MECHA_MAYHEM"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["BULLDOZER"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["PURIFY_ENTIRE_WORLD"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["NOT_THE_BEES"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["FLY_A_KITE_ON_A_WINDY_DAY"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["DIE_TO_DEAD_MANS_CHEST"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["GO_LAVA_FISHING"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["RAINBOWS_AND_UNICORNS"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["THROWING_LINES"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["TURN_GNOME_TO_STATUE"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["TALK_TO_NPC_AT_MAX_HAPPINESS"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["FREQUENT_FLYER"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["LUCKY_BREAK"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["MARATHON_MEDALIST"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["PET_THE_PET"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["THROW_A_PARTY"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["DRINK_BOTTLED_WATER_WHILE_DROWNING"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["TO_INFINITY_AND_BEYOND"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["SERVANT_IN_TRAINING"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["GOOD_LITTLE_SLAVE"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["TROUT_MONKEY"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["FAST_AND_FISHIOUS"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["SUPREME_HELPER_MINION"] = Terraria.Achievements.AchievementCategory.Challenger;
        categories["OBTAIN_HAMMER"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["BENCHED"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["HEAVY_METAL"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["STAR_POWER"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["GET_GOLDEN_DELIGHT"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["MINER_FOR_FIRE"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["HEAD_IN_THE_CLOUDS"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["GET_TERRASPARK_BOOTS"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["GET_CELL_PHONE"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["GET_ANKH_SHIELD"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["DRAX_ATTAX"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["PRISMANCER"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["SWORD_OF_THE_HERO"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["GET_ZENITH"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["HOLD_ON_TIGHT"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["THE_CAVALRY"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["DYE_HARD"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["MATCHING_ATTIRE"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["FASHION_STATEMENT"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["COMPLETELY_AWESOME"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["TIMBER"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["SICK_THROW"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["GLORIOUS_GOLDEN_POLE"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["TEMPLE_RAIDER"] = Terraria.Achievements.AchievementCategory.Collector;
        categories["LIKE_A_BOSS"] = Terraria.Achievements.AchievementCategory.Collector;

        foreach (var achievement in icon_indices.Keys)
        {
            Mod.AddContent(new VanillaAchievement(achievement));
        }

        num = 0;
        orders["TIMBER"] = num++;
        orders["BENCHED"] = num++;
        orders["OBTAIN_HAMMER"] = num++;
        orders["NO_HOBO"] = num++;
        orders["YOU_CAN_DO_IT"] = num++;
        orders["OOO_SHINY"] = num++;
        orders["HEAVY_METAL"] = num++;
        orders["MATCHING_ATTIRE"] = num++;
        orders["HEART_BREAKER"] = num++;
        orders["I_AM_LOOT"] = num++;
        orders["HOLD_ON_TIGHT"] = num++;
        orders["STAR_POWER"] = num++;
        orders["EYE_ON_YOU"] = num++;
        orders["SMASHING_POPPET"] = num++;
        orders["WHERES_MY_HONEY"] = num++;
        orders["STING_OPERATION"] = num++;
        orders["BONED"] = num++;
        orders["DUNGEON_HEIST"] = num++;
        orders["ITS_GETTING_HOT_IN_HERE"] = num++;
        orders["MINER_FOR_FIRE"] = num++;
        orders["STILL_HUNGRY"] = num++;
        orders["ITS_HARD"] = num++;
        orders["BEGONE_EVIL"] = num++;
        orders["EXTRA_SHINY"] = num++;
        orders["HEAD_IN_THE_CLOUDS"] = num++;
        orders["BUCKETS_OF_BOLTS"] = num++;
        orders["DRAX_ATTAX"] = num++;
        orders["PHOTOSYNTHESIS"] = num++;
        orders["GET_A_LIFE"] = num++;
        orders["THE_GREAT_SOUTHERN_PLANTKILL"] = num++;
        orders["TEMPLE_RAIDER"] = num++;
        orders["LIHZAHRDIAN_IDOL"] = num++;
        orders["ROBBING_THE_GRAVE"] = num++;
        orders["OBSESSIVE_DEVOTION"] = num++;
        orders["STAR_DESTROYER"] = num++;
        orders["CHAMPION_OF_TERRARIA"] = num++;
        _ = num;

        /*
         * 			Achievement achievement = new Achievement("TIMBER");
            achievement.AddCondition(ItemPickupCondition.Create(9, 619, 2504, 620, 2503, 2260, 621, 911, 1729, 5215));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("BENCHED");
            achievement.AddCondition(ItemCraftCondition.Create(ItemID.Sets.Workbenches));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("NO_HOBO");
            achievement.AddCondition(ProgressionEventCondition.Create(8));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("OBTAIN_HAMMER");
            achievement.AddCondition(ItemPickupCondition.Create(2775, 2746, 5283, 3505, 654, 3517, 7, 3493, 2780, 1513, 2516, 660, 3481, 657, 922, 3511, 2785, 3499, 3487, 196, 367, 104, 797, 2320, 787, 1234, 1262, 3465, 204, 217, 1507, 3524, 3522, 3525, 3523, 4317, 1305));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("OOO_SHINY");
            achievement.AddCondition(TileDestroyedCondition.Create(7, 6, 9, 8, 166, 167, 168, 169, 22, 204, 58, 107, 108, 111, 221, 222, 223, 211));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("HEART_BREAKER");
            achievement.AddCondition(TileDestroyedCondition.Create(12));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("HEAVY_METAL");
            achievement.AddCondition(ItemPickupCondition.Create(35, 716));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("I_AM_LOOT");
            achievement.AddCondition(CustomFlagCondition.Create("Peek"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("STAR_POWER");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("HOLD_ON_TIGHT");
            achievement.AddCondition(CustomFlagCondition.Create("Equip"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("EYE_ON_YOU");
            achievement.AddCondition(NPCKilledCondition.Create(4));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("SMASHING_POPPET");
            achievement.AddCondition(ProgressionEventCondition.Create(7));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("WORM_FODDER");
            achievement.AddCondition(NPCKilledCondition.Create(13, 14, 15));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("MASTERMIND");
            achievement.AddCondition(NPCKilledCondition.Create(266));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("WHERES_MY_HONEY");
            achievement.AddCondition(CustomFlagCondition.Create("Reach"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("STING_OPERATION");
            achievement.AddCondition(NPCKilledCondition.Create(222));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("BONED");
            achievement.AddCondition(NPCKilledCondition.Create(35));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DUNGEON_HEIST");
            achievement.AddCondition(ItemPickupCondition.Create(327));
            achievement.AddCondition(ProgressionEventCondition.Create(19));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("ITS_GETTING_HOT_IN_HERE");
            achievement.AddCondition(CustomFlagCondition.Create("Reach"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("MINER_FOR_FIRE");
            achievement.AddCondition(ItemCraftCondition.Create(122));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("STILL_HUNGRY");
            achievement.AddCondition(NPCKilledCondition.Create(113, 114));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("ITS_HARD");
            achievement.AddCondition(ProgressionEventCondition.Create(9));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("BEGONE_EVIL");
            achievement.AddCondition(ProgressionEventCondition.Create(6));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("EXTRA_SHINY");
            achievement.AddCondition(TileDestroyedCondition.Create(107, 108, 111, 221, 222, 223));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("HEAD_IN_THE_CLOUDS");
            achievement.AddCondition(CustomFlagCondition.Create("Equip"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("LIKE_A_BOSS");
            achievement.AddCondition(ItemPickupCondition.Create(1133, 1331, 1307, 267, 1293, 5334, 557, 544, 556, 560, 43, 70, 3601, 5120, 4961, 4988, 2673));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("BUCKETS_OF_BOLTS");
            achievement.AddCondition(NPCKilledCondition.Create(125, 126));
            achievement.AddConditions(NPCKilledCondition.CreateMany(127, 134));
            achievement.UseConditionsCompletedTracker();
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DRAX_ATTAX");
            achievement.AddCondition(ItemCraftCondition.Create(579, 990));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("PHOTOSYNTHESIS");
            achievement.AddCondition(TileDestroyedCondition.Create(211));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GET_A_LIFE");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("THE_GREAT_SOUTHERN_PLANTKILL");
            achievement.AddCondition(NPCKilledCondition.Create(262));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TEMPLE_RAIDER");
            achievement.AddCondition(ProgressionEventCondition.Create(22));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("LIHZAHRDIAN_IDOL");
            achievement.AddCondition(NPCKilledCondition.Create(245));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("ROBBING_THE_GRAVE");
            achievement.AddCondition(ItemPickupCondition.Create(1513, 938, 963, 977, 1300, 1254, 1514, 679, 759, 1446, 1445, 1444, 1183, 1266, 671, 3291, 4679));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("BIG_BOOTY");
            achievement.AddCondition(ProgressionEventCondition.Create(20));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("FISH_OUT_OF_WATER");
            achievement.AddCondition(NPCKilledCondition.Create(370));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("OBSESSIVE_DEVOTION");
            achievement.AddCondition(NPCKilledCondition.Create(439));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("STAR_DESTROYER");
            achievement.AddConditions(NPCKilledCondition.CreateMany(517, 422, 507, 493));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("CHAMPION_OF_TERRARIA");
            achievement.AddCondition(NPCKilledCondition.Create(398));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("BLOODBATH");
            achievement.AddCondition(ProgressionEventCondition.Create(5));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("SLIPPERY_SHINOBI");
            achievement.AddCondition(NPCKilledCondition.Create(50));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GOBLIN_PUNTER");
            achievement.AddCondition(ProgressionEventCondition.Create(10));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("WALK_THE_PLANK");
            achievement.AddCondition(ProgressionEventCondition.Create(11));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("KILL_THE_SUN");
            achievement.AddCondition(ProgressionEventCondition.Create(3));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DO_YOU_WANT_TO_SLAY_A_SNOWMAN");
            achievement.AddCondition(ProgressionEventCondition.Create(12));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TIN_FOIL_HATTER");
            achievement.AddCondition(ProgressionEventCondition.Create(13));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("BALEFUL_HARVEST");
            achievement.AddCondition(ProgressionEventCondition.Create(15));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("ICE_SCREAM");
            achievement.AddCondition(ProgressionEventCondition.Create(14));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("STICKY_SITUATION");
            achievement.AddCondition(ProgressionEventCondition.Create(16));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("REAL_ESTATE_AGENT");
            achievement.AddCondition(ProgressionEventCondition.Create(17));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("NOT_THE_BEES");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("JEEPERS_CREEPERS");
            achievement.AddCondition(CustomFlagCondition.Create("Reach"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("FUNKYTOWN");
            achievement.AddCondition(CustomFlagCondition.Create("Reach"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("INTO_ORBIT");
            achievement.AddCondition(CustomFlagCondition.Create("Reach"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("ROCK_BOTTOM");
            achievement.AddCondition(CustomFlagCondition.Create("Reach"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("MECHA_MAYHEM");
            achievement.AddCondition(ProgressionEventCondition.Create(21));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GELATIN_WORLD_TOUR");
            achievement.AddConditions(NPCKilledCondition.CreateMany(-5, -6, 1, 81, 71, -3, 147, 138, -10, 50, 59, 16, -7, 244, -8, -1, -2, 184, 204, 225, -9, 141, 183, -4));
            achievement.UseConditionsCompletedTracker();
            Main.Achievements.Register(achievement);
            achievement = new Achievement("FASHION_STATEMENT");
            achievement.AddCondition(CustomFlagCondition.Create("Equip"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("VEHICULAR_MANSLAUGHTER");
            achievement.AddCondition(CustomFlagCondition.Create("Hit"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("BULLDOZER");
            achievement.AddCondition(CustomIntCondition.Create("Pick", 10000));
            achievement.UseTrackerFromCondition("Pick");
            Main.Achievements.Register(achievement);
            achievement = new Achievement("THERE_ARE_SOME_WHO_CALL_HIM");
            achievement.AddCondition(NPCKilledCondition.Create(45));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DECEIVER_OF_FOOLS");
            achievement.AddCondition(NPCKilledCondition.Create(196));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("SWORD_OF_THE_HERO");
            achievement.AddCondition(ItemPickupCondition.Create(757));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("LUCKY_BREAK");
            achievement.AddCondition(CustomFlagCondition.Create("Hit"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("THROWING_LINES");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DYE_HARD");
            achievement.AddCondition(CustomFlagCondition.Create("Equip"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("SICK_THROW");
            achievement.AddCondition(ItemPickupCondition.Create(3389));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("FREQUENT_FLYER");
            achievement.AddCondition(CustomFloatCondition.Create("Pay", 10000f));
            achievement.UseTrackerFromCondition("Pay");
            Main.Achievements.Register(achievement);
            achievement = new Achievement("THE_CAVALRY");
            achievement.AddCondition(CustomFlagCondition.Create("Equip"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("COMPLETELY_AWESOME");
            achievement.AddCondition(ItemPickupCondition.Create(98));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TIL_DEATH");
            achievement.AddCondition(NPCKilledCondition.Create(53));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("ARCHAEOLOGIST");
            achievement.AddCondition(NPCKilledCondition.Create(52));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("PRETTY_IN_PINK");
            achievement.AddCondition(NPCKilledCondition.Create(-4));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("RAINBOWS_AND_UNICORNS");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("YOU_AND_WHAT_ARMY");
            achievement.AddCondition(CustomFlagCondition.Create("Spawn"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("PRISMANCER");
            achievement.AddCondition(ItemPickupCondition.Create(495));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("IT_CAN_TALK");
            achievement.AddCondition(ProgressionEventCondition.Create(18));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("WATCH_YOUR_STEP");
            achievement.AddCondition(CustomFlagCondition.Create("Hit"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("MARATHON_MEDALIST");
            achievement.AddCondition(CustomFloatCondition.Create("Move", 1106688f));
            achievement.UseTrackerFromCondition("Move");
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GLORIOUS_GOLDEN_POLE");
            achievement.AddCondition(ItemPickupCondition.Create(2294));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("SERVANT_IN_TRAINING");
            achievement.AddCondition(CustomFlagCondition.Create("Finish"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GOOD_LITTLE_SLAVE");
            achievement.AddCondition(CustomIntCondition.Create("Finish", 10));
            achievement.UseTrackerFromCondition("Finish");
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TROUT_MONKEY");
            achievement.AddCondition(CustomIntCondition.Create("Finish", 25));
            achievement.UseTrackerFromCondition("Finish");
            Main.Achievements.Register(achievement);
            achievement = new Achievement("FAST_AND_FISHIOUS");
            achievement.AddCondition(CustomIntCondition.Create("Finish", 50));
            achievement.UseTrackerFromCondition("Finish");
            Main.Achievements.Register(achievement);
            achievement = new Achievement("SUPREME_HELPER_MINION");
            achievement.AddCondition(CustomIntCondition.Create("Finish", 200));
            achievement.UseTrackerFromCondition("Finish");
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TOPPED_OFF");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("SLAYER_OF_WORLDS");
            achievement.AddCondition(NPCKilledCondition.Create(13, 14, 15));
            achievement.AddCondition(NPCKilledCondition.Create(113, 114));
            achievement.AddCondition(NPCKilledCondition.Create(125, 126));
            achievement.AddConditions(NPCKilledCondition.CreateMany(4, 266, 35, 50, 222, 134, 127, 262, 245, 439, 398, 370));
            achievement.UseConditionsCompletedTracker();
            Main.Achievements.Register(achievement);
            achievement = new Achievement("YOU_CAN_DO_IT");
            achievement.AddCondition(ProgressionEventCondition.Create(1));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("MATCHING_ATTIRE");
            achievement.AddCondition(CustomFlagCondition.Create("Equip"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DEFEAT_EMPRESS_OF_LIGHT");
            achievement.AddCondition(NPCKilledCondition.Create(636));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DEFEAT_QUEEN_SLIME");
            achievement.AddCondition(NPCKilledCondition.Create(657));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DEFEAT_DREADNAUTILUS");
            achievement.AddCondition(NPCKilledCondition.Create(618));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DEFEAT_OLD_ONES_ARMY_TIER3");
            achievement.AddCondition(ProgressionEventCondition.Create(23));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GET_ZENITH");
            achievement.AddCondition(ItemPickupCondition.Create(4956));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GET_TERRASPARK_BOOTS");
            achievement.AddCondition(ItemPickupCondition.Create(5000));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("FLY_A_KITE_ON_A_WINDY_DAY");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("FOUND_GRAVEYARD");
            achievement.AddCondition(CustomFlagCondition.Create("Reach"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GO_LAVA_FISHING");
            achievement.AddCondition(CustomFlagCondition.Create("Do"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TURN_GNOME_TO_STATUE");
            achievement.AddCondition(ProgressionEventCondition.Create(24));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TALK_TO_NPC_AT_MAX_HAPPINESS");
            achievement.AddCondition(CustomFlagCondition.Create("Do"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("PET_THE_PET");
            achievement.AddCondition(CustomFlagCondition.Create("Do"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("FIND_A_FAIRY");
            achievement.AddCondition(CustomFlagCondition.Create("Do"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("THROW_A_PARTY");
            achievement.AddCondition(ProgressionEventCondition.Create(25));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DIE_TO_DEAD_MANS_CHEST");
            achievement.AddCondition(CustomFlagCondition.Create("Do"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DEFEAT_DEERCLOPS");
            achievement.AddCondition(NPCKilledCondition.Create(668));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GET_GOLDEN_DELIGHT");
            achievement.AddCondition(ItemPickupCondition.Create(4022));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("DRINK_BOTTLED_WATER_WHILE_DROWNING");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GET_CELL_PHONE");
            achievement.AddCondition(ItemPickupCondition.Create(3124));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GET_ANKH_SHIELD");
            achievement.AddCondition(ItemPickupCondition.Create(1613));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("GAIN_TORCH_GODS_FAVOR");
            achievement.AddCondition(CustomFlagCondition.Create("Use"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("PLAY_ON_A_SPECIAL_SEED");
            achievement.AddCondition(CustomFlagCondition.Create("Do"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("ALL_TOWN_SLIMES");
            achievement.AddCondition(ProgressionEventCondition.Create(26));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TRANSMUTE_ITEM");
            achievement.AddCondition(ProgressionEventCondition.Create(27));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("PURIFY_ENTIRE_WORLD");
            achievement.AddCondition(CustomFlagCondition.Create("Do"));
            Main.Achievements.Register(achievement);
            achievement = new Achievement("TO_INFINITY_AND_BEYOND");
            achievement.AddCondition(CustomFlagCondition.Create("Do"));
            Main.Achievements.Register(achievement);
         */
    }
}