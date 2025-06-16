using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CitasMedicasFront.Filters
{
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;

            // Evita bucle infinito si ya está en Login
            if (HttpContext.Current.Session["UserId"] == null && !(controller == "Login" && action == "Login"))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "controller", "Login" },
                        { "action", "Login" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}



