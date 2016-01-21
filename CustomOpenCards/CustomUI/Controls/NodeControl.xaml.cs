using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CustomUI.Entitys;

namespace CustomUI.Controls
{
    /// <summary>
    /// Interaction logic for ResearcherListControl.xaml
    /// </summary>
    public partial class NodeControl
    {
        #region[Feild]
        private readonly ObservableCollection<SelectedMemberNodeItem> _authorList;
        #endregion

        public event EventHandler<AuthorEventArgs> RemoveAuthorEventHandler;

        public NodeControl(string name, bool ischecked,string groupName)
        {
            InitializeComponent();
            //在popup中显示的缘故，如果放在loaded事件中会触发多次(popup 每显示一次就会触发loaded事件)
            _authorList = new ObservableCollection<SelectedMemberNodeItem>();
            SelectedListBox.ItemsSource = _authorList;
            CurrentSelectBtn.Content = name;
            CurrentSelectBtn.GroupName = groupName;
            if (ischecked)
            {
                CurrentSelectBtn.IsChecked = true;
            }
        }

        public void AddItem(SelectedMemberNodeItem authorItem)
        {
            if (!CurrentSelectBtn.IsChecked.GetValueOrDefault()) return;
            _authorList.Add(authorItem);
            SelectedListBox.SelectedItem = authorItem;
        }

        public void RemoveItem()
        {
            if (!CurrentSelectBtn.IsChecked.GetValueOrDefault()) return ;
            var item = SelectedListBox.SelectedItem as SelectedMemberNodeItem;
            _authorList.Remove(item);
            //删除元素之后，默认选择第一个元素（视以后的业务需求来改动）
            SelectedListBox.SelectedIndex = 0;
            OnRemoveAuthorEventHandler(new AuthorEventArgs(item));
        }

        public void ItemMoveUp()
        {
            if (!CurrentSelectBtn.IsChecked.GetValueOrDefault()) return;

            var index = SelectedListBox.SelectedIndex;
            if (index <= 0) return;
            SelectedListBox.ItemsSource = null;
            var tagertItem = _authorList[index - 1];
            _authorList.RemoveAt(index - 1);
            _authorList.Insert(index, tagertItem);
            SelectedListBox.ItemsSource = _authorList;
            SelectedListBox.SelectedIndex = index - 1;
        }

        public void ItemMoveDown()
        {
            if (!CurrentSelectBtn.IsChecked.GetValueOrDefault()) return;

            var index = SelectedListBox.SelectedIndex;
            if (index < 0)
                return;
            if (index < _authorList.Count - 1)
            {
                SelectedListBox.ItemsSource = null;
                var tagertItem = _authorList[index + 1];
                _authorList.RemoveAt(index + 1);
                _authorList.Insert(index, tagertItem);
                SelectedListBox.ItemsSource = _authorList;
                SelectedListBox.SelectedIndex = index + 1;
            }
        }

        public void Save()
        {

        }

        public List<SelectedMemberNodeItem> GetData()
        {
            return _authorList.ToList();
        }

        #region [Private Method]

        private void SelectedListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RemoveItem();
        }

        private void OnRemoveAuthorEventHandler(AuthorEventArgs e)
        {
            var handler = RemoveAuthorEventHandler;
            if (handler != null)
                handler(this, e);
        }
        #endregion
    }

    public class AuthorEventArgs : EventArgs
    {
        public AuthorEventArgs(SelectedMemberNodeItem enitiy)
        {
            AuthorEnitiy = enitiy;
        }
        public SelectedMemberNodeItem AuthorEnitiy { get; set; }
    }
}
