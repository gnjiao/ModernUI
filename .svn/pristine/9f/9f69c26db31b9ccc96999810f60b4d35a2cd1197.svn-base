using System;
using CoreModule;
using FirstFloor.ModernUI.Presentation;

namespace Module.OPC.Services
{
    public class LinkGroupService : ILinkGroupService
    {
        public LinkGroup GetLinkGroup()
        {
            var linkGroup = new LinkGroup
            {
                DisplayName = "OPC设置"
            };

            linkGroup.Links.Add(new Link
            {
                DisplayName = "点信息",
                Source = new Uri($"/Module.OPC;component/Views/{nameof(Views.OpcSettingsView)}.xaml", UriKind.Relative)
            });

            linkGroup.Links.Add(new Link
            {
                DisplayName = "OPC导入",
                Source = new Uri($"/Module.OPC;component/Views/{nameof(Views.OpcImportView)}.xaml", UriKind.Relative)
            });

            return linkGroup;
        }
    }
}
