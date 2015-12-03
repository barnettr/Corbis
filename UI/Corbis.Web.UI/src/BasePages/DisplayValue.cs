using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI
{
    public class DisplayValue<T>
    {
        private T _value;
        private string _text;
        private int _id;
        private int? _ordinal;

        public DisplayValue() { }

        public DisplayValue(T value, string text, int id, int? ordinal)
        {
            _value = value;
            _text = text;
            _id = id;
            _ordinal = ordinal;
        }

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int? Ordinal
        {
            get { return _ordinal; }
            set { _ordinal = value; }
        }

        public static int Compare(DisplayValue<T> x, DisplayValue<T> y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            
            if (y == null)
            {
                return 1;
            }

            if (!x.Ordinal.HasValue && y.Ordinal.HasValue)
            {
                return -1;
            }

            if (x.Ordinal.HasValue && !y.Ordinal.HasValue)
            {
                return 1;
            }

            // Compare first by Ordinal, then by Text value if ordinals are equal
            int comparison = 0;
            if (x.Ordinal.HasValue && y.Ordinal.HasValue)
            {
                comparison = x.Ordinal.Value.CompareTo(y.Ordinal.Value);
            }

            if (comparison == 0)
            {
                comparison = string.Compare(x.Text, y.Text, true, Language.CurrentCulture);
            }
            return comparison;
        }

    }
}
