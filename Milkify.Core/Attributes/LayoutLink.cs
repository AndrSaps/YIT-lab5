using System;

namespace Milkify.Core.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class LayoutLink: Attribute
    {

        /// <summary>
        /// Link name (label) in ui
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Coma separated roles string
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Controller link action
        /// </summary>
        public string Action { get; set; }

        public int Order { get; set; }

        public string NavIcon { get; set; }

        public LayoutLink(string name = null, string roles = "", string action = "Index", int order = 1, string navIcon = "")
        {
            Name = name;
            Roles = roles;
            Action = action;
            Order = order;
            NavIcon = navIcon;
        }
    }
}