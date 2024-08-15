using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class EncuestaCheckUsuarioDTO
    {
        public int id { get; set; }
        public int idPerfil { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string correo { get; set; }
        public string empresa { get; set; }
        public string perfil { get; set; }
        public int? puestoID { get; set; }
        public tblP_Puesto puesto { get; set; }
        public string puestoDescripcion { get; set; }
        public string cc { get; set; }
        public tblP_Departamento departamento { get; set; }
        public List<tblP_Sistema> sistemas { get; set; }
        public List<tblP_Menu> permisos { get; set; }
        public bool tipoSGC { get; set; }
        public string usuarioSGC { get; set; }
        public string contrasena { get; set; }
        public string nombreUsuario { get; set; }
      

        public int encuestaID { get; set; }
        public int usuarioID { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public int departamentoID { get; set; }
        public string departamentoDesc { get; set; }
        public bool? soloLectura { get; set; }
        public bool? encuestaEditar { get; set; }
        public bool? encuestaEnviar { get; set; }
        public bool? encuestaTelefonica { get; set; }
        public bool? encuestaNotificacion { get; set; }
        public bool? encuestaPapel { get; set; }
        public bool? consultar { get; set; }
        public bool? editar { get; set; }
        public bool? enviar { get; set; }
        public bool? contestaTelefonica { get; set; }
        public bool? recibeNotificacion { get; set; }
        public bool? contestaPapel { get; set; }
        public bool crear { get; set; }
    }
}
