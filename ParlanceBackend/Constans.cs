using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParlanceBackend
{
    public static class Constants
    {
        public static readonly string USER_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static readonly string CONFIGURATION_FOLDER = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) :
            $"{USER_FOLDER}/.config";
        public static readonly string DOCUMENTS_FOLDER = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) :
            $"{USER_FOLDER}/Documents";
    }
}
