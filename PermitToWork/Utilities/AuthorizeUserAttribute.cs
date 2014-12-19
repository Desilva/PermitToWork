using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PermitToWork.Utilities
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            if (httpContext.Session["user"] == null)
            {
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Path;
            if (url.Length == 1 || url.ToLower().Contains("home"))
            {
                url = "";
            }
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Login",
                                action = "Index",
                                returnUrl = url,
                                p = filterContext.HttpContext.Request.Params["p"],
                                e = filterContext.HttpContext.Request.Params["e"]
                            })
                        );
        }
    }
}