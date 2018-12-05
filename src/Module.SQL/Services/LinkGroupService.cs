using System;
using CoreModule;
using FirstFloor.ModernUI.Presentation;

namespace Module.SQL.Services
{
    public class LinkGroupService : ILinkGroupService
    {
        public LinkGroup GetLinkGroup()
        {
            var linkGroup = new LinkGroup
            {
                DisplayName = "SQL设置"
            };

            linkGroup.Links.Add(new Link
            {
                DisplayName = "SQL",
                Source = new Uri($"/Module.SQL;component/Views/{nameof(Views.SqlSettingsView)}.xaml", UriKind.Relative)
            });

            return linkGroup;
        }
    }
}
