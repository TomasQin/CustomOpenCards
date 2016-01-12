﻿using System;
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

        [XmlElement]
        public List<AuthorNodeItem> AuthorNodeItem;

        [XmlElement]
        public List<DatePickerItem> DatePickerItem;

    }


    public class AuthorNodeItem
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
