using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {

        private CD_Cliente objCapaDato = new CD_Cliente();


        public int registrarCliente(Cliente cliente, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                Mensaje = "El nombre del Cliente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(cliente.Apellido) || string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                Mensaje = "El Apellido del Cliente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(cliente.Correo) || string.IsNullOrWhiteSpace(cliente.Correo))
            {
                Mensaje = "El Correo del Cliente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                cliente.Clave = CN_Recursos.ConvertirSha256(cliente.Clave);
                return objCapaDato.registrarCliente(cliente, out Mensaje);             
            }
            else
            {
                return 0;
            }

        }


        public List<Cliente> listar()
        {
            return objCapaDato.listar();
        }


        public bool cambiarClave(int idCliente, string nuevaClave, out string mensaje)
        {
            return objCapaDato.cambiarClave(idCliente, nuevaClave, out mensaje);
        }

        public bool reestablecerClave(int idCliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CN_Recursos.generarClave();
            string clave = CN_Recursos.ConvertirSha256(nuevaClave);

            bool resultado = objCapaDato.reestablecerClave(idCliente, clave, out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña reestablecida";
                string mensaje_correo = "<h3>Su contraseña fue reestablecida correctamente</h3><br/><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaClave);

                bool respuesta = CN_Recursos.enviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo.";
                    return false;
                }

            }
            else
            {
                return false;
            }

        }


    }
}
