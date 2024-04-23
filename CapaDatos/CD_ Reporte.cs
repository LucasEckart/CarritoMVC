using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Reporte
    {

        public Dashboard verDashboard()
        {
            Dashboard dashboard = new Dashboard();
            Conexion datos = new Conexion();

            try
            {
                datos.setearPorcedimiento("sp_ReporteDashboard");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    dashboard = new Dashboard()
                    {
                        TotalCliente = (int)datos.Lector["TotalCliente"],
                        TotalVenta = (int)datos.Lector["TotalVenta"],
                        TotalProducto = (int)datos.Lector["TotalProducto"]
                    };
                }
            }
            catch (Exception ex)
            {
                dashboard = new Dashboard();
            }
            finally
            {
                datos.cerrarConexion();
            }

            return dashboard;
        }




        public List<Reporte> ventas(string fechaInicio, string fechaFin, string IdTransaccion)
        {
            List<Reporte> lista = new List<Reporte>();
            Conexion datos = new Conexion();
            CultureInfo cultureInfo = new CultureInfo("es-AR");
            Reporte reporte = new Reporte();

            try
            {
                datos.setearPorcedimiento("sp_reporteVentas");

                datos.setearParametro("@fechaInicio", fechaInicio);
                datos.setearParametro("@fechaFin", fechaFin);
                datos.setearParametro("@idTransaccion", IdTransaccion);

                datos.ejecutarLectura();


                while (datos.Lector.Read())
                {

                    lista.Add(
                     new Reporte()
                     {
                         FechaVenta = (string)datos.Lector["FechaVenta"],
                         Cliente = (string)datos.Lector["Cliente"],
                         Producto = (string)datos.Lector["Producto"],
                         Precio = Convert.ToDecimal(datos.Lector["Precio"], cultureInfo),
                         Cantidad = (int)datos.Lector["Cantidad"],
                         Total = Convert.ToDecimal(datos.Lector["Total"], cultureInfo),
                         IdTransaccion = (string)datos.Lector["IdTransaccion"]

                     });
                }

            }
            catch (Exception ex)
            {
                lista = new List<Reporte>();
            }
            finally
            {
                datos.cerrarConexion();
            }
            return lista;
        }



    }
}
