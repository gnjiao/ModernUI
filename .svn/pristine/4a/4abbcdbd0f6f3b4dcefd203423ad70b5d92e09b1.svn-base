using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FirstFloor.ModernUI.Assets
{
    /// <summary>
    /// Interaction logic for ModernCheckboxTree.xaml
    /// </summary>
    public partial class ModernCheckboxTree : UserControl
    {

        public ModernCheckboxTree()
        {
            InitializeComponent();
        }

        //checkBox单击事件
        private void menuSelectChanged(object sender, RoutedEventArgs e)
        {
            //得到单击的对象
            TreeViewItem tree = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            TreeModel tree2 = (TreeModel)tree.Header;
            if (tree2.IsChecked == true)
            {
                //如果选中则其父项也应该选中
                if (tree2.Parent != null)
                {
                    tree2.Parent.IsChecked = true;

                }
                if (tree2.Children != null)
                {
                    //如果选中则他的所有子类都要被选中
                    foreach (TreeModel child in tree2.Children)
                    {
                        child.IsChecked = true;
                    }
                }
            }
            else
            {
                //如果取消选中子项也应该取消选中
                foreach (TreeModel child in tree2.Children)
                {
                    child.IsChecked = false;
                }
                int i = 0;
                //当其父类不为空，可以判断是否和自己同级的IsChecked都为false，如果是父类就也不被选中
                if (tree2.Parent != null)
                {
                    foreach (TreeModel child2 in tree2.Parent.Children)
                    {
                        if (child2.IsChecked == false)
                            i++;
                        else
                            break;
                    }
                    if (i == tree2.Parent.Children.Count)
                    {
                        tree2.Parent.IsChecked = false;
                    }
                }
            }
        }


        /// <summary>
        /// 控件数据
        /// </summary>
        private IList<TreeModel> _itemsSourceData;
        public IList<TreeModel> ItemsSourceData
        {
            get { return _itemsSourceData; }
            set
            {
                _itemsSourceData = value;
                tvZsmTree.ItemsSource = _itemsSourceData;
            }
        }




        /// <summary>
        /// 设置对应Id的项为选中状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="treeList"></param>
        /// <returns></returns>
        public int SetCheckedById(string id, IList<TreeModel> treeList)
        {
            foreach (var tree in treeList)
            {
                if (tree.Id.Equals(id))
                {
                    tree.IsChecked = true;
                    return 1;
                }
                if (SetCheckedById(id, tree.Children) == 1)
                {
                    return 1;
                }
            }

            return 0;
        }
        /// <summary>
        /// 设置对应Id的项为选中状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SetCheckedById(string id)
        {
            foreach (var tree in ItemsSourceData)
            {
                if (tree.Id.Equals(id))
                {
                    tree.IsChecked = true;
                    return 1;
                }
                if (SetCheckedById(id, tree.Children) == 1)
                {
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <returns></returns>
        public IList<TreeModel> CheckedItemsIgnoreRelation()
        {

            return GetCheckedItemsIgnoreRelation(_itemsSourceData);
        }
        /// <summary>
        /// 私有方法，忽略层次关系的情况下，获取选中项
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IList<TreeModel> GetCheckedItemsIgnoreRelation(IList<TreeModel> list)
        {
            IList<TreeModel> treeList = new List<TreeModel>();
            foreach (var tree in list)
            {
                if (tree.IsChecked)
                {
                    treeList.Add(tree);
                }
                foreach (var child in GetCheckedItemsIgnoreRelation(tree.Children))
                {
                    treeList.Add(child);
                }
            }
            return treeList;
        }



        #region 选中某项单击右键
        /// <summary>
        /// 选中某项所有子项菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelectAllChild_Click(object sender, RoutedEventArgs e)
        {
            if (tvZsmTree.SelectedItem != null)
            {
                TreeModel tree = (TreeModel)tvZsmTree.SelectedItem;
                tree.IsChecked = true;
                SetChildrenChecked(true, tree);
            }
        }
        /// <summary>
        /// 设置所有子项的选中状态
        /// </summary>
        /// <param name="isChecked"></param>
        /// <param name="tree"></param>
        public void SetChildrenChecked(bool isChecked, TreeModel tree)
        {
            foreach (TreeModel child in tree.Children)
            {
                child.IsChecked = isChecked;
                SetChildrenChecked(isChecked, child);
            }
        }
        #endregion
        
        #region 右键菜单事件

        /// <summary>
        /// 设置所有子项展开状态
        /// </summary>
        /// <param name="isExpanded"></param>
        /// <param name="tree"></param>
        public void SetChildrenExpanded(bool isExpanded, TreeModel tree)
        {
            foreach (TreeModel child in tree.Children)
            {
                child.IsExpanded = isExpanded;
                SetChildrenExpanded(isExpanded, child);
            }
        }

        /// <summary>
        /// 全部展开菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExpandAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsExpanded = true;
                SetChildrenExpanded(true, tree);
            }
        }

        /// <summary>
        /// 全部折叠菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUnExpandAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsExpanded = false;
                SetChildrenExpanded(false, tree);
            }
        }

        /// <summary>
        /// 全部选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsChecked = true;
                SetChildrenExpanded(true, tree);
            }
        }

        /// <summary>
        /// 全部取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeModel tree in tvZsmTree.ItemsSource)
            {
                tree.IsChecked = false;
                SetChildrenChecked(false, tree);
            }
        }
        #endregion
        
        /// <summary>
        /// 鼠标右键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }

        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }
    }
}
