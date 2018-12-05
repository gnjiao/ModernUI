using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using Core;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using OpcRcw.Comn;
using OpcRcw.Da; 

namespace Module.OPC.ViewModels
{    
    public class OpcImportViewModel : BindableBase
    {        
        public ObservableCollection<string> OpcServers { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<OpcChannelInfo> OpcChannels { get; set; } = new ObservableCollection<OpcChannelInfo>();
        public TreeView ChannelsTree { get; private set; }
        public string HostName { get; set; } = "localhost";
        public string OpcServer { get; set; }


        private bool _localRemoteChecked = true;
        public bool LocalRemoteChecked
        {
            get => _localRemoteChecked;
            set
            {
                _localRemoteChecked = value;
                HostName = _localRemoteChecked ? "localhost" : "";
                OnPropertyChanged(nameof(HostName));
            }
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand OpcConnectCommand { get; set; }
        public ICommand OkCommand { get; set; }


        private static void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)  
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void SaveOpcChannels(TreeNodeCollection root)
        {
            foreach (TreeNode node in root)
            {
                if (node.Nodes.Count > 0)
                    SaveOpcChannels(node.Nodes);
                else if (node.Checked)
                    OpcChannels.Add((OpcChannelInfo)node.Tag);
            }
        }

        public OpcImportViewModel()
        {
            ChannelsTree = new TreeView
            {                
                BorderStyle = BorderStyle.None,
                CheckBoxes = true,
                PathSeparator = ".",
                Dock = DockStyle.Fill,                
            };            

            ChannelsTree.AfterCheck += (sender, e) =>
            {
                if (e.Action != TreeViewAction.Unknown)
                {
                    if (e.Node.Nodes.Count > 0)
                    {
                        CheckAllChildNodes(e.Node, e.Node.Checked);
                    }
                }
            };

            //            AddOpcCommand = new DelegateCommand(() =>
            //            {
            //                var dlg = new ModernDialog
            //                {
            //                    Title = "OPCServer数据导入",
            //                    Content = new MainView(),
            //                    Width = 600,
            //                    Height = 800,                    
            //                };
            //
            //                dlg.Buttons = new Button[] {dlg.OkButton, dlg.CancelButton };
            //
            //                if (dlg.ShowDialog() == true)
            //                {
            //
            //                }
            //                
            //            });

            OkCommand = new RelayCommand(o =>
            {
                OpcChannels.Clear();

                if (ChannelsTree.Nodes.Count <= 0) return;

                SaveOpcChannels(ChannelsTree.Nodes);

                var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

                eventAggregator.GetEvent<OpcPointsChangedEvent>().Publish(OpcChannels);

                ChannelsTree.Nodes.Clear();                

                NavigationCommands.GoToPage.Execute($"/Module.OPC;component/Views/{nameof(Views.OpcSettingsView)}.xaml",null);

            });

            RefreshCommand = new RelayCommand(o =>
            {

                RefreshOpcServers();
                OnPropertyChanged(nameof(OpcServer));

            }, o => true);

            OpcConnectCommand = new RelayCommand(o =>
            {
                try
                {
                    OnConnect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }, o => true);
        }

        private void RefreshOpcServers()
        {
            OpcServers.Clear();
            try
            {
                var serverListType = Type.GetTypeFromProgID("OPC.ServerList", HostName);
                var serverList = (IOPCServerList2)Activator.CreateInstance(serverListType);
                Guid[] categories = { typeof(CATID_OPCDAServer10).GUID };
                serverList.EnumClassesOfCategories(categories.Length, categories, 0, null, out var enumGuids);
                int fetched;
                enumGuids.Reset();
                do
                {
                    var ids = new Guid[10];
                    enumGuids.Next(ids.Length, ids, out fetched);
                    for (var i = 0; i < fetched; i++)
                    {
                        serverList.GetClassDetails(ref ids[i], out var progId, out var _, out var _);
                        OpcServers.Add(progId);
                    }
                } while (fetched > 0);

                OpcServer = OpcServers.Count > 0 ? OpcServers[0] : "";
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void OnConnect()
        {
            var t = LocalRemoteChecked ? Type.GetTypeFromProgID(OpcServer) : Type.GetTypeFromProgID(OpcServer, HostName);

            var obj = Activator.CreateInstance(t);
            var srv = (IOPCBrowseServerAddressSpace)obj;
            try
            {
                for (; ; )
                    srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, "");
            }
            catch (COMException) { }


            ChannelsTree.Nodes.Clear();
            ImportOpcChannels(srv, ChannelsTree.Nodes);
        }

        void ImportOpcChannels(IOPCBrowseServerAddressSpace srv, TreeNodeCollection root)
        {
            srv.QueryOrganization(out var nsType);
            OpcRcw.Da.IEnumString es;
            if (nsType == OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL)
            {
                try { srv.BrowseOPCItemIDs(OPCBROWSETYPE.OPC_BRANCH, "", 0, 0, out es); }
                catch (COMException) { return; }

                int fetched;
                do
                {
                    var tmp = new string[100];
                    es.RemoteNext(tmp.Length, tmp, out fetched);
                    for (var i = 0; i < fetched; i++)
                    {
                        try { srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, tmp[i]); }
                        catch (Exception e)
                        {
                            LoggingService.Warn(
                                $"OPC server failed to handle OPC_BROWSE_DOWN request for item '{tmp[i]}' ({e.Message})");
                            continue;
                        }
                        var node = root.Add(tmp[i]);
                        ImportOpcChannels(srv, node.Nodes);
                        try { srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, ""); }
                        catch (Exception e)
                        {
                            LoggingService.Warn((
                                $"OPC server failed to handle OPC_BROWSE_UP request for item '{tmp[i]}' ({e.Message})"));
                        }
                    }
                } while (fetched > 0);

                try { srv.BrowseOPCItemIDs(OPCBROWSETYPE.OPC_LEAF, "", 0, 0, out es); }
                catch (COMException) { return; }
                IterateOpcItems(srv, root, es);
            }
            else if (nsType == OPCNAMESPACETYPE.OPC_NS_FLAT)
            {
                try { srv.BrowseOPCItemIDs(OPCBROWSETYPE.OPC_FLAT, "", 0, 0, out es); }
                catch (COMException) { return; }
                IterateOpcItems(srv, root, es);
            }
        }

        private void IterateOpcItems(IOPCBrowseServerAddressSpace srv, TreeNodeCollection root, OpcRcw.Da.IEnumString es)
        {
            int fetched;
            do
            {
                var tmp = new string[100];
                es.RemoteNext(tmp.Length, tmp, out fetched);
                for (var i = 0; i < fetched; i++)
                    AddTreeNode(srv, root, tmp[i]);
            } while (fetched > 0);
        }

        private void AddTreeNode(IOPCBrowseServerAddressSpace srv, TreeNodeCollection root, string tag)
        {
            var item = root.Add(tag);
            var channel = new OpcChannelInfo
            {
                ProgId = OpcServer,
                Host = HostName
            };
            srv.GetItemID(tag, out channel.Channel);
            item.Tag = channel;
        }

    }
}
