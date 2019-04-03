using Microsoft.SharePoint;
using System;
using System.Linq;

namespace TITcs.SharePoint.Commons.Extensions
{
    public static class SPWebExtensions
    {
        /// <summary>
        /// Change the default page for a site.
        /// </summary>
        /// <param name="web">Web context of the modification.</param>
        /// <param name="pageUrl">Relative path of the page.</param>
        public static void ChangeDefaultPage(this SPWeb web, string pageUrl)
        {
            if (web == null) throw new NullReferenceException("web");
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl");

            // stores the current default page for later restoration
            var previousDefaultPage = web.RootFolder.WelcomePage;

            web.SaveToProperties(Constants.SHAREPOINT_PREVIOUS_DEFAULT_PAGE, previousDefaultPage);

            var rootFolder = web.RootFolder;

            rootFolder.WelcomePage = pageUrl;
            rootFolder.Update();
        }

        /// <summary>
        /// Reverts the default page for the value stored in the web properties.
        /// </summary>
        /// <param name="web">Web context of the modification.</param>
        public static void RevertToPreviousDefaultPage(this SPWeb web)
        {
            if (web == null) throw new NullReferenceException("web");

            // stores the current default page for later restoration
            if (!web.AllProperties.ContainsKey(Constants.SHAREPOINT_PREVIOUS_DEFAULT_PAGE)) return;

            var previousDefaultPage = web.AllProperties[Constants.SHAREPOINT_PREVIOUS_DEFAULT_PAGE] as string;
            var rootFolder = web.RootFolder;
            rootFolder.WelcomePage = previousDefaultPage;
            rootFolder.Update();
        }

        /// <summary>
        /// Stores configuration at the SPWeb properties.
        /// </summary>
        /// <param name="web">Web context in which the value will be saved.</param>
        /// <param name="key">Property key.</param>
        /// <param name="value">Property value.</param>
        public static void SaveToProperties(this SPWeb web, string key, string value)
        {
            if (web == null) throw new NullReferenceException("web");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("value");

            if (!web.AllProperties.ContainsKey(key))
            {
                web.AllProperties.Add(key, value);
            }
            else
            {
                web.AllProperties[key] = value;
            }

            web.Update();
        }

        /// <summary>
        /// Activate the specified feature at the web context.
        /// </summary>
        /// <param name="web">Web context in which to activate the feature.</param>
        /// <param name="guid">ID of the feature to activate.</param>
        public static void ActivateFeature(this SPWeb web, string guid)
        {
            var featureId = new Guid(guid);

            if (web.Features.Cast<SPFeature>().Any(f => f.DefinitionId == featureId))
            {
                web.Features.Add(featureId, true);
            }
        }

        /// <summary>
        /// Deletes the specified file at the web context.
        /// </summary>
        /// <param name="web">Web context in which to delete the file.</param>
        /// <param name="relativePath">Path of the file to delete.</param>
        public static void DeleteFile(this SPWeb web, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;

            if (!relativePath.StartsWith("/"))
                relativePath = "/" + relativePath;

            var file = web.Site.Url + relativePath;

            var spFile = web.GetFile(file);

            if (spFile.Exists)
            {
                spFile.Delete();
            }
        }
    }
}
