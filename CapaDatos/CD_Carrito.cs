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
    public class CD_Carrito
    {
        public bool existeCarrito(int IdCliente, int IdProducto)
        {
            Conexion datos = new Conexion();
            bool resultado = true;

            try
            {
                datos.setearPorcedimiento("sp_existeCarrito");

                datos.setearParametro("@IdCliente", IdCliente);
                datos.setearParametro("@IdProducto", IdProducto);;

                datos.setearParametro("@Resultado", SqlDbType.Bit);
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


                datos.setearParametro("@Resultado", SqlDbType.Bit);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar);

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










    }
}
