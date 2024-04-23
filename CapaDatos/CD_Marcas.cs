using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Marcas
    {
        public List<Marca> listar()
        {
            List<Marca> lista = new List<Marca>();
            Conexion datos = new Conexion();

            try
            {
                datos.setearConsulta("select IdMarca, Descripcion, Activo from MARCA");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();

                    aux.IdMarca = (int)datos.Lector["IdMarca"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Activo = (bool)datos.Lector["Activo"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar categorias: " + ex.Message);
                return new List<Marca>();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public int registrarMarca(Marca marca, out string Mensaje)
        {
            Conexion datos = new Conexion();
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                datos.setearPorcedimiento("sp_registrarMarca");

                datos.setearParametro("@Descripcion", marca.Descripcion);
                datos.setearParametro("@Activo", marca.Activo);

                datos.setearParametroScalar("@Resultado", null, SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametroScalar("@Mensaje", null, SqlDbType.VarChar, ParameterDirection.Output, 500);

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




        public bool sp_editarMarca(Marca marca, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearPorcedimiento("sp_editarMarca");

                datos.setearParametro("@IdMarca", marca.IdMarca);
                datos.setearParametro("@Descripcion", marca.Descripcion);
                datos.setearParametro("@Activo", marca.Activo);

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
            return resultado;

        }



        public bool eliminarMarca(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearPorcedimiento("sp_eliminarMarca");
                datos.setearParametro("@IdMarca", id);

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
            return resultado;
        }



    }
}
