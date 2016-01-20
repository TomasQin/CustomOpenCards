using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CustomUI.AttachProperty
{
    public class LayoutExtension
    {
        /// <summary>
        /// 这个方法用来获取控件
        /// </summary>
        /// <typeparam name="T">指定控件类型，例如TextBlock</typeparam>
        /// <param name="name">指定控件名称</param>
        /// <returns></returns>
        public static T GetControl<T>(string name)
            where T : class
        {
            return _controls.FirstOrDefault(t => t.Key == name).Value as T;
        }


        private static Dictionary<string, DependencyObject> _controls;
        static LayoutExtension()
        {
            _controls = new Dictionary<string, DependencyObject>();
        }

        public static string GetName(DependencyObject obj)
        {
            return (string)obj.GetValue(NameProperty);
        }

        public static void SetName(DependencyObject obj, string value)
        {
            obj.SetValue(NameProperty, value);

        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached("Name", typeof(string), typeof(LayoutExtension), new PropertyMetadata(string.Empty, (d, e) =>
            {
                if (e.NewValue != null)
                {
                    var name = e.NewValue.ToString();

                    if (!_controls.ContainsKey(name))
                        _controls.Add(name, d);
                }
            }));

    }
}
