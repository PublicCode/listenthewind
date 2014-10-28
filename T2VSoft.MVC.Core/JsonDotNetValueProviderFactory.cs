using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;


using System.Web.Mvc;
using System.IO;
using System.Dynamic;
using System.Globalization;
using System.Collections;

namespace T2VSoft.MVC.Core
{
    public class JsonDotNetValueProviderFactory : ValueProviderFactory
    {
        private static void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
        {
            IDictionary<string, object> dictionary = value as IDictionary<string, object>;
            if (dictionary != null)
            {
                foreach (KeyValuePair<string, object> pair in dictionary)
                {
                    AddToBackingStore(backingStore, MakePropertyKey(prefix, pair.Key), pair.Value);
                }
            }
            else
            {
                IList list = value as IList;
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        AddToBackingStore(backingStore, MakeArrayKey(prefix, i), list[i]);
                    }
                }
                else
                {
                    backingStore[prefix] = value;
                }
            }
        }

        private static object GetDeserializedObject(ControllerContext controllerContext)
        {
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            string str = new StreamReader(controllerContext.HttpContext.Request.InputStream).ReadToEnd();
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return serializer.DeserializeObject(str);
        }

        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            object deserializedObject = GetDeserializedObject(controllerContext);
            if (deserializedObject == null)
            {
                return null;
            }
            Dictionary<string, object> backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            AddToBackingStore(backingStore, string.Empty, deserializedObject);
            return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
        }

        private static string MakeArrayKey(string prefix, int index)
        {
            return (prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]");
        }

        private static string MakePropertyKey(string prefix, string propertyName)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                return (prefix + "." + propertyName);
            }
            return propertyName;
        }
    }
}
