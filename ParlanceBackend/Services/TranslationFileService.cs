using System;
using System.IO;
using System.Threading.Tasks;
using Karambolo.Common;
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

            ITranslationFileFormat.LoaderForFormat(subprojectObj.Type).Update(translationFileName, delta);
        }

        /// <summary>
        /// Initialize a translation file
        /// </summary>
        /// <param name="project">Project name</param>
        /// <param name="subproject">Subproject slug</param>
        /// <param name="language">Language in question</param>
        /// <returns>A translation file object</returns>
        public async Task<TranslationFile> TranslationFile(ProjectPrivate project, string subproject, string language)
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
            return await ITranslationFileFormat.LoaderForFormat(subprojectObj.Type).LoadFromFile(translationFileName);
        }

        public async Task<TranslationFile> CreateTranslationFile(ProjectPrivate project, string subproject, string language)
        {
            var subprojectObj = FindSubproject(project, subproject);
            if (subprojectObj == null)
            {
                throw new FileNotFoundException("Unable to find subproject");
            }

            var translationFileName = TranslationFileFilename(subprojectObj, ITranslationFileFormat.LoaderForFormat(subprojectObj.Type).TransformLanguageName(language));
            if (File.Exists(translationFileName)) throw new FileNotFoundException("Translation file could not be found");

            var baseTranslationFileName = TranslationFileFilename(subprojectObj, subprojectObj.BaseLang);
            var baseTranslationFile = await ITranslationFileFormat.LoaderForFormat(subprojectObj.Type).LoadFromFile(baseTranslationFileName);

            baseTranslationFile.DestinationLanguage = language;
            foreach (var message in baseTranslationFile.Messages)
            {
                Array.Fill(message.Translation, "");
            }

            await ITranslationFileFormat.LoaderForFormat(subprojectObj.Type).SaveToFile(translationFileName, baseTranslationFile);

            return baseTranslationFile;
        }

        public string NormaliseLanguageName(ProjectPrivate project, string subproject, string language)
        {
            
            var subprojectObj = FindSubproject(project, subproject);
            if (subprojectObj == null)
            {
                throw new FileNotFoundException("Unable to find subproject");
            }

            return ITranslationFileFormat.LoaderForFormat(subprojectObj.Type).NormaliseLanguageName(language);
        }
    }
}