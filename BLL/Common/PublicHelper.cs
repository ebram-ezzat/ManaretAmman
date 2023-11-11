using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Common
{
    internal class PublicHelper
    {
        public static Dictionary<string, object> GetPropertiesWithPrefix<T>(T obj, string prefix)
        {
            var parameters = new Dictionary<string, object>();

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string parameterName = prefix + property.Name;
                parameters.Add(parameterName, property.GetValue(obj));
            }

            return parameters;
        }

    }
}
