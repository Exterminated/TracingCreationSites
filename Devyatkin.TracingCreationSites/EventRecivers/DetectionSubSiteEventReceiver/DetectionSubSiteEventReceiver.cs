using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Devyatkin.TracingCreationSites.DetectionSubSiteEventReceiver
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class DetectionSubSiteEventReceiver : SPWebEventReceiver
    {
        /// <summary>
        /// A site was deleted.
        /// </summary>
        public override void WebDeleted(SPWebEventProperties properties)
        {
            base.WebDeleted(properties);
            //TODO Добавить обработку события удаления вебы
            using (SPWeb web = properties.Web.Site.RootWeb) {
                SPListItemCollection delitingSitesRecoeds = Queries.GetWebRegistrySiteByURL(web, properties.FullUrl);
                Logger.WriteLog(Logger.Category.Information, "DetectionSubSiteEventReceiver", "Event WebDeleted. Strarting removing sites from list. Finded "+delitingSitesRecoeds.Count.ToString()+" items");
                if (delitingSitesRecoeds != null && delitingSitesRecoeds.Count > 0) {
                    foreach (SPListItem item in delitingSitesRecoeds) {
                        using (EventReceiverScope scope = new EventReceiverScope(false))
                        {
                            item.Delete();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// A site is being provisioned
        /// </summary>
        public override void WebAdding(SPWebEventProperties properties)
        {
            base.WebAdding(properties);
            using (SPWeb web = properties.Web) {
                SPList webRegistryList = web.Lists.TryGetList(Constants.WebRegistry.ListTitle);
                if (webRegistryList != null)
                {
                    SPListItem webRegistryItem = webRegistryList.Items.Add();
                    webRegistryItem[Constants.WebRegistry.SiteRelativeUrl] = SPUrlUtility.CombineUrl(properties.FullUrl, properties.NewServerRelativeUrl);
                    webRegistryItem[Constants.WebRegistry.Template] = properties.Web.ID;
                    webRegistryItem[Constants.WebRegistry.CreatedDate] = DateTime.Now;
                    using (EventReceiverScope scope = new EventReceiverScope(false))
                    {
                        webRegistryItem.Update();
                    }
                    Logger.WriteLog(Logger.Category.Information, "DetectionSubSiteEventReceiver", "Event WebAdding. Added new Web Registry element in list");
                }
                else
                {
                    Logger.WriteLog(Logger.Category.High, "DetectionSubSiteEventReceiver", "Event WebAdding. WebRegistry List is null");
                }
            }
        }

    }
}