using System;
using System.Collections.Generic;

namespace Kyrsova.Contractors
{
    public abstract class Field
    {
        public string Label { get; }
        public string Name { get; }
        public string Value { get; set; }

        protected Field(string label, string name, string value = null)
        {
            Label = label;
            Name = name;
            Value = value;
        }
    }

    public class SelectionField : Field
    {
        public IReadOnlyDictionary<string, string> Items { get; }

        public SelectionField(string label, string name, IReadOnlyDictionary<string, string> items, string value = null)
            : base(label, name, value)
        {
            Items = items;
        }
    }
}

