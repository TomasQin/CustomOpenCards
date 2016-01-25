using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CustomUI.Entitys;
using CustomUI.Interface;
using XmlFileTransferHandle.XmlEntitys;
using SelectedMemberNode = CustomUI.Entitys.SelectedMemberNode;

namespace CustomUI.Controls
{
    /// <summary>
    /// Interaction logic for AuthorSelectControl.xaml
    /// </summary>
    public partial class ListViewMemberSelectControl : IParameterBasicInterface
    {
        #region [Field]
        private List<SelectedMemberNode> Nodes { get; set; }
        private List<ListViewMemberItem> AuthorList { get; set; }
        private List<NodeControl> NodeControlList { get; set; }
        #endregion

        public ListViewMemberSelectControl()
        {
            InitializeComponent();
            NodeControlList = new List<NodeControl>();
        }

        public Param ParamItem { get; set; }

        public void SaveParameter()
        {
            foreach (var item in NodeControlList)
            {
                item.Save();
            }
        }

        public void InitData()
        {
            //todo 根据sourceID来去查DB
            AuthorList = new List<ListViewMemberItem>()
            {
                new ListViewMemberItem("孙传芳", "SCF"),
                new ListViewMemberItem("吴佩浮", "WPF"),
                new ListViewMemberItem("阎锡山", "YXS"),
                new ListViewMemberItem("张作霖", "ZZL"),
                new ListViewMemberItem("袁世凯", "YSK"),
                new ListViewMemberItem("孙中山", "SZS"),
                new ListViewMemberItem("蔡锷", "CE")
            };

            Nodes = ParamItem.SelectedMemberNode.Select(item => new SelectedMemberNode(item.Caption)).ToList();


            AuthorListBox.ItemsSource = AuthorList;
            SelectedPopup.Height = Nodes.Count * 110 + 38;

            var guid = Guid.NewGuid();
            for (int i = 0; i < Nodes.Count; i++)
            {
                var control = new NodeControl(Nodes[i].NodeName, i == 0, guid.ToString());
                control.RemoveAuthorEventHandler += HandlerRemoveAuthorEvent;
                control.Height = 110;

                NodeControlList.Add(control);
                ChildrenPanel.Children.Add(control);
            }
        }

        #region [Private Method]

        private void ShowPopupBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPopup.IsOpen = true;
        }

        private void OkBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var list = new List<SelectedMemberNodeItem>();
            foreach (var item in NodeControlList)
            {

                list.AddRange(item.GetData());
            }
            if (list.Count < 1) return;

            var displayName = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    displayName = list[i].Name;
                }
                else
                {
                    if (i > 2)
                    {
                        displayName = string.Format("{0}...", displayName);
                        break;
                    }
                    displayName = string.Format("{0},{1}", displayName, list[i].Name);
                }
            }

            ContentLabel.Content = displayName;
            SelectedPopup.IsOpen = false;
        }

        private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPopup.IsOpen = false;
        }

        private void SearchBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var searchText = SearchInpuTextBox.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                foreach (var item in AuthorList)
                {
                    item.IsVisible = Visibility.Visible;
                }
            }
            else
            {
                foreach (var item in AuthorList)
                {
                    item.IsVisible = item.PyName.ToLower().Contains(searchText.ToLower()) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 把作者名字从右边的子控件移入到左边的全部列表里
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftMoveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in NodeControlList)
            {
                item.RemoveItem();
            }
        }

        /// <summary>
        /// 把作者名字从左边移入到右边的子控件中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightMoveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var item = AuthorListBox.SelectedItem as ListViewMemberItem;
            if (item == null) return;
            item.IsVisible = Visibility.Collapsed;
            NodeControlList.ForEach(p => p.AddItem(item));
        }

        private void MoveUp_OnClick(object sender, RoutedEventArgs e)
        {
            NodeControlList.ForEach(p => p.ItemMoveUp());
        }

        private void MoveDown_OnClick(object sender, RoutedEventArgs e)
        {
            NodeControlList.ForEach(p => p.ItemMoveDown());
        }
        #endregion

        #region [Shortcut Operation]
        private void SearchInpuTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBtn_OnClick(null, null);
        }

        private void AuthorListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RightMoveBtn_OnClick(null, null);
        }

        void HandlerRemoveAuthorEvent(object sender, AuthorEventArgs e)
        {
            if (e.AuthorEnitiy == null) return;
            var firstOrDefault = AuthorList.FirstOrDefault(p => p == e.AuthorEnitiy);
            if (firstOrDefault == null) return;
            firstOrDefault.IsVisible = Visibility.Visible;
            AuthorListBox.SelectedItem = firstOrDefault;
        }
        #endregion

    }
}