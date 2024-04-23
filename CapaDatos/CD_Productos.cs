using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Productos
    {
        public List<Producto> listar()
        {
            List<Producto> lista = new List<Producto>();
            Conexion datos = new Conexion();
            CultureInfo cultureInfo = new CultureInfo("es-AR");

            try
            {
                string consulta = "select p.IdProducto, p.Descripcion, p.Nombre, " +
                    "m.IdMarca, m.Descripcion AS DesMarca, c.IdCategoria, c.Descripcion AS DesCategoria, " +
                    "p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo " +
                    "from PRODUCTO p " +
                    "inner join MARCA m on m.IdMarca = p.IdMarca " + 
                    "inner join CATEGORIA c on c.IdCategoria = p.IdCategoria";

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Producto aux = new Producto();
                    aux.IdMarca = new Marca();
                    aux.IdCategoria = new Categoria();

                    aux.IdProducto = (int)datos.Lector["IdProducto"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = decimal.Parse(datos.Lector["Precio"].ToString(), cultureInfo);
                    aux.Stock = (int)datos.Lector["Stock"];
                    aux.RutaImagen = datos.Lector["RutaImagen"] != DBNull.Value ? (string)datos.Lector["RutaImagen"] : "";
                    aux.NombreImagen = datos.Lector["NombreImagen"] != DBNull.Value ? (string)datos.Lector["NombreImagen"] : "";

                    aux.Activo = (bool)datos.Lector["Activo"];


                    aux.IdMarca.IdMarca = (int)datos.Lector["IdMarca"];
                    aux.IdMarca.Descripcion = (string)datos.Lector["DesMarca"];
                    aux.IdCategoria.IdCategoria = (int)datos.Lector["IdCategoria"];
                    aux.IdCategoria.Descripcion = (string)datos.Lector["DesCategoria"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                return new List<Producto>();
            }
            finally
            {
                datos.cerrarConexion();
            }

        }


        public int registrarProducto(Producto producto, out string Mensaje)
        {
            Conexion datos = new Conexion();
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                datos.setearPorcedimiento("sp_registrarProducto");

                datos.setearParametro("@Nombre", producto.Nombre);
                datos.setearParametro("@Descripcion", producto.Descripcion);
                datos.setearParametro("@IdMarca", producto.IdMarca.IdMarca);
                datos.setearParametro("@IdCategoria", producto.IdCategoria.IdCategoria);
                datos.setearParametro("@Precio", producto.Precio);
                datos.setearParametro("@Stock", producto.Stock);
                datos.setearParametro("@Activo", producto.Activo);

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




        public bool editarProducto(Producto producto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearPorcedimiento("sp_editarProducto");

                datos.setearParametro("@IdProducto", producto.IdProducto);
                datos.setearParametro("@Nombre", producto.Nombre);
                datos.setearParametro("@Descripcion", producto.Descripcion);
                datos.setearParametro("@IdMarca",producto.IdMarca.IdMarca);
                datos.setearParametro("@IdCategoria", producto.IdCategoria.IdCategoria);
                datos.setearParametro("@Precio", producto.Precio);
                datos.setearParametro("@Stock", producto.Stock);
                datos.setearParametro("@Activo", producto.Activo);

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


        public bool guardarDatosImg(Producto producto, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                string consulta = "update PRODUCTO set RutaImagen = @RutaImagen, NombreImagen = @NombreImagen where IdProducto = @IdProducto";
                datos.setearConsulta(consulta);

                string rutaImagen = !string.IsNullOrEmpty(producto.RutaImagen) ? producto.RutaImagen : "";
                string nombreImagen = !string.IsNullOrEmpty(producto.NombreImagen) ? producto.NombreImagen : "";

                datos.setearParametro("@RutaImagen", producto.RutaImagen);
                datos.setearParametro("@NombreImagen", producto.NombreImagen);
                datos.setearParametro("@IdProducto", producto.IdProducto);
                int rta = datos.ejecutarAccion();

                if(rta > 0)
                {
                    resultado = true;
                }
                else
                {
                    mensaje = "No se pudo actualziar la iamgen";
                }

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





        public bool eliminarProducto(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            Conexion datos = new Conexion();

            try
            {
                datos.setearPorcedimiento("sp_eliminarProducto");
                datos.setearParametro("@IdProducto", id);

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
