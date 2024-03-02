using RoguelikeBase.Items.Processors.Consumables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Utils
{
    internal static class ReflectionUtils
    {
        public static T CreateInstanceFromType<T,U>(Type type, string suffix = "")
        {
            string processorName = string.Concat(typeof(U).Namespace, '.', type.Name, suffix);
            Type processorType = Type.GetType(processorName);

            return processorType != null ? (T)Activator.CreateInstance(processorType) : default(T);
        }

        public static T CreateInstanceFromString<T, U>(string type, string suffix = "")
        {
            return (T)CreateInstanceFromString(typeof(U).Namespace, type, suffix);
        }

        public static object CreateInstanceFromString(string nameSpace, string type, string suffix = "")
        {
            string processorName = string.Concat(nameSpace, '.', type, suffix);
            Type processorType = Type.GetType(processorName);

            return processorType != null ? Activator.CreateInstance(processorType) : null;
        }
    }
}
