using System;
using System.Collections.Generic;
using System.Windows;

namespace CustomUI.Entitys
{
    public class TreeNode
    {
        public TreeNode(string name, string id, string pid)
        {
            Name = name;
            ID = id;
            ParentID = pid;
        }

        public string Name { get; set; }

        public string ID { get; set; }

        public string ParentID { get; set; }
    }

    public class TreeNodeExtend : SelectedMemberNodeItem
    {
        public TreeNodeExtend()
        {
            Children = new List<TreeNodeExtend>();
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _pid;
        public string Pid
        {
            get { return _pid; }
            set { _pid = value; }
        }

        private List<TreeNodeExtend> _children;

        public List<TreeNodeExtend> Children
        {
            get { return _children; }
            set { _children = value; }

        }

        private TreeNodeExtend _parent;
        public TreeNodeExtend Parent
        {
            get { return _parent; }

            set { _parent = value; }
        }

        private Visibility _isVisibility = Visibility.Visible;
        public Visibility IsVisibility
        {
            get { return _isVisibility; }
            set
            {
                if (value != _isVisibility)
                {
                    _isVisibility = value;
                    NotifyPropertyChanged(() => IsVisibility);
                }
            }
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    NotifyPropertyChanged(() => IsExpanded);
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    NotifyPropertyChanged(() => IsSelected);
                }
            }
        }

        public bool NameContainsText(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(Name))
                return false;

            return Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }
    }
}
