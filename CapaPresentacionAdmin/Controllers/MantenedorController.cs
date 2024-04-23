using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }



        // +++++++++++++++++ Categoria ++++++++++++++++++
        #region Categoria
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> listaCategoria = new List<Categoria>();
            listaCategoria = new CN_Categoria().listar();

            return Json(new { data = listaCategoria }, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult guardarCategoria(Categoria categoria)
        {
            object resultado;
            string mensaje = string.Empty;
            CN_Categoria negocioCategoria    = new CN_Categoria();

            if (categoria.IdCategoria == 0)
            {
                resultado = negocioCategoria.registrarCategoria(categoria, out mensaje);
            }
            else
            {
                resultado = negocioCategoria.editarCategoria(categoria, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult eliminarCategoria(int id)
        {
            CN_Categoria negocioCategoria = new CN_Categoria();
            bool respuesta = false;
            string mensaje = string.Empty;


            respuesta = negocioCategoria.eliminarCategoria(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion


        // +++++++++++++++++ Marca ++++++++++++++++++++++
        #region Marca


        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> listaMarca = new List<Marca>();
            listaMarca = new CN_Marca().listar();

            return Json(new { data = listaMarca }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult guardarMarca(Marca marca)
        {
            object resultado;
            string mensaje = string.Empty;
            CN_Marca negocioMarca = new CN_Marca();

            if (marca.IdMarca == 0)
            {
                resultado = negocioMarca.registrarMarca(marca, out mensaje);
            }
            else
            {
                resultado = negocioMarca.editarMarca(marca, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult eliminarMarca(int id)
        {
            CN_Marca negocioMarca = new CN_Marca();
            bool respuesta = false;
            string mensaje = string.Empty;


            respuesta = negocioMarca.eliminarMarca(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion


        // +++++++++++++++++ Producto ++++++++++++++++++++++
        #region Producto

        [HttpGet]
        public JsonResult ListarProdcuto()
        {
            List<Producto> listaProducto = new List<Producto>();
            listaProducto = new CN_Producto().listar();

            return Json(new { data = listaProducto }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult guardarProducto(string producto, HttpPostedFileBase archivoImg)
        {
            CN_Producto negocioProducto = new CN_Producto();
            Producto objProducto = new Producto();
            

            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagen = true;

            objProducto = JsonConvert.DeserializeObject<Producto>(producto);

            decimal precio;

            if (decimal.TryParse(objProducto.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-AR"), out precio))
            {
                objProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacion_exitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }


            if (objProducto.IdProducto == 0)
            {
                int idProductoGenerado = negocioProducto.registrarProducto(objProducto, out mensaje);
                
                if(idProductoGenerado != 0)
                {
                    objProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            else
            {
                operacionExitosa = negocioProducto.editarProducto(objProducto, out mensaje);
            }

            if(operacionExitosa)
            {
                if(archivoImg != null)
                {
                    string rutaGuardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImg.FileName);
                    string nombreImg = string.Concat(objProducto.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImg.SaveAs(Path.Combine(rutaGuardar, nombreImg));
                    }
                    catch (Exception ex)
                    {
                        string msj = ex.Message;
                        guardarImagen = false;
                    }

                    if (guardarImagen)
                    {
                        objProducto.RutaImagen = rutaGuardar;
                        objProducto.NombreImagen = nombreImg;
                        bool rta = negocioProducto.guardarDatosImg(objProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con la imagen";
                    }
                }
            }

            return Json(new { operacion_exitosa = operacionExitosa, idGenerado = objProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ImgProducto(int id)
        {
            bool conversion;
            Producto producto = new CN_Producto().listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.convertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen),out conversion);

            return Json(new {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(producto.NombreImagen)},JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult eliminarProducto(int id)
        {

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().eliminarProducto(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);



        
        }
        #endregion

    }
}