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
using System.Globalization;
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
                datos.setearParametro("Contacto", venta.Contacto);
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



        public List<DetalleVenta> listarCompras(int IdCliente)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();
            Conexion datos = new Conexion();
            CultureInfo cultureInfo = new CultureInfo("es-AR");

            try
            {
                string consulta = "select * from fn_listarCompra(@IdCliente)";

                datos.setearConsulta(consulta);
                datos.setearParametro("@IdCliente", IdCliente);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new DetalleVenta()
                    {
                        IdProducto = new Producto()
                        {

                            Nombre = (string)datos.Lector["Nombre"],
                            Precio = decimal.Parse(datos.Lector["Precio"].ToString(), cultureInfo),
                            RutaImagen = datos.Lector["RutaImagen"] != DBNull.Value ? (string)datos.Lector["RutaImagen"] : "",
                            NombreImagen = datos.Lector["NombreImagen"] != DBNull.Value ? (string)datos.Lector["NombreImagen"] : "",
                        },
                        Cantidad = (int)datos.Lector["Cantidad"],
                        Total = decimal.Parse(datos.Lector["Total"].ToString(), cultureInfo),
                        IdTransaccion = (string)datos.Lector["IdTransaccion"]

                }); 
                }
                return lista;
            }
            catch (Exception)
            {
                return new List<DetalleVenta>();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }





    }
}
