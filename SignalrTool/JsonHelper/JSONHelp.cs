using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SignalrTool.JsonHelper
{
    public static class JSONHelp
    {

        public static string SerializeObject(object obj,string dateformat = "",string fields = "")
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            MyCustomResolver Resolver = null;
            if (!string.IsNullOrEmpty(dateformat))
            {
                settings.DateFormatString = dateformat;
            }

            if (!string.IsNullOrEmpty(fields))
            {
                Resolver = new MyCustomResolver();
                Resolver.Fields = fields;
                settings.ContractResolver = Resolver;
            }
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj,settings);
            settings = null;
            Resolver = null;
            return result;
        }

        public static T DeserializeObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }

    public class MyCustomResolver : DefaultContractResolver
    {
        private string[] fieldList = null;
        public string Fields
        {
            set 
            {
                fieldList = value.ToUpper().Split(',');
            }
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> fields = base.CreateProperties(type, memberSerialization);
            List<JsonProperty> temps = new List<JsonProperty>();
            JsonProperty temp = null;
            if (fieldList.Length > 0)
            {
                for (int j = 0; j < fieldList.Length; j++)
                {
                    temp = fields.Where(i => i.PropertyName.ToUpper() == fieldList[j]).SingleOrDefault();
                    if (temp != null)
                    {
                        temps.Add(temp);
                    }
                }
                
            }

            if (temps.Count > 0)
            {
                return temps;
            }

            return fields;

        }
    }
}