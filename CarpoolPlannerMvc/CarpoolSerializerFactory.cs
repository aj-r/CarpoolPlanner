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
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ContractResolver = new FilterContractResolver();
            return serializer;
        }
    }
}