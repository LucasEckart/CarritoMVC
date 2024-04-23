using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult cambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario usuario = new Usuario();
            usuario = new CN_Usuario().listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if(usuario == null) 
            {
                ViewBag.Error = "Correo o contraseña incorrecto";
                return View();
            }
            else
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
    
            }
            return View();
        }
    }
}