using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CarpoolPlanner
{
    public class JsonSerializerFactory
    {
        private static volatile JsonSerializerFactory current = new JsonSerializerFactory();

        public static JsonSerializerFactory Current
        {
            get { return current; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                current = value;
            }
        }

        public virtual JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializer.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            return serializer;
        }
    }
}