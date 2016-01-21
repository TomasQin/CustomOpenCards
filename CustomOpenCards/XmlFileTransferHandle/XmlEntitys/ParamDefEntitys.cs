using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using XmlFileTransferHandle.XmlEnum;

namespace XmlFileTransferHandle.XmlEntitys
{
    [Serializable]
    public class Params
    {
        [XmlAttribute]
        public bool ShowAllParams;

        [XmlElement]
        public List<Param> Param;
    }

    [Serializable]
    public class Param
    {
        [XmlAttribute]
        public string ID;

        [XmlAttribute]
        public ParamControlType Type;

        [XmlAttribute]
        public string Caption;

        [XmlAttribute]
        public string DataSourceID;

        [XmlAttribute]
        public string ParentID;

        [XmlAttribute]
        public string ChildID;

        [XmlElement]
        public List<SelectedMemberNode> SelectedMemberNode;

        [XmlElement]
        public List<DatePickerItem> DatePickerItem;

        //判断参数是否显示captionlabel
        public bool IsCaptionLabelNeeded
        {
            get
            {
                bool isNeeded;
                switch (Type)
                {
                    case ParamControlType.DateTimePickerGroupControl:
                        isNeeded = false;
                        break;
                    default:
                        isNeeded = true;
                        break;
                }
                return isNeeded;

            }
        }

    }

    public class SelectedMemberNode
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string Caption { get; set; }
    }

    public class DatePickerItem
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public bool IsEnable { get; set; }

        [XmlAttribute]
        public DateTimePickerType DateTimePickerType { get; set; }

        [XmlAttribute]
        public int AddValue { get; set; }

        [XmlAttribute]
        public string LinkedParentID { get; set; }

        [XmlAttribute]
        public string Caption { get; set; }
    }

}
