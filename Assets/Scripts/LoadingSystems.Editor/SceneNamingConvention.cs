using System.Text.RegularExpressions;

namespace Assets.Scripts.LoadingSystems.Editor
{
    public static class SceneNamingConvention
    {
        public static Regex IgnoredSceneRegex = new Regex(@"^Test-\w+$"); // Example of match: "Test-abilities", "Test-Boss"

        public static Regex GameplaySceneRegex = new Regex(@"^[\w-]*[Gg]ameplay$"); // Example of match: "0-Gameplay", "main_gameplay"
        public static Regex MasterSceneRegex = new Regex(@"^Master\w+$"); // Example of match: "MasterIceWorld"
        public static Regex RoomSceneRegex = new Regex(@"^[\w-]+$"); // Example of match: "Elevator-to-lava-world",
    }
}