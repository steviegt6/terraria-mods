namespace Nightshade.Content;

internal static class PlatCritterHelpers
{
    public static string GetGoldName(string name)
    {
        return name switch
        {
            "Squirrel" => "SquirrelGold",
            "SquirrelCage" => "SquirrelGoldCage",
            "LadyBugCage" => "GoldLadybugCage",
            // Somehow, this one is named "correctly" already?!
            // "ButterflyCage" => "GoldButterflyJar",
            "DragonflyCage" => "GoldDragonflyJar",
            "GoldfishCage" => "GoldGoldfishBowl",
            _ => "Gold" + name,
        };
    }
}