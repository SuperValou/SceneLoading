namespace Packages.SceneLoading.Editor.SceneInfoGenerations
{
    public static class SceneInfoGeneration
    {
        public static void Execute()
        {
            var generator = new SceneInfoGenerator();
            generator.Initialize();
            generator.Generate();
        }
    }
}