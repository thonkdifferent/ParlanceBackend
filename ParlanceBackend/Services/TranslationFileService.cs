using System;
using System.IO;
using Microsoft.Extensions.Options;
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

        /// <summary>
        /// Finds and returns a specific subproject based on its slug
        /// </summary>
        /// <param name="project">The project in question</param>
        /// <param name="subproject">Subproject slug</param>
        /// <returns></returns>
#nullable enable
        private ProjectSpecification.Subproject? FindSubproject(ProjectPrivate project, string subproject)
#nullable disable
        {
            var publicProj = project.ToPublicProject(_parlanceConfiguration); // fill the internal fields

            var spec = publicProj.SpecFile();//get the specfile

            //check if the slug is mentions
            var subprojectObj = spec.Subprojects.Find(sp => sp.Slug == subproject);
            if (subprojectObj == null) return null;
            
            //set the configuration
            subprojectObj.SetConfiguration(_parlanceConfiguration);

            return subprojectObj;
        }

        /// <summary>
        /// Get the parsed language file path
        /// </summary>
        /// <param name="subproject">Subproject slug</param>
        /// <param name="language">Language in question</param>
        /// <returns>Parsed file path</returns>
        private string TranslationFileFilename(ProjectSpecification.Subproject subproject,
            string language)
        {
            return Path.Join(_git.GetDirectoryFromSlug(Utility.Slugify(subproject.parentProjName)),
                subproject.Path.Replace("{lang}", language));
        }

        /// <summary>
        /// Update the translation file with a specified delta
        /// </summary>
        /// <param name="delta">New data</param>
        /// <param name="project">Project name</param>
        /// <param name="subproject">Subproject slug</param>
        /// <param name="language">Language to modify</param>
        public void UpdateTranslationFile(TranslationDelta delta, ProjectPrivate project, string subproject, string language)
        {
            //get the subproject
            var subprojectObj = FindSubproject(project, subproject);
            if(subprojectObj == null)
            {
                throw new FileNotFoundException("Unable to find subproject");
            }
            //get the filename
            var translationFileName = TranslationFileFilename(subprojectObj, language);

            //call the appropriate function
            switch (subprojectObj.Type)
            {
                case "qt":
                    QtTranslationFile.Update(translationFileName, delta);
                    break;
                default:
                    throw new ArgumentException("Unknown File Type");
            }
        }

        /// <summary>
        /// Initialize a translation file
        /// </summary>
        /// <param name="project">Project name</param>
        /// <param name="subproject">Subproject slug</param>
        /// <param name="language">Language in question</param>
        /// <returns>A translation file object</returns>
        public TranslationFile TranslationFile(ProjectPrivate project, string subproject, string language)
        {
            //find the subproject
            var subprojectObj = FindSubproject(project, subproject);
            if (subprojectObj == null)
            {
                throw new FileNotFoundException("Unable to find subproject");
            }
            //get the full translation file path
            var translationFileName = TranslationFileFilename(subprojectObj, language);

            if (!File.Exists(translationFileName)) throw new FileNotFoundException("Translation file could not be found");

            //call the appropriate function
            return subprojectObj.Type switch
            {
                "qt" => QtTranslationFile.LoadFromFile(translationFileName),
                "gettext" => GettextTranslationFile.LoadFromFile(translationFileName),
                "webext-json" => WebExtensionsJsonTranslationFile.LoadFromFile(translationFileName),
                _ => throw new ArgumentException("Unknown File Type")
            };
        }
    }
}