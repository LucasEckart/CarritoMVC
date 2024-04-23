using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaDatos;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            listaUsuarios = new CN_Usuario().listar();

            return Json( new {data = listaUsuarios },JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult guardarUsuario(Usuario usuario) 
        {
            object resultado;
            string mensaje = string.Empty;
            CN_Usuario negocioUsuario = new CN_Usuario();

            if(usuario.IdUsuario == 0)
            {
                resultado = negocioUsuario.registrarUsuario(usuario, out mensaje);
            }
            else
            {
                resultado = negocioUsuario.editarUsuario(usuario, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarUsuario(int id)   
        {
            CN_Usuario negocioUsuario = new CN_Usuario();
            bool respuesta = false;
            string mensaje = string.Empty;


            respuesta = negocioUsuario.eliminarUsuario(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]
        public JsonResult listaReporte(string fechaInicio, string fechaFin, string IdTransaccion)
        {
            List<Reporte> reporte = new List<Reporte>();
            reporte = new CN_Reporte().ventas(fechaInicio, fechaFin, IdTransaccion);
       
            return Json(new { data = reporte }, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard dashboard = new CN_Reporte().verDashboard();

            return Json(new { resultado = dashboard }, JsonRequestBehavior.AllowGet);
        }




    }
}