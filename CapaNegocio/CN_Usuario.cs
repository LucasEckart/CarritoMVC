using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> listar()
        {
            return objCapaDato.listar();
        }


        public int registrarUsuario(Usuario usuario, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Nombre)) 
            {
                Mensaje = "El nombre del usuario no puede ser vacio";           
            }

            if (string.IsNullOrEmpty(usuario.Apellido) || string.IsNullOrWhiteSpace(usuario.Apellido))
            {
                Mensaje = "El Apellido del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrWhiteSpace(usuario.Correo))
            {
                Mensaje = "El Correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.generarClave();

                string asunto = "Creacion de cuenta";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3><br/><p>Su contraseña para acceder es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.enviarCorreo(usuario.Correo, asunto, mensaje_correo);
                if (respuesta)
                {
                    usuario.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDato.registrarUsuario(usuario, out Mensaje);
                }
                else
                {
                    usuario.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDato.registrarUsuario(usuario, out Mensaje);
                }
            }
            else
            {
                return 0;
            }

        }

        public bool editarUsuario(Usuario usuario, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Nombre))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(usuario.Apellido) || string.IsNullOrWhiteSpace(usuario.Apellido))
            {
                Mensaje = "El Apellido del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrWhiteSpace(usuario.Correo))
            {
                Mensaje = "El Correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.editarUsuario(usuario, out Mensaje);
            }
            else
            {
                return false;
            }

        }

        public bool eliminarUsuario(int id, out string Mensaje)
        {
            return objCapaDato.eliminarUsuario(id, out Mensaje);
        }

    }
}
