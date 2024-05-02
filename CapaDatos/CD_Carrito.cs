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
    public class CD_Carrito
    {
        public bool existeCarrito(int IdCliente, int IdProducto)
        {
            Conexion datos = new Conexion();
            bool resultado = true;

            try
            {
                datos.setearPorcedimiento("sp_existeCarrito");

                datos.setearParametro("IdCliente", IdCliente);
                datos.setearParametro("IdProducto", IdProducto); ;

                datos.setearParametroScalar("@Resultado", null, SqlDbType.Int, ParameterDirection.Output);
                datos.ejecutarAccion();

                resultado = Convert.ToBoolean(datos.getearParametro("@Resultado").Value);
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return resultado;
        }





        public bool operacionCarrito(int IdCliente, int IdProducto, bool sumar, out string Mensaje)
        {
            Conexion datos = new Conexion();

            bool resultado = true;
            Mensaje = string.Empty;

            try
            {
                datos.setearPorcedimiento("sp_operacionCarrito");

                datos.setearParametro("IdCliente", IdCliente);
                datos.setearParametro("IdProducto", IdProducto);
                datos.setearParametro("sumar", sumar);


                datos.setearParametroScalar("@Resultado", null, SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametroScalar("@Mensaje", null, SqlDbType.VarChar, ParameterDirection.Output, 500);

                datos.ejecutarAccion();

                resultado = Convert.ToBoolean(datos.getearParametro("@Resultado").Value);
                Mensaje = datos.getearParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return resultado; ;
        }



        public int cantidadCarrito(int IdCliente)
        {
            int resultado = 0;
            Conexion datos = new Conexion();

            try
            {
                datos.setearConsulta("select count(*) from CARRITO where IdCliente = @IdCliente");
                datos.setearParametro("@IdCliente", IdCliente);
                resultado = Convert.ToInt32(datos.ejecutarAccionScalar());
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return resultado;
        }



        public List<Carrito> listarProducto(int IdCliente)
        {
            List<Carrito> lista = new List<Carrito>();
            Conexion datos = new Conexion();
            CultureInfo cultureInfo = new CultureInfo("es-AR");

            try
            {
                string consulta = "select * from fn_obtenerCarritoCliente(@IdCliente)";

                datos.setearConsulta(consulta);
                datos.setearParametro("@IdCliente", IdCliente);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Carrito()
                    {
                        Producto = new Producto()
                        {

                            IdProducto = (int)datos.Lector["IdProducto"],
                            Nombre = (string)datos.Lector["Nombre"],
                            Precio = decimal.Parse(datos.Lector["Precio"].ToString(), cultureInfo),
                            RutaImagen = datos.Lector["RutaImagen"] != DBNull.Value ? (string)datos.Lector["RutaImagen"] : "",
                            NombreImagen = datos.Lector["NombreImagen"] != DBNull.Value ? (string)datos.Lector["NombreImagen"] : "",
                            IdMarca = new Marca() { Descripcion = (string)datos.Lector["DesMarca"] }
                        },
                        Cantidad = (int)datos.Lector["Cantidad"],
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                return new List<Carrito>();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }



        public bool eliminarCarrito(int IdCliente, int IdProducto)
        {
            Conexion datos = new Conexion();
            bool resultado = true;

            try
            {
                datos.setearPorcedimiento("sp_eliminarCarrito");

                datos.setearParametro("IdCliente", IdCliente);
                datos.setearParametro("IdProducto", IdProducto); ;

                datos.setearParametroScalar("@Resultado", null, SqlDbType.Int, ParameterDirection.Output);
                datos.ejecutarAccion();

                resultado = Convert.ToBoolean(datos.getearParametro("@Resultado").Value);
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return resultado;
        }







    }
}
