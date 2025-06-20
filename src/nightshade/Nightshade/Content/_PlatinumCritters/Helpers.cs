namespace Nightshade.Content;

internal static class PlatCritterHelpers
{
    public static string GetGoldName(string name)
    {
        return name == "Squirrel" ? name + "Gold" : "Gold" + name;
    }
}