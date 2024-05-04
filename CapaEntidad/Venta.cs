using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {

        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string contacto { get; set; }
        public string IdPartido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string FechaTexto{ get; set; }
        public string IdTransaccion { get; set; }

        public List<DetalleVenta> detalleVentas { get; set; }

    }
}
