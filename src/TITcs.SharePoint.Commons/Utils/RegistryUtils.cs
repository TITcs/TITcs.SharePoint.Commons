using Microsoft.Win32;

namespace TITcs.SharePoint.Logger.Utils
{
    public static class RegistryUtils
    {
        /// <summary>
        /// Reads an entry from the windows registry.
        /// </summary>
        /// <param name="keyName">The full registry path of the key, beginning with a valid registry root, such as "HKEY_CURRENT_USER".</param>
        /// <param name="valueName">The name of the name/value pair.</param>
        /// <returns>The value associated with the given key.</returns>
        public static object Read(string keyName, string valueName)
        {
            return Registry.GetValue(keyName, valueName, default(object));
        }
    }
}
