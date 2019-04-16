using Microsoft.SharePoint;
using System;

namespace Devyatkin.TracingCreationSites.EventRecivers.WebRegistryEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class WebRegistryEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            //SPListItem currentItem = properties.AfterProperties;
            SPList currentList = properties.List;
            using (SPWeb web = properties.OpenWeb())
            {
                if (currentList.Title == Constants.WebRegistry.ListTitle)
                {
                    SPWeb chWeb = FindSubSite(web, properties.AfterProperties[Constants.WebRegistry.SiteRelativeUrl].ToString());
                    if (chWeb == null)
                    {
                        properties.Status = SPEventReceiverStatus.CancelWithError;
                        properties.ErrorMessage = "You can't create record with non existing subsite URL";
                        Logger.WriteLog(Logger.Category.Information, "Devyatkin.TracingCreationSites", "Blocked by adding with nonexistent address in WebRegistry");
                    }
                    else Logger.WriteLog(Logger.Category.Information, "Devyatkin.TracingCreationSites", "Added new record in WebRegistry");
                }
            }

        }

        /// <summary>
        /// An item is being deleted
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleting(properties);

            SPListItem currentItem = properties.ListItem;
            SPList currentList = properties.List;
            using (SPWeb web = properties.OpenWeb())
            {
                if (currentList.Title == Constants.WebRegistry.ListTitle)
                {
                    SPWeb chWeb = FindSubSite(web, currentItem[Constants.WebRegistry.SiteRelativeUrl].ToString());
                    if (chWeb == null)
                    {
                        properties.Status = SPEventReceiverStatus.Continue;
                        Logger.WriteLog(Logger.Category.Medium, "Devyatkin.TracingCreationSites", "Deleted item with not existin site url in WebRegistry");
                    }
                    else
                    {
                        properties.Status = SPEventReceiverStatus.CancelWithError;
                        properties.ErrorMessage = "You can't delete record of subsite with existing URL";
                        Logger.WriteLog(Logger.Category.Information, "Devyatkin.TracingCreationSites", "Detected deleted item, stoped in WebRegistry");                        
                    }
                }
                else
                {
                    Logger.WriteLog(Logger.Category.Information, "Devyatkin.TracingCreationSites", "Wrong list");
                }
            }

        }
        public static SPWeb FindSubSite(SPWeb rootWeb, string subSiteUrl)
        {
            SPWebCollection webCollection = rootWeb.Webs;
            if (webCollection.Count > 0)
            {
                SPWeb subsite = null;
                foreach (SPWeb web in webCollection)
                {
                    if (web.Url == subSiteUrl)
                    {
                        subsite = web;
                        break;
                    }
                    else if (web.Webs.Count > 0)
                    {
                        subsite = FindSubSite(web, subSiteUrl);
                        break;
                    }

                }
                return subsite;

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// An item is being updated
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
            if (properties.List.Title == Constants.WebRegistry.ListTitle)
            {
                properties.Status = SPEventReceiverStatus.CancelWithError;
                properties.ErrorMessage = "You can't editing this item in WebRegistry list";
                Logger.WriteLog(Logger.Category.Information, "Devyatkin.TracingCreationSites", "Stoped editing item");
            }
        }
    }
}