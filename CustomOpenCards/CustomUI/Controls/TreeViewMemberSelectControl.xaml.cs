using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CustomUI.Entitys;

namespace CustomUI.Controls
{
    /// <summary>
    /// Interaction logic for AuthorSelectControl.xaml
    /// </summary>
    public partial class TreeViewMemberSelectControl 
    {
        #region [Field]
        private IEnumerator<TreeNodeExtend> _matchingPeopleEnumerator;
        private List<TreeNodeExtend> _rootNode;

        private ObservableCollection<string> SelectList { get; set; }
        #endregion

        public TreeViewMemberSelectControl()
        {
            InitializeComponent();
            Loaded += AuthorSelectControl_Loaded;
        }

        private void AuthorSelectControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectList = new ObservableCollection<string>();
            var source = new DataSource();
            _rootNode = source.Get(source.GetDataFromDB());
            AuthorsTreeView.ItemsSource = _rootNode;
        }

        private void AddAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            var author = AuthorsTreeView.SelectedItem as TreeNodeExtend;
            if (author == null) return;
            if (author.Children.Count > 0) return;
            if (SelectList.Any(p => p == author.Name)) return;
            SelectList.Add(author.Name);
            AuthorNameList.ItemsSource = SelectList;
            AuthorNameList.SelectedItem = author.Name;
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            if (AuthorNameList.SelectedItem == null) return;

            SelectList.Remove(AuthorNameList.SelectedItem.ToString());
            AuthorNameList.ItemsSource = SelectList;
            AuthorNameList.SelectedIndex = SelectList.Count - 1;
        }

        private void MoveUp_OnClick(object sender, RoutedEventArgs e)
        {
            var index = AuthorNameList.SelectedIndex;
            if (index <= 0) return;
            AuthorNameList.ItemsSource = null;
            var tagertItem = SelectList[index - 1];
            SelectList.RemoveAt(index - 1);
            SelectList.Insert(index, tagertItem);
            AuthorNameList.ItemsSource = SelectList;
            AuthorNameList.SelectedIndex = index - 1;
        }

        private void MoveDown_OnClick(object sender, RoutedEventArgs e)
        {
            var index = AuthorNameList.SelectedIndex;
            if (index < 0)
                return;
            if (index < SelectList.Count - 1)
            {
                AuthorNameList.ItemsSource = null;
                var tagertItem = SelectList[index + 1];
                SelectList.RemoveAt(index + 1);
                SelectList.Insert(index, tagertItem);
                AuthorNameList.ItemsSource = SelectList;
                AuthorNameList.SelectedIndex = index + 1;
            }
        }

        private void BtnOK_OnClick(object sender, RoutedEventArgs e)
        {
            var displayAuthorName = string.Empty;
            foreach (var item in SelectList)
            {
                displayAuthorName = string.IsNullOrEmpty(displayAuthorName) ? item : string.Format("{0},{1}", displayAuthorName, item);
            }
            XDisplayNameTb.Text = displayAuthorName;
            XSelectPopup.IsOpen = false;
            //todo 保存到高级属性
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            XSelectPopup.IsOpen = false;
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            PerformSearch(XSearchText.Text);
        }

        #region Search Logic

        void PerformSearch(string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return;

            if (_matchingPeopleEnumerator == null || !_matchingPeopleEnumerator.MoveNext())
                VerifyMatchingPeopleEnumerator(searchText);

            if (_matchingPeopleEnumerator == null) return;
            var person = _matchingPeopleEnumerator.Current;

            if (person == null)
                return;

            // Ensure that this person is in view.
            if (person.Parent != null)
                person.Parent.IsExpanded = true;

            person.IsSelected = true;
        }

        void VerifyMatchingPeopleEnumerator(string searchtext)
        {
            var matches = FindMatches(searchtext, _rootNode);
            _matchingPeopleEnumerator = matches.GetEnumerator();

            if (!_matchingPeopleEnumerator.MoveNext())
            {
                MessageBox.Show(
                    "No matching names were found.",
                    "Try Again",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
            }
        }

        IEnumerable<TreeNodeExtend> FindMatches(string searchText, IEnumerable<TreeNodeExtend> persons)
        {
            foreach (TreeNodeExtend child in persons)
            {
                if (child.NameContainsText(searchText) && child.Children.Count == 0)
                    yield return child;

                foreach (TreeNodeExtend match in FindMatches(searchText, child.Children))
                    yield return match;
            }
        }

        #endregion // Search Logic

        private void ShowSelect_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            XSelectPopup.IsOpen = XSelectPopup.IsOpen != true;
        }

        private void BtnShowSelect_OnClick(object sender, RoutedEventArgs e)
        {
            XSelectPopup.IsOpen = XSelectPopup.IsOpen != true;
        }

        /// <summary>
        /// 双击加入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                AddAuthor_OnClick(null, null);
            }
        }

        /// <summary>
        /// 双击移出列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Remove_OnClick(null, null);
            }
        }

        /// <summary>
        /// 按下回车键进行搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XSearchText_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch(XSearchText.Text);
            }
        }
    }

    public class DataSource
    {
        public List<TreeNodeExtend> Get(List<TreeNode> authorList)
        {
            var list = new List<TreeNodeExtend>();
            foreach (var item in authorList.Where(p => p.ParentID == "0"))
            {
                var authorExtend = new TreeNodeExtend { ID = item.ID, Name = item.Name, Pid = item.ParentID };
                list.Add(authorExtend);
                GenerateData(authorList, authorExtend);
            }
            return list;
        }

        public List<TreeNode> GetDataFromDB()
        {
            var list = new List<TreeNode>
            {
                new TreeNode("经济研究所", "1001", "0"),
                new TreeNode("所长室", "2001", "1001"),
                new TreeNode("股票研究部", "2002", "1001"),
                new TreeNode("宏观策略部", "2003", "1001"),
                new TreeNode("金融工程部", "2004", "1001"),
                new TreeNode("综合部", "2005", "1001"),
                new TreeNode("所长办公室", "3001", "2001"),
                new TreeNode("副所长", "3002", "2001"),
                new TreeNode("王东", "3003", "2002"),
                new TreeNode("梁铮", "3004", "2002"),
                new TreeNode("房地产研究", "3005", "2002"),
                new TreeNode("银行研究", "3006", "2002"),
                new TreeNode("姜必新", "4001", "3001"),
                new TreeNode("区瑞明", "4002", "3005"),
                new TreeNode("朱红磊", "4003", "3005"),
                new TreeNode("王静", "4004", "3006"),
                new TreeNode("李冠正", "4005", "3006"),
                new TreeNode("机构业务部", "1002", "0"),
                new TreeNode("机构业务总部", "2201", "1002"),
                new TreeNode("机构销售一部", "2202", "1002"),
                new TreeNode("机构销售二部", "2203", "1002"),
                new TreeNode("机构销售三部", "2204", "1002"),
                new TreeNode("机构销售思部", "2205", "1002"),
                new TreeNode("机构业务发展部", "2206", "1002")
            };
            return list;
        }

        private void GenerateData(List<TreeNode> authorList, TreeNodeExtend parent)
        {
            foreach (var item in authorList.Where(p => p.ParentID == parent.ID))
            {
                var authorExtend = new TreeNodeExtend { ID = item.ID, Name = item.Name, Pid = item.ParentID, Parent = parent };
                parent.Children.Add(authorExtend);
                GenerateData(authorList, authorExtend);
            }
        }
    }

}
