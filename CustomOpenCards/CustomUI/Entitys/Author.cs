using System.Windows;
using CustomUI.Common;

namespace CustomUI.Entitys
{
    public class Author : PropertyChangedBase
    {
        public Author(string name, string pyName)
        {
            Name = name;
            PyName = pyName;
            IsVisible = Visibility.Visible;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged(() => Name);
            }
        }

        private string _pyName;

        public string PyName
        {
            get { return _pyName; }
            set
            {
                _pyName = value;
                NotifyPropertyChanged(() => PyName);
            }
        }

        private Visibility _isVisible;

        public Visibility IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                NotifyPropertyChanged(() => IsVisible);
            }
        }

    }
}
