using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.OPC.Models
{
    [Serializable]
    public abstract class StringConstants
    {
        public static string PluginName = "OPC Connection Plugin";
        public static string PluginId = "opc_connection_plug";

        public static string PropertyCommandName = "OPC properties...";
        public static string CommunicationGroupName = "Communication";
    }
}
