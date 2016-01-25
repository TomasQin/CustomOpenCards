using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
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

        private const int ShowNumber = 30;

        private List<SelectedMemberNode> Nodes { get; set; }

        private List<NodeControl> NodeControlList { get; set; }

        /// <summary>
        /// 记录所有从服务器查询到的member
        /// UI显示以30条为界限(暂定)
        /// </summary>
        private readonly List<ListViewMemberItem> _totalMemberItems;

        readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private Thread _searchTask;
        string _searchText = string.Empty;
        #endregion

        public ObservableCollection<ListViewMemberItem> MemberList { get; set; }

        public ListViewMemberSelectControl( )
        {
            InitializeComponent();
            NodeControlList = new List<NodeControl>();
            _totalMemberItems = new List<ListViewMemberItem>();
            MemberList = new ObservableCollection<ListViewMemberItem>();
            AuthorListBox.DataContext = MemberList;
        }

        public Param ParamItem { get; set; }

        public void SaveParameter( )
        {
            foreach(var item in NodeControlList)
            {
                item.Save();
            }
        }

        public void InitData( )
        {
            for(int i = 0; i < 2000; i++)
            {
                if(i < 3)
                {
                    MemberList.Add(new ListViewMemberItem(string.Format("孙传芳{0}", i), string.Format("{0}SCF", i)));
                    MemberList.Add(new ListViewMemberItem(string.Format("吴佩浮{0}", i), string.Format("{0}WPF", i)));
                    MemberList.Add(new ListViewMemberItem(string.Format("阎锡山{0}", i), string.Format("{0}YXS", i)));
                    MemberList.Add(new ListViewMemberItem(string.Format("张作霖{0}", i), string.Format("{0}ZZL", i)));
                    MemberList.Add(new ListViewMemberItem(string.Format("袁世凯{0}", i), string.Format("{0}YSK", i)));
                    MemberList.Add(new ListViewMemberItem(string.Format("孙中山{0}", i), string.Format("{0}SZS", i)));
                    MemberList.Add(new ListViewMemberItem(string.Format("蔡锷{0}", i), string.Format("{0}CE", i)));
                }
                _totalMemberItems.Add(new ListViewMemberItem(string.Format("孙传芳{0}", i), string.Format("{0}SCF", i)));
                _totalMemberItems.Add(new ListViewMemberItem(string.Format("吴佩浮{0}", i), string.Format("{0}WPF", i)));
                _totalMemberItems.Add(new ListViewMemberItem(string.Format("阎锡山{0}", i), string.Format("{0}YXS", i)));
                _totalMemberItems.Add(new ListViewMemberItem(string.Format("张作霖{0}", i), string.Format("{0}ZZL", i)));
                _totalMemberItems.Add(new ListViewMemberItem(string.Format("袁世凯{0}", i), string.Format("{0}YSK", i)));
                _totalMemberItems.Add(new ListViewMemberItem(string.Format("孙中山{0}", i), string.Format("{0}SZS", i)));
                _totalMemberItems.Add(new ListViewMemberItem(string.Format("蔡锷{0}", i), string.Format("{0}CE", i)));
            }


            Nodes = ParamItem.SelectedMemberNode.Select(item => new SelectedMemberNode(item.Caption)).ToList();

            SelectedPopup.Height = Nodes.Count * 110 + 38;

            var guid = Guid.NewGuid();
            for(int i = 0; i < Nodes.Count; i++)
            {
                var control = new NodeControl(Nodes[i].NodeName, i == 0, guid.ToString());
                control.RemoveAuthorEventHandler += HandlerRemoveAuthorEvent;
                control.Height = 110;

                NodeControlList.Add(control);
                ChildrenPanel.Children.Add(control);
            }

            if(_searchTask == null)
            {
                _searchTask = new Thread(SeatchMember);
                _searchTask.Start();
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
            foreach(var item in NodeControlList)
            {

                list.AddRange(item.GetData());
            }
            if(list.Count < 1) return;

            var displayName = string.Empty;
            for(int i = 0; i < list.Count; i++)
            {
                if(i == 0)
                {
                    displayName = list[i].Name;
                }
                else
                {
                    if(i > 2)
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
            //var searchText = SearchInpuTextBox.Text;
            //SeatchMember(searchText);
            _searchText = SearchInpuTextBox.Text;
            _autoResetEvent.Set();
        }
      
        private void SeatchMember( )
        {
            while(true)
            {
                _autoResetEvent.WaitOne();

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(( ) =>
                {
                    MemberList.Clear();
                }));

                if(string.IsNullOrEmpty(_searchText))
                {
                    for(int i = 0; i < ShowNumber; i++)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(( ) =>
                        {
                            MemberList.Add(_totalMemberItems[i]);
                        }));
                    }
                }
                else
                {
                    foreach(var item in _totalMemberItems)
                    {
                        if(MemberList.Count > ShowNumber)
                        {
                            break;
                        }
                        if(item.PyName.ToLower().Contains(_searchText.ToLower()))
                        {
                            var item1 = item;
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(( ) =>
                            {
                                MemberList.Add(item1);
                            }));
                        }
                    }
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
            foreach(var item in NodeControlList)
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
            if(item == null) return;
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
            if(e.AuthorEnitiy == null) return;
            var firstOrDefault = MemberList.FirstOrDefault(p => p == e.AuthorEnitiy);
            if(firstOrDefault == null) return;
            firstOrDefault.IsVisible = Visibility.Visible;
            AuthorListBox.SelectedItem = firstOrDefault;
        }
        #endregion

    }
}