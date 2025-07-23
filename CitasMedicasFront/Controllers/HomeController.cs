using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CitasMedicasFront.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var rolId = Session["RolId"]?.ToString();

            switch (rolId)
            {
                case "2": // Administrador
                    return View("DashboardAdministrador");
                case "4": // Médico
                    return View("DashboardMedico");
                case "6": // Recepcionista
                    return View("DashboardRecepcionista");
                default:
                    return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult DashboardAdministrador()
        {

            return View();
        }

        public ActionResult DashboardMedico()
        {


            return View();
        }

        public ActionResult DashboardRecepcionista()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}