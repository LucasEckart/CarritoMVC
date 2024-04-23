using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Xml;
using System.Collections;
using System.Data;

namespace CapaDatos
{
    public class CD_Categorias
    {
        public List<Categoria> listar()
        {
            List<Categoria> lista = new List<Categoria>();
            Conexion datos = new Conexion();

            try
            {
                datos.setearConsulta("select IdCategoria, Descripcion, Activo from CATEGORIA");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();

                    aux.IdCategoria = (int)datos.Lector["IdCategoria"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Activo = (bool)datos.Lector["Activo"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar categorias: " + ex.Message);
                return new List<Categoria>();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public int registrarCategoria(Categoria categoria, out string Mensaje)
        {
            Conexion datos = new Conexion();
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                datos.setearPorcedimiento("sp_registrarCategoria");

                datos.setearParametro("@Descripcion", categoria.Descripcion);
                datos.setearParametro("@Activo", categoria.Activo);

                datos.setearParametro("@Resultado", SqlDbType.Int);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar);

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


        public bool sp_editarCategoria(Categoria categoria, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearPorcedimiento("sp_editarCategoria");

                datos.setearParametro("@IdCategoria", categoria.IdCategoria);
                datos.setearParametro("@Descripcion", categoria.Descripcion);
                datos.setearParametro("@Activo", categoria.Activo);

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



        public bool eliminarCategoria(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearPorcedimiento("sp_eliminarCategoria");
                datos.setearParametro("@IdCategoria", id);

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
