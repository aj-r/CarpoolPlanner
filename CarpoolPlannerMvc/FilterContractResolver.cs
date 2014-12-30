﻿using System;
using System.Reflection;
using CarpoolPlanner.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarpoolPlanner
{
    /// <summary>
    /// A JSON contract resolver that filters out sensitive data that should not be sent to the client.
    /// </summary>
    public class FilterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(User))
            {
                switch (property.PropertyName)
                {
                    case "LoginName":
                    case "IsAdmin":
                    case "Status":
                        property.ShouldSerialize = instance =>
                        {
                            var user = (User)instance;
                            return AppUtils.IsUserAdmin() || (AppUtils.CurrentUser != null && AppUtils.CurrentUser.Id == user.Id);
                        };
                        break;
                    case "Email":
                        property.ShouldSerialize = instance =>
                        {
                            var user = (User)instance;
                            return user.EmailVisible || AppUtils.IsUserAdmin() ||
                                (AppUtils.CurrentUser != null && AppUtils.CurrentUser.Id == user.Id);
                        };
                        break;
                    case "Phone":
                        property.ShouldSerialize = instance =>
                        {
                            var user = (User)instance;
                            return user.PhoneVisible || AppUtils.IsUserAdmin() ||
                                (AppUtils.CurrentUser != null && AppUtils.CurrentUser.Id == user.Id);
                        };
                        break;
                }
            }

            return property;
        }
    }
}