using System.Collections.Generic;

using Terraria.ModLoader;

namespace Daybreak.Common.Features.Achievements;

// TODO: [DependsOn<AchievementImpl>]
public sealed class VanillaAchievements : ModSystem
{
    private abstract class VanillaCategory : AchievementCategory
    {
        public override string Name { get; }

        protected VanillaCategory(string name)
        {
            Name = "Terraria/" + name;
            VANILLA_CATEGORIES_BY_NAME[name] = this;
        }
    }

    private sealed class CategorySlayer() : VanillaCategory("Slayer");

    private sealed class CategoryCollector() : VanillaCategory("Collector");

    private sealed class CategoryExplorer() : VanillaCategory("Explorer");

    private sealed class CategoryChallenger() : VanillaCategory("Challenger");

    private abstract class VanillaAchievement : Achievement
    {
        public override string Name { get; }

        protected VanillaAchievement(string name)
        {
            Name = "Terraria/" + name;
            VANILLA_ACHIEVEMENTS_BY_NAME[name] = this;
        }
    }


    private sealed class AchievementTimber() : VanillaAchievement("TIMBER");

    private sealed class AchievementNoHobo() : VanillaAchievement("NO_HOBO");

    private sealed class AchievementObtainHammer() : VanillaAchievement("OBTAIN_HAMMER");

    private sealed class AchievementHeartBreaker() : VanillaAchievement("HEART_BREAKER");

    private sealed class AchievementOooShiny() : VanillaAchievement("OOO_SHINY");

    private sealed class AchievementHeavyMetal() : VanillaAchievement("HEAVY_METAL");

    private sealed class AchievementAmLoot() : VanillaAchievement("I_AM_LOOT");

    private sealed class AchievementStarPower() : VanillaAchievement("STAR_POWER");

    private sealed class AchievementHoldOnTight() : VanillaAchievement("HOLD_ON_TIGHT");

    private sealed class AchievementEyeOnYou() : VanillaAchievement("EYE_ON_YOU");

    private sealed class AchievementSmashingPoppet() : VanillaAchievement("SMASHING_POPPET");

    private sealed class AchievementWormFodder() : VanillaAchievement("WORM_FODDER");

    private sealed class AchievementMastermind() : VanillaAchievement("MASTERMIND");

    private sealed class AchievementWheresMyHoney() : VanillaAchievement("WHERES_MY_HONEY");

    private sealed class AchievementStingOperation() : VanillaAchievement("STING_OPERATION");

    private sealed class AchievementBoned() : VanillaAchievement("BONED");

    private sealed class AchievementDungeonHeist() : VanillaAchievement("DUNGEON_HEIST");

    private sealed class AchievementItsGettingHotInHere() : VanillaAchievement("ITS_GETTING_HOT_IN_HERE");

    private sealed class AchievementMinerForFire() : VanillaAchievement("MINER_FOR_FIRE");

    private sealed class AchievementStillHungry() : VanillaAchievement("STILL_HUNGRY");

    private sealed class AchievementItsHard() : VanillaAchievement("ITS_HARD");

    private sealed class AchievementBegoneEvil() : VanillaAchievement("BEGONE_EVIL");

    private sealed class AchievementExtraShiny() : VanillaAchievement("EXTRA_SHINY");

    private sealed class AchievementHeadInTheClouds() : VanillaAchievement("HEAD_IN_THE_CLOUDS");

    private sealed class AchievementLikeABoss() : VanillaAchievement("LIKE_A_BOSS");

    private sealed class AchievementBucketsOfBolts() : VanillaAchievement("BUCKETS_OF_BOLTS");

    private sealed class AchievementDraxAttax() : VanillaAchievement("DRAX_ATTAX");

    private sealed class AchievementPhotosynthesis() : VanillaAchievement("PHOTOSYNTHESIS");

    private sealed class AchievementGetALife() : VanillaAchievement("GET_A_LIFE");

    private sealed class AchievementTheGreatSouthernPlantkill() : VanillaAchievement("THE_GREAT_SOUTHERN_PLANTKILL");

    private sealed class AchievementTempleRaider() : VanillaAchievement("TEMPLE_RAIDER");

