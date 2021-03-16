namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    internal interface ISessionChunkBuilding
    {
        void AppendTextChunk(string text);
        void AppendVariableChunk(string variableName);
        void AppendSubsessionChunk(string subtemplateName);
    }
}