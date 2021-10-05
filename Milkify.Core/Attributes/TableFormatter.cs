using System;

namespace Milkify.Core.Attributes
{

    public enum FormatType
    {
        Currency,
        YesNo
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TableFormatter: Attribute
    {
        public TableFormatter(FormatType formatter)
        {
            Formatter = formatter;
        }

        public FormatType Formatter { get; set; }
    }
}