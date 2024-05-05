using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objCapaDato = new CD_Venta();

        public bool registrar(Venta venta, DataTable detalleVenta, out string Mensaje)
        {
            return objCapaDato.registrar(venta, detalleVenta, out Mensaje);
        }

        public List<DetalleVenta> listarCompras(int IdCliente)
        {
            return objCapaDato.listarCompras(IdCliente);
        }



    }
}
