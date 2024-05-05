using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Web.Services.Description;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Collections;
using CapaEntidad.Paypal;
using System.EnterpriseServices;
using CapaPresentacionTienda.Filter;

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
                    Base64 = CN_Recursos.convertirBase64(Path.Combine(c.Producto.RutaImagen, c.Producto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(c.Producto.NombreImagen)
                },
                Cantidad = c.Cantidad
            }).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult operacionCarrito(int IdProducto, bool sumar)
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().operacionCarrito(IdCliente, IdProducto, sumar, out mensaje);

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

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult obtenerPartido(string idProvincia)
        {
            List<Partido> lista = new List<Partido>();

            lista = new CN_Ubicacion().obtenerPartido(idProvincia);

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }


        [ValidarSession]
        [Authorize]
        public ActionResult carrito()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> procesarPago(List<Carrito> listaCarrito, Venta venta)
        {
            decimal total = 0;
            DataTable detalleVenta = new DataTable();
            detalleVenta.Locale = new CultureInfo("es-AR");
            detalleVenta.Columns.Add("IdProducto", typeof(string));
            detalleVenta.Columns.Add("Cantidad", typeof(int));
            detalleVenta.Columns.Add("Total", typeof(decimal));

            List<Item> listaItem = new List<Item>();



            foreach (Carrito carrito in listaCarrito)
            {
                decimal subTotal = Convert.ToDecimal(carrito.Cantidad.ToString()) * carrito.Producto.Precio;

                total += subTotal;

                listaItem.Add(new Item
                {
                    name= carrito.Producto.Nombre,
                    quantity= carrito.Cantidad.ToString(),
                    unit_amount= new UnitAmount() { 
                        currency_code = "USD",
                        value = carrito.Producto.Precio.ToString("G", new CultureInfo("es-AR"))
                    }
                });

                detalleVenta.Rows.Add(new object[]
                {
                    carrito.Producto.IdProducto,
                    carrito.Cantidad,
                    subTotal
                });
            }


            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", new CultureInfo("es-AR")),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal
                        {
                            currency_code = "USD",
                            value = total.ToString("G", new CultureInfo("es-AR"))
                        }
                    }
                },
                description = "Compra  de articulo de mi tienda",
                items = listaItem
            };


            Checkout_Order checkout_Order = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit> { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "MiTienda.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44300/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44300/Tienda/Carrito"
                }
            };


            venta.MontoTotal = total;
            venta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = venta;
            TempData["detalleVenta"] = detalleVenta;

            CN_Paypal paypal = new CN_Paypal();
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();
            response_paypal = await paypal.crearSolisitud(checkout_Order);


            return Json(response_paypal, JsonRequestBehavior.AllowGet);

        }

        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];
            CN_Paypal paypal = new CN_Paypal();
            Response_Paypal<Response_Capture> response_Paypal = new Response_Paypal<Response_Capture>();
            response_Paypal = await paypal.aprobarPago(token);

            ViewData["Status"] = response_Paypal.Status;

            if (response_Paypal.Status)
            {
                Venta venta = (Venta)TempData["Venta"];
                DataTable detalleVenta = (DataTable)TempData["detalleVenta"];
                venta.IdTransaccion = response_Paypal.Response.purchase_units[0].payments.captures[0].id;
                string mensaje = string.Empty;

                bool respuesta = new CN_Venta().registrar(venta, detalleVenta, out mensaje);

                ViewData["IdTransaccion"] = venta.IdTransaccion;    

            }

            return View();
        }



        [ValidarSession]
        [Authorize]
        public ActionResult misCompras()
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            List<DetalleVenta> lista = new List<DetalleVenta>();
            bool conversion;

            lista = new CN_Venta().listarCompras(IdCliente).Select(c => new DetalleVenta()
            {
                IdProducto = new Producto()
                {
                    Nombre = c.IdProducto.Nombre,
                    Precio = c.IdProducto.Precio,
                    Base64 = CN_Recursos.convertirBase64(Path.Combine(c.IdProducto.RutaImagen, c.IdProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(c.IdProducto.NombreImagen)
                },
                Cantidad = c.Cantidad,
                Total = c.Total,
                IdTransaccion = c.IdTransaccion
            }).ToList();

            return View(lista);

        }

    }
}

