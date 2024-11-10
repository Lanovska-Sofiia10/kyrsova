using System;
using System.Collections.Generic;
using System.Linq;

namespace Kyrsova.Contractors
{
    public class Form
    {
        public string UniqueCode { get; }

        public int OrderId { get; }

        public int Step { get; }

        public bool IsFinal { get; }

        public IReadOnlyList<Field> Fields { get; }

        public Form(string uniqueCode, int orderId, int step, bool isFinal, IEnumerable<Field> fields)
        {
            if (string.IsNullOrWhiteSpace(uniqueCode))
                throw new ArgumentException("Unique code cannot be null or whitespace.", nameof(uniqueCode));

            if (step < 0)
                throw new ArgumentOutOfRangeException(nameof(step), "Step must be non-negative.");

            if (fields == null)
                throw new ArgumentNullException(nameof(fields), "Fields cannot be null.");

            UniqueCode = uniqueCode;
            OrderId = orderId;
            Step = step;
            IsFinal = isFinal;
            Fields = fields.ToList().AsReadOnly();
        }
    }
}

