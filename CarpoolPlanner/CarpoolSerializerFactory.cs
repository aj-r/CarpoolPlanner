using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarpoolPlanner
{
    public class CarpoolSerializerFactory : JsonSerializerFactory
    {
        public override JsonSerializer GetSerializer()
        {
            var serializer = base.GetSerializer();
            serializer.Converters.Add(new KeyedCollectionConverter());
            serializer.Converters.Add(new PagedListConverter());
            serializer.ContractResolver = new FilterContractResolver();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return serializer;
        }
    }
}