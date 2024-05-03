using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDato = new CD_Ubicacion();

        public List<Provincia> obtenerProvincia()
        {
            return objCapaDato.obtenerProvincia();
        }

        public List<Partido> obtenerPartido(string idDepartamento)
        {
            return objCapaDato.obtenerPartido(idDepartamento);
        }


    }
}
