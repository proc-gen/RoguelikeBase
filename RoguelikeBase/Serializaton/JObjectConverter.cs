using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RoguelikeBase.Serializaton
{
    internal class JObjectConverter
    {
        public JObjectConverter() { }

        public object Convert(Type t, JObject data)
        {
            MethodInfo method = GetType().GetMethod("Convert")
                             .MakeGenericMethod([t]);
            return method.Invoke(this, [data]);
        }

        public T Convert<T>(JObject data)
            where T : new()
        {
            object retVal = Activator.CreateInstance<T>();

            var DeserializedType = typeof(T);

            PropertyInfo[] properties = DeserializedType.GetProperties();
            FieldInfo[] fields = DeserializedType.GetFields();

            TypedReference tRef = __makeref(retVal);

            foreach (var pi in properties)
            {
                if (data[pi.Name] != null && pi.Name != "IsChanged")
                {
                    pi.SetValue(retVal, System.Convert.ChangeType(data[pi.Name], pi.PropertyType));
                }
            }

            foreach (var fi in fields)
            {
                if (data[fi.Name] != null)
                {
                    fi.SetValueDirect(tRef, System.Convert.ChangeType(data[fi.Name], fi.FieldType));
                }
            }

            return (T)retVal;
        }
    }
}
