using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {

        private CD_Marcas objCapaDato = new CD_Marcas();
        public List<Marca> listar()
        {
            return objCapaDato.listar();
        }


        public int registrarMarca(Marca marca, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(marca.Descripcion) || string.IsNullOrWhiteSpace(marca.Descripcion))
            {
                Mensaje = "La desripcion de la marca no puede ser vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.registrarMarca(marca, out Mensaje);
            }
            else
            {
                return 0;
            }
        }


        public bool editarMarca(Marca marca, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(marca.Descripcion) || string.IsNullOrWhiteSpace(marca.Descripcion))
            {
                Mensaje = "La desripcion de la categoría no puede ser vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.sp_editarMarca(marca, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool eliminarMarca(int id, out string Mensaje)
        {
            return objCapaDato.eliminarMarca(id, out Mensaje);
        }

        public List<Marca> listarMarcaCategoria(int idcategoria)
        {
            return objCapaDato.listarMarcaCategoria(idcategoria);
        }

    }
}
