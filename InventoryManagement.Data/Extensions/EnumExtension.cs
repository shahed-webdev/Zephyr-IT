using System;
using System.ComponentModel;
using System.Reflection;

namespace InventoryManagement.Data
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }
    }
}