using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Registrar(Cliente cliente)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(cliente.Nombre) ? "" : cliente.Nombre;
            ViewData["Apellidos"] = string.IsNullOrEmpty(cliente.Apellido) ? "" : cliente.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(cliente.Correo) ? "" : cliente.Correo;

            if(cliente.Clave != cliente.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CN_Cliente().registrarCliente(cliente, out mensaje);
            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente cliente = null;

            cliente = new CN_Cliente().listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();
            if (cliente == null)
            {
                ViewBag.Error = "Correo o Contraseña incorrecto";
                return View();
            }
            else
            {
                if (cliente.Reestablecer)
                {
                    TempData["IdCliente"] = cliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(cliente.Correo, false);

                    Session["Cliente"] = cliente;

                    ViewBag.Error = null;

                    return RedirectToAction("Index", "Tienda");
                }
            }
        }
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente cliente = new Cliente();
            cliente = new CN_Cliente().listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro un cliente relacionado con ese correo.";
                return View();
            }

            string mensaje = string.Empty;
            bool reespuesta = new CN_Cliente().reestablecerClave(cliente.IdCliente, correo, out mensaje);

            if (reespuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }


        [HttpPost]
        public ActionResult CambiarClave(string IdCliente, string claveActual, string nuevaClave, string confirmarClave)
        {
            Cliente cliente= new Cliente();

            cliente = new CN_Cliente().listar().Where(u => u.IdCliente == int.Parse(IdCliente)).FirstOrDefault();

            if (cliente.Clave != CN_Recursos.ConvertirSha256(claveActual))
            {
                TempData["IdCliente"] = IdCliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if (nuevaClave != confirmarClave)
            {
                TempData["IdCliente"] = IdCliente;
                ViewData["vclave"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaClave = CN_Recursos.ConvertirSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().cambiarClave(int.Parse(IdCliente), nuevaClave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = IdCliente;
                ViewBag.Error = mensaje;
                return View();
            }
        }


        public ActionResult CerrarSession(string correo)
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
            
        }
    }
}