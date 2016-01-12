using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CustomUI.Common
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void NotifyPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (PropertyChanged != null)
            {
                var memberExpression = expression.Body as MemberExpression;
                if (memberExpression != null)
                {
                    string propertyName = memberExpression.Member.Name;
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }

            }
        }
    }
}
