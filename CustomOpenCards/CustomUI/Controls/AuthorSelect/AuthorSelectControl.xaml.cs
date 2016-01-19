using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CustomUI.Entitys;

namespace CustomUI.Controls.AuthorSelect
{
    /// <summary>
    /// Interaction logic for AuthorSelectControl.xaml
    /// </summary>
    public partial class AuthorSelectControl
    {
        #region [Field]
        private List<SelectedAuthorNode> Nodes { get; set; }
        private List<Author> AuthorList { get; set; }
        private List<NodeControl> NodeControlList { get; set; }
        #endregion

        public AuthorSelectControl()
        {
            InitializeComponent();
            NodeControlList = new List<NodeControl>();
        }

        public override void SaveParameter()
        {
        }

        public override void InitData()
        {
            //todo 根据sourceID来去查DB
            AuthorList = new List<Author>()
            {
                new Author("孙传芳", "SCF"),
                new Author("吴佩浮", "WPF"),
                new Author("阎锡山", "YXS"),
                new Author("张作霖", "ZZL"),
                new Author("袁世凯", "YSK"),
                new Author("孙中山", "SZS"),
                new Author("蔡锷", "CE")
            };

            Nodes = ParamItem.AuthorNodeItem.Select(item => new SelectedAuthorNode(item.Caption)).ToList();

            AuthorListBox.ItemsSource = AuthorList;
            SelectedPopup.Height = Nodes.Count * 110 + 38;

            for (int i = 0; i < Nodes.Count; i++)
            {
                var control = new NodeControl(Nodes[i].NodeName, i == 0);
                control.RemoveAuthorEventHandler += HandlerRemoveAuthorEvent;
                control.Height = 110;

                NodeControlList.Add(control);
                ChildrenPanel.Children.Add(control);
            }
        }

        #region [Private Method]

        private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in NodeControlList)
            {
                item.Save();
            }
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

        private void ShowPopupBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPopup.IsOpen = true;
        }

        private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPopup.IsOpen = false;
        }

        /// <summary>
        /// 把作者名字从右边的子控件移入到左边的全部列表里
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftMoveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Author tagetAuthor = null;
            foreach (var item in NodeControlList)
            {
                var authorItem = item.RemoveItem();
                if (authorItem == null)
                {
                    continue;
                }
                tagetAuthor = authorItem;
            }
            if (tagetAuthor == null) return;
            var firstOrDefault = AuthorList.FirstOrDefault(p => p == tagetAuthor);
            if (firstOrDefault == null) return;
            firstOrDefault.IsVisible = Visibility.Visible;
            AuthorListBox.SelectedItem = firstOrDefault;
        }

        /// <summary>
        /// 把作者名字从左边移入到右边的子控件中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightMoveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var item = AuthorListBox.SelectedItem as Author;
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