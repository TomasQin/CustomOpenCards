using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CustomUI.Entitys;

namespace CustomUI.Controls.AuthorSelect
{
    /// <summary>
    /// Interaction logic for ResearcherListControl.xaml
    /// </summary>
    public partial class NodeControl
    {
        public EventHandler<AuthorEventArgs> RemoveAuthorEventHandler;

        public NodeControl(string name, bool ischecked)
        {
            InitializeComponent();
            //在popup中显示的缘故，如果放在loaded事件中会触发多次(popup 每显示一次就会触发loaded事件)
            _authorList = new ObservableCollection<Author>();
            SelectedListBox.ItemsSource = _authorList;
            CurrentSelectBtn.Content = name;
            CurrentSelectBtn.IsChecked = ischecked;

        }

        public void AddItem(Author authorItem)
        {
            if (!CurrentSelectBtn.IsChecked.GetValueOrDefault()) return;
            _authorList.Add(authorItem);
            SelectedListBox.SelectedItem = authorItem;
        }

        public Author RemoveItem()
        {
            if (!CurrentSelectBtn.IsChecked.GetValueOrDefault()) return null;
            var item = SelectedListBox.SelectedItem as Author;
            _authorList.Remove(item);
            //删除元素之后，默认选择第一个元素（视以后的业务需求来改动）
            SelectedListBox.SelectedIndex = 0;
            return item;
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

        #region [Private Method]

        private readonly ObservableCollection<Author> _authorList;

        private void SelectedListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = RemoveItem();
            RemoveAuthorEventHandler(sender, new AuthorEventArgs(item));
        }

        #endregion
    }

    public class AuthorEventArgs : EventArgs
    {
        public AuthorEventArgs(Author enitiy)
        {
            AuthorEnitiy = enitiy;
        }
        public Author AuthorEnitiy { get; set; }
    }
}
