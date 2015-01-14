using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PagedList;

namespace CarpoolPlanner
{
    public class PagedListConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IPagedList).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var pagedList = (IPagedList)value;
            writer.WriteStartObject();
            writer.WritePropertyName("firstItemOnPage");
            writer.WriteValue(pagedList.FirstItemOnPage);
            writer.WritePropertyName("lastItemOnPage");
            writer.WriteValue(pagedList.LastItemOnPage);
            writer.WritePropertyName("pageCount");
            writer.WriteValue(pagedList.PageCount);
            writer.WritePropertyName("pageNumber");
            writer.WriteValue(pagedList.PageNumber);
            writer.WritePropertyName("pageSize");
            writer.WriteValue(pagedList.PageSize);
            writer.WritePropertyName("totalItemCount");
            writer.WriteValue(pagedList.TotalItemCount);
            writer.WritePropertyName("items");
            writer.WriteStartArray();
            foreach (var item in (IEnumerable)pagedList)
            {
                serializer.Serialize(writer, item);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}