    private sealed class AchievementLihzahrdianIdol() : VanillaAchievement("LIHZAHRDIAN_IDOL");

    private sealed class AchievementRobbingTheGrave() : VanillaAchievement("ROBBING_THE_GRAVE");

    private sealed class AchievementBigBooty() : VanillaAchievement("BIG_BOOTY");

    private sealed class AchievementFishOutOfWater() : VanillaAchievement("FISH_OUT_OF_WATER");

    private sealed class AchievementObsessiveDevotion() : VanillaAchievement("OBSESSIVE_DEVOTION");

    private sealed class AchievementStarDestroyer() : VanillaAchievement("STAR_DESTROYER");

    private sealed class AchievementChampionOfTerraria() : VanillaAchievement("CHAMPION_OF_TERRARIA");

    private sealed class AchievementBloodbath() : VanillaAchievement("BLOODBATH");

    private sealed class AchievementGoblinPunter() : VanillaAchievement("GOBLIN_PUNTER");

    private sealed class AchievementKillTheSun() : VanillaAchievement("KILL_THE_SUN");

    private sealed class AchievementWalkThePlank() : VanillaAchievement("WALK_THE_PLANK");

    private sealed class AchievementDoYouWantToSlayASnowman() : VanillaAchievement("DO_YOU_WANT_TO_SLAY_A_SNOWMAN");

    private sealed class AchievementTinFoilHatter() : VanillaAchievement("TIN_FOIL_HATTER");

    private sealed class AchievementBalefulHarvest() : VanillaAchievement("BALEFUL_HARVEST");

    private sealed class AchievementIceScream() : VanillaAchievement("ICE_SCREAM");

    private sealed class AchievementSlipperyShinobi() : VanillaAchievement("SLIPPERY_SHINOBI");

    private sealed class AchievementStickySituation() : VanillaAchievement("STICKY_SITUATION");

    private sealed class AchievementRealEstateAgent() : VanillaAchievement("REAL_ESTATE_AGENT");

    private sealed class AchievementNotTheBees() : VanillaAchievement("NOT_THE_BEES");

    private sealed class AchievementJeepersCreepers() : VanillaAchievement("JEEPERS_CREEPERS");

    private sealed class AchievementFunkytown() : VanillaAchievement("FUNKYTOWN");

    private sealed class AchievementIntoOrbit() : VanillaAchievement("INTO_ORBIT");

    private sealed class AchievementRockBottom() : VanillaAchievement("ROCK_BOTTOM");

    private sealed class AchievementMechaMayhem() : VanillaAchievement("MECHA_MAYHEM");

    private sealed class AchievementGelatinWorldTour() : VanillaAchievement("GELATIN_WORLD_TOUR");

    private sealed class AchievementFashionStatement() : VanillaAchievement("FASHION_STATEMENT");

    private sealed class AchievementVehicularManslaughter() : VanillaAchievement("VEHICULAR_MANSLAUGHTER");

    private sealed class AchievementBulldozer() : VanillaAchievement("BULLDOZER");

    private sealed class AchievementThereAreSomeWhoCallHim() : VanillaAchievement("THERE_ARE_SOME_WHO_CALL_HIM");

    private sealed class AchievementDeceiverOfFools() : VanillaAchievement("DECEIVER_OF_FOOLS");

    private sealed class AchievementSwordOfTheHero() : VanillaAchievement("SWORD_OF_THE_HERO");

    private sealed class AchievementLuckyBreak() : VanillaAchievement("LUCKY_BREAK");

    private sealed class AchievementThrowingLines() : VanillaAchievement("THROWING_LINES");

    private sealed class AchievementDyeHard() : VanillaAchievement("DYE_HARD");

    private sealed class AchievementFrequentFlyer() : VanillaAchievement("FREQUENT_FLYER");

    private sealed class AchievementTheCavalry() : VanillaAchievement("THE_CAVALRY");

    private sealed class AchievementCompletelyAwesome() : VanillaAchievement("COMPLETELY_AWESOME");

    private sealed class AchievementTilDeath() : VanillaAchievement("TIL_DEATH");

    private sealed class AchievementArchaeologist() : VanillaAchievement("ARCHAEOLOGIST");

