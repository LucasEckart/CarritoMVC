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
    public class CD_Ubicacion
    {
        public List<Provincia> obtenerProvincia()
        {
            List<Provincia> lista = new List<Provincia>();
            Conexion datos = new Conexion();

            try
            {
                datos.setearConsulta("select * from provincia");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Provincia aux = new Provincia();

                    aux.IdProvincia = (string)datos.Lector["IdProvincia"];
                    aux.Descripcion = (string)datos.Lector["Descipcion"];

                    lista.Add(aux);
                }
                return lista;

            }
            catch (Exception ex)
            {
                return new List<Provincia>();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public List<Partido> obtenerPartido(string IdProvincia)
        {
            List<Partido> lista = new List<Partido>();
            Conexion datos = new Conexion();

            try
            {
                datos.setearConsulta("select * from PARTIDO where IdProvincia = @IdProvincia");
                datos.setearParametro("@IdProvincia", IdProvincia);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Partido aux = new Partido();

                    aux.IdPartido = (string)datos.Lector["IdPartido"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.IdProvincia = (string)datos.Lector["IdProvincia"];

                    lista.Add(aux);
                }
                return lista;

            }
            catch (Exception ex)
            {
                return new List<Partido>();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }




    }
}
