using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlFileTransferHandle.XmlEntitys
{
    [Serializable]
    public class Tabs
    {
        [XmlElement]
        public List<Tab> Tab;
    }

    [Serializable]
    public class Tab
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public bool Visible;

        [XmlAttribute]
        public string Label;

        [XmlAttribute]
        public string LanguageResourceKey;

        [XmlElement]
        public List<Group> Group;
    }

    [Serializable]
    public class Group
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public bool Visible = true;

        [XmlAttribute]
        public string Label;

        [XmlAttribute]
        public string LanguageResourceKey;

        [XmlElement]
        public List<Button> Button;
    }

    [Serializable]
    public class Button
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Label;

        [XmlAttribute]
        public string LanguageResourceKey;

        [XmlAttribute]
        public bool Visible;

        [XmlAttribute]
        public string ImageSourceName;

        [XmlAttribute]
        public string ClickMethod;

        [XmlAttribute]
        public bool IsNeedValidate;

        [XmlAttribute]
        public bool Enabled;

        [XmlAttribute]
        public int LogicType;

        [XmlAttribute]
        public string SuperTip;

        [XmlAttribute]
        public string ClickedParam;

        [XmlAttribute]
        public int Width;

        [XmlAttribute]
        public int Height;

        [XmlAttribute]
        public int RowCount;

        [XmlElement]
        public List<RibbonGalleryItem> RibbonGalleryItem;

        [XmlElement]
        public List<SplitButtonItem> SplitButtonItem;

        [XmlElement]
        public List<DropDownItem> DropDownItem;

    }

    public class RibbonGalleryItem
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Label;

        [XmlAttribute]
        public string LanguageResourceKey;

        [XmlAttribute]
        public bool Visible;

        [XmlAttribute]
        public string ImageSourceName;

        [XmlAttribute]
        public string ClickMethod;

        [XmlAttribute]
        public int Width;

        [XmlAttribute]
        public int Height;
    }

    public class SplitButtonItem
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Label;

        [XmlAttribute]
        public string LanguageResourceKey;

        [XmlAttribute]
        public bool Visible;

        [XmlAttribute]
        public string ImageSourceName;

        [XmlAttribute]
        public string ClickMethod;

        [XmlAttribute]
        public string SuperTip;

    }

    public class DropDownItem
    {
        [XmlAttribute]
        public string Label;
    }
}
