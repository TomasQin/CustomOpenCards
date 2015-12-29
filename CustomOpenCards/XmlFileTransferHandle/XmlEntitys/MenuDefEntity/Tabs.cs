using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlFileTransferHandle.XmlEntitys.MenuDefEntity
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
        public bool Visible=true;

        [XmlAttribute]
        public string Label;

        [XmlAttribute]
        public string LanguageResourceKey;

        [XmlElement]
        public List<Button> Button;
    }
}
