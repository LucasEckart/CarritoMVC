using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {

        private CD_Productos objCapaDato = new CD_Productos();
        public List<Producto> listar()
        {
            return objCapaDato.listar();
        }


        public int registrarProducto(Producto producto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrWhiteSpace(producto.Nombre))
            {
                Mensaje = "El nombre del producto no puede ser vacío";
            }

            else if (string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Descripcion))
            {
                Mensaje = "La descripcion del producto no puede ser vacío";
            }

            else if (producto.IdMarca.IdMarca == 0)
            {
                Mensaje = "Debes seleccionar una marca";
            }
            else if (producto.IdCategoria.IdCategoria == 0)
            {
                Mensaje = "Debes seleccionar una Categoria";
            }
            else if (producto.Precio == 0)
            {
                Mensaje = "Debes ingresar el precio del producto";
            }
            else if (producto.Stock == 0)
            {
                Mensaje = "Debes seleccionar el stock del producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.registrarProducto(producto, out Mensaje);
            }
            else
            {
                return 0;
            }

        }


        public bool editarProducto(Producto producto, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrWhiteSpace(producto.Nombre))
            {
                Mensaje = "El nombre del producto no puede ser vacío";
            }

            else if (string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Descripcion))
            {
                Mensaje = "La descripcion del producto no puede ser vacío";
            }

            else if (producto.IdMarca.IdMarca == 0)
            {
                Mensaje = "Debes seleccionar una marca";
            }
            else if (producto.IdCategoria.IdCategoria == 0)
            {
                Mensaje = "Debes seleccionar una Categoria";
            }
            else if (producto.IdMarca.IdMarca == 0)
            {
                Mensaje = "Debes seleccionar una marca";
            }
            else if (producto.Precio == 0)
            {
                Mensaje = "Debes ingresar el precio del producto";
            }
            else if (producto.Stock == 0)
            {
                Mensaje = "Debes seleccionar el stock del producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.editarProducto(producto, out Mensaje);
            }
            else
            {
                return false;
            }
        }


        public bool guardarDatosImg(Producto producto, out string mensaje)
        {
            return objCapaDato.guardarDatosImg(producto, out mensaje);
        }


        public bool eliminarProducto(int id, out string Mensaje)
        {
            return objCapaDato.eliminarProducto(id, out Mensaje);
        }



    }
}
