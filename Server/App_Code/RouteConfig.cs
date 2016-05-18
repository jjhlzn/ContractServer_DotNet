using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace Server
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
          //  routes.EnableFriendlyUrls();
            routes.MapPageRoute("order", "order/{action}.json", "~/order.aspx");
            routes.MapPageRoute("approval", "approval/{action}.json", "~/approval.aspx");
            routes.MapPageRoute("product", "product/{action}.json", "~/product.aspx");
            routes.MapPageRoute("login", "login/{action}.json", "~/login.aspx");

        
        }
    }
}