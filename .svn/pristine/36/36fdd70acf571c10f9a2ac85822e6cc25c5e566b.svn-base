using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Core;

namespace Module.OPC.Models
{
    sealed class ChannelFactory
    {
        //Prevent class reation
        private ChannelFactory() { }

        public static IPoint CreateChannel(XmlElement node)
        {
            string name = node.Attributes["name"].Value;
            string opcChannel = node.Attributes["opcChannel"].Value;
            string opcServer = node.Attributes["opcServer"].Value;
            string opcHost = node.Attributes["opcHost"].Value;

            return CreateChannel(name, "",ReadWriteFlags.ReadWrite, opcChannel, opcServer, opcHost);
        }

        public static IPoint CreateChannel(string name, string device, ReadWriteFlags readwrite, string opcChannel, string opcServer, string opcHost)
        {
            return new OpcPoint(name,device,readwrite, null,null, opcChannel, opcServer, opcHost);
        }

        public static void SaveChannel(XmlElement node, IPoint channel)
        {
            OpcPoint channelBase = (OpcPoint)channel;
            node.SetAttribute("name", channelBase.Name);
            node.SetAttribute("opcChannel", channelBase.OpcChannel);
            node.SetAttribute("opcServer", channelBase.OpcServer);
            node.SetAttribute("opcHost", channelBase.OpcHost);
        }
    }
}
