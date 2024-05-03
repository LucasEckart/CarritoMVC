using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Web.Services.Description;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult detalleProducto(int idproducto = 0)
        {
            Producto producto = new Producto();
            bool conversion;

            producto = new CN_Producto().listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();

            if (producto != null)
            {
                producto.Base64 = CN_Recursos.convertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen), out conversion);
                producto.Extension = Path.GetExtension(producto.NombreImagen);
            }

            return View(producto);
        }



        public ActionResult listarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categoria().listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult listarMarcaCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().listarMarcaCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult listarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;
            lista = new CN_Producto().listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                IdMarca = p.IdMarca,
                IdCategoria = p.IdCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.convertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo,
            }).Where(p => p.IdCategoria.IdCategoria == (idcategoria == 0 ? p.IdCategoria.IdCategoria : idcategoria) &&
            p.IdMarca.IdMarca == (idmarca == 0 ? p.IdMarca.IdMarca : idmarca) &&
            p.Stock >= 0 && p.Activo == true).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }



        [HttpPost]
        public JsonResult agregarCarrito(int IdProducto)
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool existe = new CN_Carrito().existeCarrito(IdCliente, IdProducto);

            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya se encuentra en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().operacionCarrito(IdCliente, IdProducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadCarrito()
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().cantidadCarrito(IdCliente);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult listarProductoCarrito()
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            List<Carrito> lista = new List<Carrito>();
            bool conversion;

            lista = new CN_Carrito().listarProducto(IdCliente).Select(c => new Carrito()
            {
                Producto = new Producto()
                {
                    IdProducto = c.Producto.IdProducto,
                    Nombre = c.Producto.Nombre,
                    IdMarca = c.Producto.IdMarca,
                    Precio = c.Producto.Precio,
                    RutaImagen = c.Producto.RutaImagen,
                    Base64 = CN_Recursos.convertirBase64(Path.Combine(c.Producto.RutaImagen, c.Producto.NombreImagen), out conversion ),
                    Extension = Path.GetExtension(c.Producto.NombreImagen)
                },
                Cantidad = c.Cantidad
            }).ToList();

            return Json(new { data = lista}, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult operacionCarrito(int IdProducto, bool sumar)
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().operacionCarrito(IdCliente, IdProducto, true, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult eliminarCarrito(int IdProducto)
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().eliminarCarrito(IdCliente, IdProducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult obtenerProvincia()
        {
            List<Provincia> lista = new List<Provincia>();

            lista = new CN_Ubicacion().obtenerProvincia();

            return Json(new { lista = lista}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult obtenerPartido(string idProvincia)
        {
            List<Partido> lista = new List<Partido>();

            lista = new CN_Ubicacion().obtenerPartido(idProvincia);

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult carrito()
        {
            return View();
        }
    }
}

