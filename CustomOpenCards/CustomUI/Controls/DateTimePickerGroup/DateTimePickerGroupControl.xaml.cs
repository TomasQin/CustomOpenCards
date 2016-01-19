using System;
using System.Collections.Generic;
using System.Windows.Controls;
using CustomUI.Entitys;
using XmlFileTransferHandle.XmlEntitys;
using XmlFileTransferHandle.XmlEnum;

namespace CustomUI.Controls.DateTimePickerGroup
{
    /// <summary>
    /// Interaction logic for DateTimePickerControl.xaml
    /// </summary>
    public partial class DateTimePickerGroupControl
    {
        #region [Fied]
        private readonly List<DatePicker> _controlList = new List<DatePicker>();
        #endregion

        public DateTimePickerGroupControl()
        {
            InitializeComponent();
        }

        public override void SaveParameter()
        {

        }

        public override void InitData()
        {
            var baseDateTinme = DateTime.Now;
            DatePicker parentDatePicker = null;
            foreach (var item in ParamItem.DatePickerItem)
            {
                var datePicker = new DatePicker { Tag = item, IsEnabled = item.IsEnable };
                if (item.LinkedParentID == "Root")
                {
                    parentDatePicker = datePicker;
                }
                else
                {
                    _controlList.Add(datePicker);
                }
                var entity = new ParameterItemEntity(item.Caption, datePicker, ParamItem);
                var control = new ContentControl
                {
                    Template = FindResource("ParameterTemplate") as ControlTemplate,
                    DataContext = entity
                };
                RootStackPanel.Children.Add(control);
            }

            if (parentDatePicker != null)
            {
                parentDatePicker.SelectedDateChanged += ParametersManagement_SelectedDateChanged;
                parentDatePicker.Text = baseDateTinme.ToShortDateString();
            }
        }

        #region [Private Method]
        private void ParametersManagement_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_controlList.Count <= 0) return;
            var picker = sender as DatePicker;
            if (picker == null) return;
            var baseDateTinme = DateTime.Parse(picker.Text);

            foreach (var datePicker in _controlList)
            {
                var info = datePicker.Tag as DatePickerItem;
                if (info == null) continue;
                switch (info.DateTimePickerType)
                {
                    case DateTimePickerType.QuarterStart:
                        datePicker.Text = GetQuarterStartDate(baseDateTinme).ToShortDateString();
                        break;
                    case DateTimePickerType.QuarterEnd:
                        datePicker.Text = GetQuarterEndDate(baseDateTinme).ToShortDateString();
                        break;
                    case DateTimePickerType.AddDays:
                        datePicker.Text = baseDateTinme.AddDays(info.AddValue).ToShortDateString();
                        break;
                    case DateTimePickerType.AddMonths:
                        datePicker.Text = baseDateTinme.AddMonths(info.AddValue).ToShortDateString();
                        break;
                    case DateTimePickerType.AddYears:
                        datePicker.Text = baseDateTinme.AddYears(info.AddValue).ToShortDateString();
                        break;
                }

            }
        }

        private DateTime GetQuarterStartDate(DateTime dateTime)
        {
            DateTime quarterStart = new DateTime();
            switch (dateTime.Month)
            {
                case 1:
                case 2:
                case 3:
                    quarterStart = DateTime.Parse(String.Format("{0}-12-31", dateTime.AddYears(-1).Year));
                    break;
                case 4:
                case 5:
                case 6:
                    quarterStart = DateTime.Parse(String.Format("{0}-3-31", dateTime.Year));
                    break;
                case 7:
                case 8:
                case 9:
                    quarterStart = DateTime.Parse(String.Format("{0}-6-30", dateTime.Year));
                    break;
                case 10:
                case 11:
                case 12:
                    quarterStart = DateTime.Parse(String.Format("{0}-9-30", dateTime.Year));
                    break;
            }
            return quarterStart;
        }

        private DateTime GetQuarterEndDate(DateTime dateTime)
        {
            DateTime quarterEnd = new DateTime();
            switch (dateTime.Month)
            {
                case 1:
                case 2:
                case 3:
                    quarterEnd = DateTime.Parse(String.Format("{0}-3-31", dateTime.Year));
                    break;
                case 4:
                case 5:
                case 6:
                    quarterEnd = DateTime.Parse(String.Format("{0}-6-30", dateTime.Year));
                    break;
                case 7:
                case 8:
                case 9:
                    quarterEnd = DateTime.Parse(String.Format("{0}-9-30", dateTime.Year));
                    break;
                case 10:
                case 11:
                case 12:
                    quarterEnd = DateTime.Parse(String.Format("{0}-12-31", dateTime.Year));
                    break;
            }
            return quarterEnd;
        }
        #endregion

    }
}
