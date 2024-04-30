using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {

        public int registrarCliente(Cliente Cliente, out string Mensaje)
        {
            Conexion datos = new Conexion();
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                datos.setearPorcedimiento("sp_RegistrarCliente");

                datos.setearParametro("@Nombre", Cliente.Nombre);
                datos.setearParametro("@Apellido", Cliente.Apellido);
                datos.setearParametro("@Correo", Cliente.Correo);
                datos.setearParametro("@Clave", Cliente.Clave);
                           
                datos.setearParametro("@Mensaje", SqlDbType.VarChar);
                datos.setearParametro("@Resultado", SqlDbType.Int);

                datos.ejecutarAccion();

                idAutogenerado = Convert.ToInt32(datos.getearParametro("@Resultado").Value);
                Mensaje = datos.getearParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;
                Mensaje = ex.Message;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return idAutogenerado;
        }



        public List<Cliente> listar()
        {
            List<Cliente> lista = new List<Cliente>();
            Conexion datos = new Conexion();

            try
            {
                datos.setearConsulta("select IdCliente, Nombre, Apellido, Correo, Clave, Reestablecer from Cliente");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Cliente aux = new Cliente();

                    aux.IdCliente = (int)datos.Lector["IdCliente"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Correo = (string)datos.Lector["Correo"];
                    aux.Clave = (string)datos.Lector["Clave"];
                    aux.Reestablecer = (bool)datos.Lector["Reestablecer"];

                    lista.Add(aux);
                }

                return lista;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar Clientes: " + ex.Message);
                return new List<Cliente>();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }



        public bool cambiarClave(int IdCliente, string nuevaClave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearConsulta("update Cliente set clave = @nuevaClave, Reestablecer = 0 where IdCliente = @id");
                datos.setearParametro("@id", IdCliente);
                datos.setearParametro("@nuevaClave", nuevaClave);
                resultado = datos.ejecutarAccion() > 0 ? true : false;

            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return resultado;

        }




        public bool reestablecerClave(int idCliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearConsulta("update Cliente set clave = @clave, Reestablecer = 1 where idCliente = @id");
                datos.setearParametro("@id", idCliente);
                datos.setearParametro("@clave", clave);
                resultado = datos.ejecutarAccion() > 0 ? true : false;
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
            return resultado;
        }






    }
}