    private sealed class AchievementPrettyInPink() : VanillaAchievement("PRETTY_IN_PINK");

    private sealed class AchievementRainbowsAndUnicorns() : VanillaAchievement("RAINBOWS_AND_UNICORNS");

    private sealed class AchievementYouAndWhatArmy() : VanillaAchievement("YOU_AND_WHAT_ARMY");

    private sealed class AchievementPrismancer() : VanillaAchievement("PRISMANCER");

    private sealed class AchievementItCanTalk() : VanillaAchievement("IT_CAN_TALK");

    private sealed class AchievementWatchYourStep() : VanillaAchievement("WATCH_YOUR_STEP");

    private sealed class AchievementMarathonMedalist() : VanillaAchievement("MARATHON_MEDALIST");

    private sealed class AchievementGloriousGoldenPole() : VanillaAchievement("GLORIOUS_GOLDEN_POLE");

    private sealed class AchievementServantInTraining() : VanillaAchievement("SERVANT_IN_TRAINING");

    private sealed class AchievementGoodLittleSlave() : VanillaAchievement("GOOD_LITTLE_SLAVE");

    private sealed class AchievementTroutMonkey() : VanillaAchievement("TROUT_MONKEY");

    private sealed class AchievementFastAndFishious() : VanillaAchievement("FAST_AND_FISHIOUS");

    private sealed class AchievementSupremeHelperMinion() : VanillaAchievement("SUPREME_HELPER_MINION");

    private sealed class AchievementToppedOff() : VanillaAchievement("TOPPED_OFF");

    private sealed class AchievementSlayerOfWorlds() : VanillaAchievement("SLAYER_OF_WORLDS");

    private sealed class AchievementYouCanDoIt() : VanillaAchievement("YOU_CAN_DO_IT");

    private sealed class AchievementSickThrow() : VanillaAchievement("SICK_THROW");

    private sealed class AchievementMatchingAttire() : VanillaAchievement("MATCHING_ATTIRE");

    private sealed class AchievementBenched() : VanillaAchievement("BENCHED");

    private sealed class AchievementDefeatQueenSlime() : VanillaAchievement("DEFEAT_QUEEN_SLIME");

    private sealed class AchievementDefeatEmpressOfLight() : VanillaAchievement("DEFEAT_EMPRESS_OF_LIGHT");

    private sealed class AchievementGetZenith() : VanillaAchievement("GET_ZENITH");

    private sealed class AchievementFindAFairy() : VanillaAchievement("FIND_A_FAIRY");

    private sealed class AchievementDefeatDreadnautilus() : VanillaAchievement("DEFEAT_DREADNAUTILUS");

    private sealed class AchievementDefeatOldOnesArmyTier3() : VanillaAchievement("DEFEAT_OLD_ONES_ARMY_TIER3");

    private sealed class AchievementFlyAKiteOnAWindyDay() : VanillaAchievement("FLY_A_KITE_ON_A_WINDY_DAY");

    private sealed class AchievementTurnGnomeToStatue() : VanillaAchievement("TURN_GNOME_TO_STATUE");

    private sealed class AchievementTalkToNpcAtMaxHappiness() : VanillaAchievement("TALK_TO_NPC_AT_MAX_HAPPINESS");

    private sealed class AchievementGetTerrasparkBoots() : VanillaAchievement("GET_TERRASPARK_BOOTS");

    private sealed class AchievementThrowAParty() : VanillaAchievement("THROW_A_PARTY");

    private sealed class AchievementPetThePet() : VanillaAchievement("PET_THE_PET");

    private sealed class AchievementGoLavaFishing() : VanillaAchievement("GO_LAVA_FISHING");

    private sealed class AchievementFoundGraveyard() : VanillaAchievement("FOUND_GRAVEYARD");

    private sealed class AchievementDieToDeadMansChest() : VanillaAchievement("DIE_TO_DEAD_MANS_CHEST");

    private sealed class AchievementDefeatDeerclops() : VanillaAchievement("DEFEAT_DEERCLOPS");

    private sealed class AchievementGetGoldenDelight() : VanillaAchievement("GET_GOLDEN_DELIGHT");

