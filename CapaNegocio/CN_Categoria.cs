using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;


namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categorias objCapaDato = new CD_Categorias();
        public List<Categoria> listar()
        {
            return objCapaDato.listar();
        }


        public int registrarCategoria(Categoria categoria, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(categoria.Descripcion) || string.IsNullOrWhiteSpace(categoria.Descripcion))
            {
                Mensaje = "La desripcion de la categoría no puede ser vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            { 
                return objCapaDato.registrarCategoria(categoria, out Mensaje);
            }
            else
            {
                return 0;
            }
        }


        public bool editarCategoria(Categoria categoria, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(categoria.Descripcion) || string.IsNullOrWhiteSpace(categoria.Descripcion))
            {
                Mensaje = "La desripcion de la categoría no puede ser vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.sp_editarCategoria(categoria, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool eliminarCategoria(int id, out string Mensaje)
        {
            return objCapaDato.eliminarCategoria(id, out Mensaje);
        }











    }
}
