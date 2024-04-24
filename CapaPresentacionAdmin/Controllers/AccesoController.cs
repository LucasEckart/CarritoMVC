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
                if (usuario.Reestablecer)
                {
                    TempData["IdUsuario"] = usuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
    
            }
        }
        [HttpPost]
        public ActionResult cambiarClave(string idUsuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            Usuario usuario = new Usuario();

            usuario = new CN_Usuario().listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            if(usuario.Clave != CN_Recursos.ConvertirSha256(claveActual))
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if(nuevaClave != confirmarClave)
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vclave"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaClave = CN_Recursos.ConvertirSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CN_Usuario().cambiarClave(int.Parse(idUsuario), nuevaClave, out mensaje);

            if(respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idUsuario;
                ViewBag.Error = mensaje;
                return View();
            }
        }
    }
}