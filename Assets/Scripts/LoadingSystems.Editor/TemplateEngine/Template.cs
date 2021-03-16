using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class Template
    {
        public Template GetSubtemplate(string subtemplateName)
        {
            return new Template(mainBoolStub:false);
        }

        public ISession CreateSession()
        {
            if (_mainBoolStub)
            {
                return this.CreateMainSessionStub();
            }

            return this.CreateSubSessionStub();
        }

        // -- STUBS

        private readonly bool _mainBoolStub;

        public Template(bool mainBoolStub)
        {
            _mainBoolStub = mainBoolStub;
        }

        private ISession CreateMainSessionStub()
        {
            Session session = new Session();
            ISessionChunkBuilding sessionBuilding = session;

            sessionBuilding.AppendTextChunk("namespace ");
            sessionBuilding.AppendVariableChunk("namespace");
            sessionBuilding.AppendTextChunk(@"
{
    public enum SceneId
    {
		");

            sessionBuilding.AppendSubsessionChunk("enumMember");
            sessionBuilding.AppendTextChunk(@"
    }
}");

            return session;
        }

        private ISession CreateSubSessionStub()
        {
            Session session = new Session();
            ISessionChunkBuilding sessionBuilding = session;

            sessionBuilding.AppendVariableChunk("sceneEnumMemberName");
            //sessionBuilding.AppendTextChunk(" = ");
            //sessionBuilding.AppendVariableChunk("sceneEnumMemberValue");
            sessionBuilding.AppendTextChunk(@",
		");

            return session;
        }
    }
}