using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
    public partial class TreeViewMemberSelectControl : IParameterBasicInterface
    {
        #region [Field]
        private IEnumerator<TreeNodeExtend> _matchingPeopleEnumerator;
        private List<TreeNodeExtend> _rootNode;

        //private ObservableCollection<string> SelectList { get; set; }

        private List<SelectedMemberNode> Nodes { get; set; }
        private List<NodeControl> NodeControlList { get; set; }

        #endregion

        public TreeViewMemberSelectControl()
        {
            InitializeComponent();
            NodeControlList = new List<NodeControl>();
        }

        public Param ParamItem { get; set; }

        public void InitData()
        {
            var source = new DataSource();
            _rootNode = source.Get(source.GetDataFromDB());
            AuthorsTreeView.ItemsSource = _rootNode;

            Nodes = ParamItem.SelectedMemberNode.Select(item => new SelectedMemberNode(item.Caption)).ToList();

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

        public void SaveParameter()
        {

        }

        #region [Private Method]

        private void HandlerRemoveAuthorEvent(object sender, AuthorEventArgs e)
        {
            var tagetAuthor = e.AuthorEnitiy as TreeNodeExtend;
            if (tagetAuthor == null) return;
            tagetAuthor.IsVisibility = Visibility.Visible;
            tagetAuthor.IsSelected = true;
        }

        private void AddAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            var author = AuthorsTreeView.SelectedItem as TreeNodeExtend;
            if (author == null) return;
            if (author.Children.Count > 0) return;

            foreach (var item in NodeControlList)
            {
                item.AddItem(author);
            }
            author.IsVisibility = Visibility.Collapsed;
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in NodeControlList)
            {
                item.RemoveItem();
            }
        }

        private void MoveUp_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in NodeControlList)
            {
                item.ItemMoveUp();
            }
        }

        private void MoveDown_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in NodeControlList)
            {
                item.ItemMoveDown();
            }
        }

        private void BtnOK_OnClick(object sender, RoutedEventArgs e)
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

            XDisplayNameTb.Text = displayName;
            XSelectPopup.IsOpen = false;
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

        private void PerformSearch(string searchText)
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

        private void VerifyMatchingPeopleEnumerator(string searchtext)
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

        #endregion

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
