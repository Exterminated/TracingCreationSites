using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Devyatkin.TracingCreationSites.Features.Devyatkin.TracingCreationSites_Feature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("29a3cc7e-2a7c-4863-8638-566ad15586a1")]
    public class DevyatkinEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (SPSite site = properties.Feature.Parent as SPSite)
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList webRegistry = web.Lists.TryGetList(Constants.WebRegistry.ListTitle);
                    if (webRegistry != null)
                    {
                        const SPEventReceiverType _eventType = SPEventReceiverType.ItemAdding;
                        web.Lists[Constants.WebRegistry.ListTitle].EventReceivers.Add(_eventType, Assembly.GetExecutingAssembly().FullName, "Devyatkin.TracingCreationSites.EventRecivers.WebRegistryEventReceiver.WebRegistryEventReceiver");
                        const SPEventReceiverType _eventType2 = SPEventReceiverType.ItemDeleting;
                        web.Lists[Constants.WebRegistry.ListTitle].EventReceivers.Add(_eventType2, Assembly.GetExecutingAssembly().FullName, "Devyatkin.TracingCreationSites.EventRecivers.WebRegistryEventReceiver.WebRegistryEventReceiver");
                        const SPEventReceiverType _eventType3 = SPEventReceiverType.ItemUpdating;
                        web.Lists[Constants.WebRegistry.ListTitle].EventReceivers.Add(_eventType3, Assembly.GetExecutingAssembly().FullName, "Devyatkin.TracingCreationSites.EventRecivers.WebRegistryEventReceiver.WebRegistryEventReceiver");


                    }
                    else
                    {
                        //throw new Exception("WebRegistryList Error");
                        Logger.WriteLog(Logger.Category.Unexpected, "Devyatkin.TracingCreationSites.Features.Devyatkin.TracingCreationSites_Feature", "Error in feature activating");
                    }


                }
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            using (SPSite site = properties.Feature.Parent as SPSite)
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList oList = web.Lists[Constants.WebRegistry.ListTitle];
                    const SPEventReceiverType _eventType = SPEventReceiverType.ItemAdding;
                    const SPEventReceiverType _eventType2 = SPEventReceiverType.ItemDeleting;
                    const SPEventReceiverType _eventType3 = SPEventReceiverType.ItemUpdating;
                    for (int i = oList.EventReceivers.Count - 1; i >= 0; i--)
                    {
                        if (oList.EventReceivers[i].Type.Equals(_eventType) || oList.EventReceivers[i].Type.Equals(_eventType2) || oList.EventReceivers[i].Type.Equals(_eventType3))
                        {
                            try
                            {
                                oList.EventReceivers[i].Delete();
                            }
                            catch (Exception e)
                            {
                                Logger.WriteLog(Logger.Category.High, "DevyatkinEventReceiver", e.ToString());
                            }
                        }
                    }
                    SPList list = web.Lists.TryGetList(Constants.WebRegistry.ListTitle);
                    web.Lists.Delete(list.ID);
                }
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
