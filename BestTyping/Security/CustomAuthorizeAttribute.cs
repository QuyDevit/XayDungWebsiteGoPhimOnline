using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BestTyping.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.Session["User"];
            if (user == null || !IsAuthorized(user))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Auth", action = "Index", area = "Admin" }
                    )
                );
            }
        }
        private bool IsAuthorized(object user)
        {
            // Giả sử user là một đối tượng có thuộc tính typeaccount
            dynamic userObj = user;
            return userObj.TypeAccount == 2;
        }
    }
}