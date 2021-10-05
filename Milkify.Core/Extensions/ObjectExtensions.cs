using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Milkify.Core.Attributes;
using Milkify.Core.Dtos;

namespace Milkify.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetPropertyByName(this object o, string propertyName)
        {
            Type myType = o.GetType();
            PropertyInfo myPropInfo = myType.GetProperty(propertyName);

            TableColumn tableColumn = myPropInfo.GetCustomAttribute<TableColumn>(false);

            object obj = myPropInfo.GetValue(o, null);
            if (tableColumn != null && !string.IsNullOrEmpty(tableColumn.Path))
            {
                obj = obj.GetDeepPropertyValue(tableColumn.Path);
            }

            TableFormatter tableFormatter = myPropInfo.GetCustomAttribute<TableFormatter>(false);
            if (tableFormatter != null)
            {
                switch (tableFormatter.Formatter)
                {
                    case FormatType.Currency:
                    {
                        var value = (decimal) obj;
                        return value.ToString("C");
                    }
                    case FormatType.YesNo:
                    {
                        var value = (bool) obj;
                        return value ? "Yes" : "No";
                    }
                }
            }

            return obj?.ToString();
        }

        public static object GetDeepPropertyValue(this object instance, string path)
        {
            var pp = path.Split('.');
            Type t = instance.GetType();
            foreach (var prop in pp)
            {
                PropertyInfo propInfo = t.GetProperty(prop);
                if (propInfo != null)
                {
                    instance = propInfo.GetValue(instance, null);
                    t = propInfo.PropertyType;
                }
                else throw new ArgumentException("Properties path is not correct");
            }

            return instance;
        }
    }
}