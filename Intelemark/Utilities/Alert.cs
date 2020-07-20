using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Intelemark
{
    public enum AlertType : Int32
    {
        success,
        error,
        info,
        warning
    }

    public enum PositionClass
    {
        [StringValue("toast-top-right")]
        TopRight,
        [StringValue("toast-bottom-right")]
        BottomRight,
        [StringValue("toast-top-full-width")]
        TopFullWidth,
        [StringValue("toast-bottom-full-width")]
        TopBottomWidth
    }

    public class Alert
    {
        public AlertType AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
        public PositionClass Position { get; set; }
        public string ToastrClass { get => GetStringValue(Position); }

        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());

            StringValue[] attrs =
               fi.GetCustomAttributes(typeof(StringValue),
                                       false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

    }

    public class StringValue : System.Attribute
    {
        private readonly string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }



}