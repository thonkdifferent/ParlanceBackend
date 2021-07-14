using System;
using System.IO;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Misc;
using ParlanceBackend.Models;
using ParlanceBackend.TranslationFiles;

namespace ParlanceBackend.Services
{
    public class TranslationFileService
    {

        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly GitService _git;
        public TranslationFileService(IOptions<ParlanceConfiguration> parlanceConfiguration, GitService git)
        {
            _parlanceConfiguration = parlanceConfiguration;
            _git = git;
        }
        
        private ProjectSpecification.Subproject FindSubproject(ProjectPrivate project, string subproject)
        {
            var publicProj = project.ToPublicProject(_parlanceConfiguration);

            var spec = publicProj.specFile();

            var subprojectObj = spec.Subprojects.Find(sp => sp.Slug == subproject);
            if (subprojectObj == null) return null;
            
            subprojectObj.SetConfiguration(_parlanceConfiguration);

            return subprojectObj;
        }

        private string TranslationFileFilename(ProjectSpecification.Subproject subproject,
            string language)
        {
            return $"{_git.GetDirectoryFromSlug(Utility.Slugify(subproject.parentProjName))}/{subproject.Path.Replace("{lang}", language)}";
        }

        public void UpdateTranslationFile(TranslationDelta delta, ProjectPrivate project, string subproject, string language)
        {
            var subprojectObj = FindSubproject(project, subproject);
            var translationFileName = TranslationFileFilename(subprojectObj, language);

            switch (subprojectObj.Type)
            {
                case "qt":
                    QtTranslationFile.Update(translationFileName, delta);
                    break;
                default:
                    throw new Exception("Unknown File Type");
            }
        }

        public TranslationFile TranslationFile(ProjectPrivate project, string subproject, string language)
        {
            var subprojectObj = FindSubproject(project, Utility.Slugify(subproject));
            var translationFileName = TranslationFileFilename(subprojectObj, language);
            
            //TODO: maybe throw an error instead?
            if (!File.Exists(translationFileName)) return new TranslationFile();

            return subprojectObj.Type switch
            {
                "qt" => QtTranslationFile.LoadFromFile(translationFileName),
                "gettext" => GettextTranslationFile.LoadFromFile(translationFileName),
                "webext-json" => WebExtensionsJsonTranslationFile.LoadFromFile(translationFileName),
                _ => throw new Exception("Unknown File Type")
            };
        }
    }
}