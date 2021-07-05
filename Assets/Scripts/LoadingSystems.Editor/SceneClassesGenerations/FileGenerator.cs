using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations
{
    public abstract class FileGenerator
    {
        protected abstract string TemplateFile { get; }
        protected abstract string OutputFile { get; }

        public void GenerateFile()
        {
            // Get templates
            string templatePath = AssetDatabaseExt.GetAssetFilePath(TemplateFile);
            var parser = new TemplateParser(templatePath);

            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();

            // Build session
            ISession session = BuildSession(template);

            // Write session to file
            string destinationFilePath = AssetDatabaseExt.GetAssetFilePath(OutputFile);
            Debug.Log($"About to rewrite file at '{destinationFilePath}'...");
            SessionWriter writer = new SessionWriter();
            writer.WriteSession(session, destinationFilePath);
        }

        protected abstract ISession BuildSession(ITemplate template);
    }
}