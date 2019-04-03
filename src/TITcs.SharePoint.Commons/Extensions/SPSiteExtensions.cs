using Microsoft.SharePoint;
using System;
using System.Linq;

namespace TITcs.SharePoint.Commons.Extensions
{
    public static class SPSiteExtensions
    {
        /// <summary>
        /// Run code with elevated privileges onto the site.
        /// </summary>
        /// <param name="site">The current site.</param>
        /// <param name="codeToRunElevated">Code to run elevated.</param>
        public static void RunWithElevatedPrivileges(this SPSite site, Action<SPSite> codeToRunElevated)
        {
            var siteId = site.ID;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (var newSite = new SPSite(siteId))
                {
                    codeToRunElevated(newSite);
                }
            });
        }

        /// <summary>
        /// Activate the specified feature at the web context.
        /// </summary>
        /// <param name="site">Site context in which to activate the feature.</param>
        /// <param name="guid">ID of the feature to activate.</param>
        public static void ActivateFeature(this SPSite site, string guid)
        {
            var featureId = new Guid(guid);

            if (site.Features.Cast<SPFeature>().Any(f => f.DefinitionId == featureId))
            {
                site.Features.Add(featureId, true);
            }
        }
    }
}
