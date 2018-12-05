using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FirstFloor.ModernUI.Assets
{
    public class TreeModel : INotifyPropertyChanged
    {
        #region 私有变量
        
        private string _id;
        
        private string _name;
        
        private string _icon;
        
        private bool _isChecked;
        
        private bool _isExpanded;
        
        private IList<TreeModel> _children;
        
        private TreeModel _parent;

        private object _tag;

        #endregion
        
        public TreeModel()
        {
            Children = new List<TreeModel>();
            _isChecked = false;
            IsExpanded = false;
            //_icon = "/Images/16_16/folder_go.png";
        }
        
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        
        public string ToolTip
        {
            get { return String.Format("{0}-{1}", Id, Name); }
        }
        
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }
        
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                NotifyPropertyChanged("IsExpanded");
            }
        }
        
        public TreeModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        
        public IList<TreeModel> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public static void AddChildrens(TreeModel parent, string[] children, int count)
        {
            for (var index = 0; index < count; index++)
            {
                var child = children[index];
                var childTreeModel = new TreeModel()
                {
                    Parent = parent,
                    Name = child
                };

                parent.Children.Add(childTreeModel);
            }
        }

        public object Tag
        {
            get => _tag;
            set => _tag = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
