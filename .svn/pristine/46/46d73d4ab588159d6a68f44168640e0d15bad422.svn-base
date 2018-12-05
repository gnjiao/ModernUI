using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreModule;
using FirstFloor.ModernUI.Presentation;

namespace Module.Sample.Services
{
    public class LinkGroupService : ILinkGroupService
    {
        public LinkGroup GetLinkGroup()
        {
            var linkGroup = new LinkGroup
            {
                DisplayName = "SampleModule"
            };

            linkGroup.Links.Add(new Link
            {
                DisplayName = "SampleWindow",
                Source = new Uri($"/Module.Sample;component/Views/{nameof(Views.SampleWindow)}.xaml", UriKind.Relative)
            });

            return linkGroup;
        }
    }
}
