using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarpoolPlanner
{
    public static class HttpRequestExtensions
    {
        public static RouteValueDictionary QueryStringRouteValues(this HttpRequestBase request)
        {
            return new RouteValueDictionary(request.QueryString.Cast<string>().ToDictionary(k => k, k => (object)request.QueryString[k]));
        }
    }
}