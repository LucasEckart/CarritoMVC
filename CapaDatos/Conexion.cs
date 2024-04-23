using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Configuration;

namespace CapaDatos
{
    public class Conexion
    {
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();


        private SqlConnection conexion;
        public SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public Conexion()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=DBCARRITO; integrated security=true");
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void setearPorcedimiento(string sp)
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = sp;
        }



        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void setearParametroScalar(string nombre, object valor, SqlDbType? tipoDato = null, ParameterDirection direccion = ParameterDirection.Input, int tamaño = -1)
        {
            SqlParameter parametro = new SqlParameter(nombre, valor);

            if (tipoDato.HasValue)
            {
                parametro.SqlDbType = tipoDato.Value;
            }

            if (tamaño != -1)
            {
                parametro.Size = tamaño;
            }

            parametro.Direction = direccion;

            comando.Parameters.Add(parametro);
        }


        public SqlParameter getearParametro(string nombre)
        {
            return comando.Parameters[nombre];
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                return comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int ejecutarAccionScalar()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                return int.Parse(comando.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }


    }
}
