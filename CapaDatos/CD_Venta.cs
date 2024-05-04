using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Contracts;

using CapaEntidad;
using System.Xml;
using System.Collections;
namespace CapaDatos
{
    public class CD_Venta
    {

        public bool registrar(Venta venta, DataTable detalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearPorcedimiento("sp_registrarVenta");

                datos.setearParametro("IdCliente", venta.IdCliente);
                datos.setearParametro("TotalProducto", venta.TotalProducto);
                datos.setearParametro("MontoTotal", venta.MontoTotal);
                datos.setearParametro("Contacto", venta.contacto);
                datos.setearParametro("IdPartido", venta.IdPartido);
                datos.setearParametro("Telefono", venta.Telefono);
                datos.setearParametro("Direccion", venta.Direccion);
                datos.setearParametro("IdTransaccion", venta.IdTransaccion);
                datos.setearParametro("DetalleVenta", detalleVenta);

                datos.setearParametroScalar("@Resultado", null, SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametroScalar("@Mensaje", null, SqlDbType.VarChar, ParameterDirection.Output, 500);

                datos.ejecutarAccion();

                respuesta = Convert.ToBoolean(datos.getearParametro("@Resultado").Value);
                Mensaje = datos.getearParametro("@Mensaje").Value.ToString();

            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return respuesta;
        }



    }
}
