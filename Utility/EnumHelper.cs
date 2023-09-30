using System;
using System.ComponentModel;
using System.Reflection;

namespace Utility
{
    public class EnumHelper
    {
        public static string GetEnumDescription(Type type, string enumName)
        {
            try
            {
                FieldInfo field = type.GetField(enumName);
                if (field == null) return "";

                DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute == null) return "";

                return attribute.Description;
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public static object GetEnumByDescription(Type type, string description)
        {
            object result = null;

            try
            {
                FieldInfo[] fieldInfos = type.GetFields();

                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    DescriptionAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute?.Description == description)
                    {
                        result = Enum.Parse(type, fieldInfo.GetValue(null).ToString());
                        return result;
                    }
                }
            }
            catch(Exception ex)
            { }

            return result;
        }
    }
}