    private sealed class AchievementDrinkBottledWaterWhileDrowning() : VanillaAchievement("DRINK_BOTTLED_WATER_WHILE_DROWNING");

    private sealed class AchievementGetCellPhone() : VanillaAchievement("GET_CELL_PHONE");

    private sealed class AchievementGetAnkhShield() : VanillaAchievement("GET_ANKH_SHIELD");

    private sealed class AchievementGainTorchGodsFavor() : VanillaAchievement("GAIN_TORCH_GODS_FAVOR");

    private sealed class AchievementPlayOnASpecialSeed() : VanillaAchievement("PLAY_ON_A_SPECIAL_SEED");

    private sealed class AchievementAllTownSlimes() : VanillaAchievement("ALL_TOWN_SLIMES");

    private sealed class AchievementTransmuteItem() : VanillaAchievement("TRANSMUTE_ITEM");

    private sealed class AchievementPurifyEntireWorld() : VanillaAchievement("PURIFY_ENTIRE_WORLD");

    private sealed class AchievementToInfinityAndBeyond() : VanillaAchievement("TO_INFINITY_AND_BEYOND");

    internal static readonly Dictionary<string, AchievementCategory> VANILLA_CATEGORIES_BY_NAME = [];
    internal static readonly Dictionary<string, Achievement> VANILLA_ACHIEVEMENTS_BY_NAME = [];

    private static readonly Dictionary<string, int> icon_indices = [];

