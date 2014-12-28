using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
            serializer.Converters.Add(new IsoDateTimeConverter());
            serializer.Converters.Add(new KeyedCollectionConverter());
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return serializer;
        }
    }
}