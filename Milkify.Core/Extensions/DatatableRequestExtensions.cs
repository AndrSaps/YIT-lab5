using System;
using System.Collections.Generic;
using System.Reflection;
using Milkify.Core.Datatable.DatatableRequests;

namespace Milkify.Core.Extensions
{
    public static class DatatableRequestExtensions
    {
        public static string GetQueryString(this DefaultDataTablesRequest request)
        {
            if (request == null)
            {
                return "";
            }
            List<string> result = new List<string>();
            PropertyInfo[] props = request.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(request, null);
                if (value != null)
                {
                    result.Add(prop.Name + "=" + value);
                }
            }

            return string.Join('&', result);

        }

        public static string GetQueryString(this DefaultDataTablesRequest request, string overrideMember, string overrideValue)
        {
            if (request == null)
            {
                return "";
            }
            List<string> result = new List<string>();
            PropertyInfo[] props = request.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name != overrideMember)
                {
                    var value = prop.GetValue(request, null);
                    if (value != null)
                    {
                        result.Add(prop.Name + "=" + value);
                    }
 
                }
                else
                {
                    result.Add(overrideMember + "=" + overrideValue);
                }
              
            }

            return string.Join('&', result);
        }
    }
}