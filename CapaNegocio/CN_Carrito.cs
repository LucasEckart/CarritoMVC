using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito objCapaDato = new CD_Carrito();

        public bool existeCarrito(int IdCliente, int IdProducto)
        {
            return objCapaDato.existeCarrito(IdCliente, IdProducto);
        }


        public bool operacionCarrito(int IdCliente, int IdProducto, bool sumar, out string Mensaje)
        {
            return objCapaDato.operacionCarrito(IdCliente, IdProducto, sumar, out Mensaje);
        }


        public int cantidadCarrito(int IdCliente)
        {
            return objCapaDato.cantidadCarrito(IdCliente);
        }



    }
}
