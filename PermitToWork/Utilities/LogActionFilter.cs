using PermitToWork.Models.Log;
using PermitToWork.Models.User;
using ProjectLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PermitToWork.Utilities
{
    public class LogActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log("OnActionExecuting", filterContext.RouteData, filterContext.HttpContext);
            filterContext.HttpContext.Response.Filter = new CapturingResponseFilter(filterContext.HttpContext.Response.Filter);
            
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData, filterContext.HttpContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData, filterContext.HttpContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData, filterContext.HttpContext);
        }

        private void Log(string methodName, RouteData routeData, HttpContextBase httpContext)
        {
            var user = httpContext.Session["user"] != null ? httpContext.Session["user"] as UserEntity : new UserEntity();
            var id_permit = 0;
            if (httpContext.Request.Params["id"] != null)
            {
                id_permit = Int32.Parse(httpContext.Request.Params["id"]);
            }
            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();
            var comment = "";
            if (httpContext.Request.Params["comment"] != null)
            {
                comment = httpContext.Request.Params["comment"];
            }
            int? who = null;
            if (httpContext.Request.Params["who"] != null)
            {
                who = Int32.Parse(httpContext.Request.Params["who"]);
            }
            var assessor = "";
            if (httpContext.Request.Params["acc_assessor"] != null)
            {
                assessor = httpContext.Request.Params["acc_assessor"];
            }
            var extension = 0;
            if (httpContext.Request.QueryString["extension"] != null)
            {
                extension = Int32.Parse(httpContext.Request.QueryString["extension"]);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < httpContext.Request.Headers.Count; i++)
                sb.AppendFormat("{0}={1};", httpContext.Request.Headers.Keys[i],
                                            httpContext.Request.Headers[i].ToString());

            var IpAddress = httpContext.Request.UserHostAddress;
            var url = httpContext.Request.RawUrl;
            var RequestHeader = sb.ToString();
            var RequestBody = "";
            using (StreamReader reader = new StreamReader(httpContext.Request.InputStream))
            {
                try
                {
                    httpContext.Request.InputStream.Position = 0;
                    RequestBody = reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    RequestBody = string.Empty;
                    //log errors
                }
                finally
                {
                    httpContext.Request.InputStream.Position = 0;
                }

                if (RequestBody.Contains("password"))
                {
                    RequestBody = RequestBody.Substring(0, RequestBody.IndexOf("password"));
                }
            }
            var message = string.Format("{0} : {1}({4}) accessing /{2}/{3}", DateTime.Now.ToString(), user.alpha_name, controllerName, actionName, user.id);

            if (methodName == "OnResultExecuted")
            {
                var filter = (CapturingResponseFilter)httpContext.Response.Filter;
                filter.id_permit = id_permit;
                filter.extension = extension;
                filter.user = user;
                filter.controllerName = controllerName;
                filter.actionName = actionName;
                filter.comment = comment;
                filter.assessor = assessor;
                filter.who = who;
                //LogEntity log = new LogEntity();

                //log.generateLog(user, id_permit, controllerName, actionName, comment, extension);
            }
            else if (methodName == "OnActionExecuting")
            {
                LogItem log = new LogItem();
                log.actionName = actionName;
                log.controllerName = controllerName;
                log.requestBody = RequestBody;
                log.requestHeader = RequestHeader;
                log.dateTime = DateTime.Now;
                log.fileSemiPath = httpContext.Server.MapPath("~/Log/");
                log.ipAddress = IpAddress;
                log.rawUrl = url;
                log.user_id = user.id.ToString();
                log.username = user.alpha_name;
                log.saveLog();
            }
            
            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}