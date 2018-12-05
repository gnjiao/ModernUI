using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreModule;
using FirstFloor.ModernUI.Presentation;

namespace Module.PLC.Services
{
    public class LinkGroupService : ILinkGroupService
    {
        public LinkGroup GetLinkGroup()
        {
            var linkGroup = new LinkGroup
            {
                DisplayName = "PLC管理"
            };

            linkGroup.Links.Add(new Link
            {
                Source = new Uri($"/Module.PLC;component/Views/{nameof(Views.PlcSettingsView)}.xaml", UriKind.Relative)
            });

            return linkGroup;
        }
    }
}
