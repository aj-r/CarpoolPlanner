using System;
using System.Globalization;
using System.Reflection;
using CarpoolPlanner.Controllers;
using CarpoolPlanner.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarpoolPlanner
{
    /// <summary>
    /// A JSON contract resolver that filters out sensitive data that should not be sent to the client.
    /// </summary>
    public class FilterContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(User))
            {
                switch (property.PropertyName)
                {
                    case "isAdmin":
                    case "status":
                    case "emailNotify":
                    case "emailVisible":
                    case "phoneNotify":
                    case "phoneVisible":
                        property.ShouldSerialize = instance =>
                        {
                            var user = (User)instance;
                            return UserController.CanCurrentUserModify(user);
                        };
                        break;
                    case "email":
                        property.ShouldSerialize = instance =>
                        {
                            var user = (User)instance;
                            return user.EmailVisible || UserController.CanCurrentUserModify(user);
                        };
                        break;
                    case "phone":
                        property.ShouldSerialize = instance =>
                        {
                            var user = (User)instance;
                            return user.PhoneVisible || UserController.CanCurrentUserModify(user);
                        };
                        break;
                }
            }

            return property;
        }
    }
}