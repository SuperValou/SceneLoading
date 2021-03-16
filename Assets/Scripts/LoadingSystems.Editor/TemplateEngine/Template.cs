namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class Template
    {
        public ISession CreateSession()
        {
            Session session = new Session();
            ISessionChunkBuilding sessionBuilding = session;
            
            sessionBuilding.AppendTextChunk("namespace ");
            sessionBuilding.AppendVariableChunk("namespace");
            sessionBuilding.AppendTextChunk(@"
{
    public enum SceneId
    {
		
    }
}");
            return session;
        }
    }
}