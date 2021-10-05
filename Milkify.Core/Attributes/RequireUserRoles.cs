using System;
using System.Collections;
using System.Collections.Generic;

namespace Milkify.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequireUserRoles : Attribute
    {
        public RequireUserRoles(string userRoles)
        {
            UserRoles = userRoles;
        }
        
        public string UserRoles { get; set; }
    }
}