    public override void Load()
    {
        base.Load();

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

        /*
         * 			AchievementCategory category = AchievementCategory.Slayer;
            Main.Achievements.RegisterAchievementCategory("EYE_ON_YOU", category);
            Main.Achievements.RegisterAchievementCategory("SLIPPERY_SHINOBI", category);
            Main.Achievements.RegisterAchievementCategory("WORM_FODDER", category);
            Main.Achievements.RegisterAchievementCategory("MASTERMIND", category);
            Main.Achievements.RegisterAchievementCategory("STING_OPERATION", category);
            Main.Achievements.RegisterAchievementCategory("DEFEAT_DEERCLOPS", category);
            Main.Achievements.RegisterAchievementCategory("BONED", category);
            Main.Achievements.RegisterAchievementCategory("STILL_HUNGRY", category);
            Main.Achievements.RegisterAchievementCategory("DEFEAT_DREADNAUTILUS", category);
            Main.Achievements.RegisterAchievementCategory("DEFEAT_QUEEN_SLIME", category);
            Main.Achievements.RegisterAchievementCategory("BUCKETS_OF_BOLTS", category);
            Main.Achievements.RegisterAchievementCategory("THE_GREAT_SOUTHERN_PLANTKILL", category);
            Main.Achievements.RegisterAchievementCategory("LIHZAHRDIAN_IDOL", category);
            Main.Achievements.RegisterAchievementCategory("FISH_OUT_OF_WATER", category);
            Main.Achievements.RegisterAchievementCategory("DEFEAT_EMPRESS_OF_LIGHT", category);
            Main.Achievements.RegisterAchievementCategory("OBSESSIVE_DEVOTION", category);
            Main.Achievements.RegisterAchievementCategory("STAR_DESTROYER", category);
            Main.Achievements.RegisterAchievementCategory("CHAMPION_OF_TERRARIA", category);
            Main.Achievements.RegisterAchievementCategory("GOBLIN_PUNTER", category);
            Main.Achievements.RegisterAchievementCategory("DO_YOU_WANT_TO_SLAY_A_SNOWMAN", category);
            Main.Achievements.RegisterAchievementCategory("WALK_THE_PLANK", category);
            Main.Achievements.RegisterAchievementCategory("BALEFUL_HARVEST", category);
            Main.Achievements.RegisterAchievementCategory("ICE_SCREAM", category);
            Main.Achievements.RegisterAchievementCategory("TIN_FOIL_HATTER", category);
            Main.Achievements.RegisterAchievementCategory("DEFEAT_OLD_ONES_ARMY_TIER3", category);
            Main.Achievements.RegisterAchievementCategory("TIL_DEATH", category);
            Main.Achievements.RegisterAchievementCategory("THERE_ARE_SOME_WHO_CALL_HIM", category);
            Main.Achievements.RegisterAchievementCategory("ARCHAEOLOGIST", category);
            Main.Achievements.RegisterAchievementCategory("PRETTY_IN_PINK", category);
            Main.Achievements.RegisterAchievementCategory("DECEIVER_OF_FOOLS", category);
            Main.Achievements.RegisterAchievementCategory("VEHICULAR_MANSLAUGHTER", category);
            category = AchievementCategory.Explorer;
            Main.Achievements.RegisterAchievementCategory("SMASHING_POPPET", category);
            Main.Achievements.RegisterAchievementCategory("BEGONE_EVIL", category);
            Main.Achievements.RegisterAchievementCategory("FOUND_GRAVEYARD", category);
            Main.Achievements.RegisterAchievementCategory("ITS_HARD", category);
            Main.Achievements.RegisterAchievementCategory("FUNKYTOWN", category);
            Main.Achievements.RegisterAchievementCategory("WATCH_YOUR_STEP", category);
            Main.Achievements.RegisterAchievementCategory("YOU_CAN_DO_IT", category);
            Main.Achievements.RegisterAchievementCategory("BLOODBATH", category);
            Main.Achievements.RegisterAchievementCategory("KILL_THE_SUN", category);
            Main.Achievements.RegisterAchievementCategory("STICKY_SITUATION", category);
            Main.Achievements.RegisterAchievementCategory("NO_HOBO", category);
            Main.Achievements.RegisterAchievementCategory("IT_CAN_TALK", category);
            Main.Achievements.RegisterAchievementCategory("HEART_BREAKER", category);
            Main.Achievements.RegisterAchievementCategory("I_AM_LOOT", category);
            Main.Achievements.RegisterAchievementCategory("ROBBING_THE_GRAVE", category);
            Main.Achievements.RegisterAchievementCategory("GET_A_LIFE", category);
            Main.Achievements.RegisterAchievementCategory("FIND_A_FAIRY", category);
            Main.Achievements.RegisterAchievementCategory("TRANSMUTE_ITEM", category);
            Main.Achievements.RegisterAchievementCategory("JEEPERS_CREEPERS", category);
            Main.Achievements.RegisterAchievementCategory("WHERES_MY_HONEY", category);
            Main.Achievements.RegisterAchievementCategory("DUNGEON_HEIST", category);
            Main.Achievements.RegisterAchievementCategory("BIG_BOOTY", category);
            Main.Achievements.RegisterAchievementCategory("ITS_GETTING_HOT_IN_HERE", category);
            Main.Achievements.RegisterAchievementCategory("INTO_ORBIT", category);
            Main.Achievements.RegisterAchievementCategory("ROCK_BOTTOM", category);
            Main.Achievements.RegisterAchievementCategory("OOO_SHINY", category);
            Main.Achievements.RegisterAchievementCategory("EXTRA_SHINY", category);
            Main.Achievements.RegisterAchievementCategory("PHOTOSYNTHESIS", category);
            Main.Achievements.RegisterAchievementCategory("PLAY_ON_A_SPECIAL_SEED", category);
            category = AchievementCategory.Challenger;
            Main.Achievements.RegisterAchievementCategory("GELATIN_WORLD_TOUR", category);
            Main.Achievements.RegisterAchievementCategory("SLAYER_OF_WORLDS", category);
            Main.Achievements.RegisterAchievementCategory("REAL_ESTATE_AGENT", category);
            Main.Achievements.RegisterAchievementCategory("ALL_TOWN_SLIMES", category);
            Main.Achievements.RegisterAchievementCategory("YOU_AND_WHAT_ARMY", category);
            Main.Achievements.RegisterAchievementCategory("TOPPED_OFF", category);
            Main.Achievements.RegisterAchievementCategory("MECHA_MAYHEM", category);
            Main.Achievements.RegisterAchievementCategory("BULLDOZER", category);
            Main.Achievements.RegisterAchievementCategory("PURIFY_ENTIRE_WORLD", category);
            Main.Achievements.RegisterAchievementCategory("NOT_THE_BEES", category);
            Main.Achievements.RegisterAchievementCategory("FLY_A_KITE_ON_A_WINDY_DAY", category);
            Main.Achievements.RegisterAchievementCategory("DIE_TO_DEAD_MANS_CHEST", category);
            Main.Achievements.RegisterAchievementCategory("GO_LAVA_FISHING", category);
            Main.Achievements.RegisterAchievementCategory("RAINBOWS_AND_UNICORNS", category);
            Main.Achievements.RegisterAchievementCategory("THROWING_LINES", category);
            Main.Achievements.RegisterAchievementCategory("TURN_GNOME_TO_STATUE", category);
            Main.Achievements.RegisterAchievementCategory("TALK_TO_NPC_AT_MAX_HAPPINESS", category);
            Main.Achievements.RegisterAchievementCategory("FREQUENT_FLYER", category);
            Main.Achievements.RegisterAchievementCategory("LUCKY_BREAK", category);
            Main.Achievements.RegisterAchievementCategory("MARATHON_MEDALIST", category);
            Main.Achievements.RegisterAchievementCategory("PET_THE_PET", category);
            Main.Achievements.RegisterAchievementCategory("THROW_A_PARTY", category);
            Main.Achievements.RegisterAchievementCategory("DRINK_BOTTLED_WATER_WHILE_DROWNING", category);
            Main.Achievements.RegisterAchievementCategory("TO_INFINITY_AND_BEYOND", category);
            Main.Achievements.RegisterAchievementCategory("SERVANT_IN_TRAINING", category);
            Main.Achievements.RegisterAchievementCategory("GOOD_LITTLE_SLAVE", category);
            Main.Achievements.RegisterAchievementCategory("TROUT_MONKEY", category);
            Main.Achievements.RegisterAchievementCategory("FAST_AND_FISHIOUS", category);
            Main.Achievements.RegisterAchievementCategory("SUPREME_HELPER_MINION", category);
            category = AchievementCategory.Collector;
            Main.Achievements.RegisterAchievementCategory("OBTAIN_HAMMER", category);
            Main.Achievements.RegisterAchievementCategory("BENCHED", category);
            Main.Achievements.RegisterAchievementCategory("HEAVY_METAL", category);
            Main.Achievements.RegisterAchievementCategory("STAR_POWER", category);
            Main.Achievements.RegisterAchievementCategory("GET_GOLDEN_DELIGHT", category);
            Main.Achievements.RegisterAchievementCategory("MINER_FOR_FIRE", category);
            Main.Achievements.RegisterAchievementCategory("HEAD_IN_THE_CLOUDS", category);
            Main.Achievements.RegisterAchievementCategory("GET_TERRASPARK_BOOTS", category);
            Main.Achievements.RegisterAchievementCategory("GET_CELL_PHONE", category);
            Main.Achievements.RegisterAchievementCategory("GET_ANKH_SHIELD", category);
            Main.Achievements.RegisterAchievementCategory("DRAX_ATTAX", category);
            Main.Achievements.RegisterAchievementCategory("PRISMANCER", category);
            Main.Achievements.RegisterAchievementCategory("SWORD_OF_THE_HERO", category);
            Main.Achievements.RegisterAchievementCategory("GET_ZENITH", category);
            Main.Achievements.RegisterAchievementCategory("HOLD_ON_TIGHT", category);
            Main.Achievements.RegisterAchievementCategory("THE_CAVALRY", category);
            Main.Achievements.RegisterAchievementCategory("DYE_HARD", category);
            Main.Achievements.RegisterAchievementCategory("MATCHING_ATTIRE", category);
            Main.Achievements.RegisterAchievementCategory("FASHION_STATEMENT", category);
            Main.Achievements.RegisterAchievementCategory("COMPLETELY_AWESOME", category);
            Main.Achievements.RegisterAchievementCategory("TIMBER", category);
            Main.Achievements.RegisterAchievementCategory("SICK_THROW", category);
            Main.Achievements.RegisterAchievementCategory("GLORIOUS_GOLDEN_POLE", category);
            Main.Achievements.RegisterAchievementCategory("TEMPLE_RAIDER", category);
            Main.Achievements.RegisterAchievementCategory("LIKE_A_BOSS", category);
         */
    }
}