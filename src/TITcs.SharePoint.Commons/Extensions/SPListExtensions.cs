using Microsoft.SharePoint;

namespace TITcs.SharePoint.Commons.Extensions
{
    public static class SPListExtensions
    {
        /// <summary>
        /// Returns the SPFolder object associated with the given path.
        /// </summary>
        /// <param name="list">List in which the folder in located.</param>
        /// <param name="folder">Folder path inside the list</param>
        /// <returns>Returns the SPFolder object associated with the given path.</returns>
        public static SPFolder FindFolder(this SPList list, string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) return null;

            return list.RootFolder.SubFolders[folder];
        }
    }
}
