using Microsoft.SharePoint;

namespace Devyatkin.TracingCreationSites
{
    public static class Queries
    {
        //Лимит получаемых элементов в запросе
        private static uint QueryLimit => 2000;
        public static SPListItemCollection GetWebRegistrySiteByURL(SPWeb web, string url)
        {
            SPQuery query = new SPQuery()
            {
                Query = @"<Where><Eq><FieldRef Name='SiteRelativeUrl' /><Value Type='Text'>"+ url + @"</Value></Eq></Where>",
                ViewFields = @"<FieldRef Name='CreatedDate' /><FieldRef Name='Template' /><FieldRef Name='SiteRelativeUrl' />",
                RowLimit = QueryLimit
            };
            SPList list = web.Lists.TryGetList(Constants.WebRegistry.ListTitle);
            SPListItemCollection items = list.GetItems(query);
            return items;
        }
    }
}
