using System;

namespace Milkify.Core.Attributes
{
   
    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumn: Attribute
    {
        public string Label { get; set; }
        public int Order { get; set; }
        public string Path { get; set; }
   
        
        public TableColumn(string label, string path = null, int order = 0)
        {
            this.Label = label;
            this.Order = order;
            this.Path = path;
        }
    }
}