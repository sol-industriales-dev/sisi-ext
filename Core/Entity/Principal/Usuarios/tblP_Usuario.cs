using Core.Entity.Principal.Configuraciones;
using Core.Entity.Principal.Menus;
using Core.Entity.GestorArchivos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Entity.ControlObra;
using Core.Entity.Administrativo.ReservacionVehiculo;
using Core.Entity.FileManager;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_Usuario
    {

        public int id { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombreUsuario { get; set; }
        public string correo { get; set; }
        public string empresa { get; set; }
        public int perfilID { get; set; }
        public virtual tblP_Perfil perfil { get; set; }
        public string contrasena { get; set; }
        public bool estatus { get; set; }
        public virtual List<tblP_Menu> permisos { get; set; }
        public int puestoID { get; set; }
        public virtual tblP_Puesto puesto { get; set; }
        public string cc { get; set; }
        public virtual List<tblP_AccionesVista> permisosPorVista { get; set; }
        public bool enviar { get; set; }
        public bool cliente { get; set; }
        public bool tipoSGC { get; set; }
        public string usuarioSGC { get; set; }
        public bool usuarioAuditor { get; set; }
        public string cveEmpleado { get; set; }
        public virtual List<tblGA_Permisos> permisosDirectorios { get; set; }
        public virtual List<tblGA_Version> versionesDirectorios { get; set; }
        public virtual List<tblGA_Vistas> vistasDirectorios { get; set; }
        public bool tipoSeguridad { get; set; }
        public string usuarioSeguridad { get; set; }
        public bool usuarioMAZDA { get; set; }
        public virtual List<tblGA_AccesoDepartamento> accesoDepartamentos { get; set; }
        public bool dashboardMaquinariaPermiso { get; set; }
        public bool dashboardMaquinariaAdmin { get; set; }
        public bool esAuditor { get; set; }
        public bool externoSeguridad { get; set; }
        public virtual List<tblFA_Autorizante> listaAutorizantesFa { get; set; }
        public virtual List<tblCO_Capitulos> capitulo { get; set; }
        public virtual List<tblRV_Solicitudes> solicitudesVehiculo { get; set; }
        public bool sistemasGenerales { get; set; }
        public virtual List<tblFM_Version> versionesArchivos { get; set; }
        public virtual List<tblFM_Permiso> permisosGestorArchivos { get; set; }
        public virtual List<tblFM_Permisos_Usuario> permisosUsuarioGestorArchivos { get; set; }
        public bool gestionRH { get; set; }
        public bool usuarioGeneral { get; set; }
        public bool externoGestor { get; set; }
        public bool esColombia { get; set; }
        public bool isBajio { get; set; }
        public bool externoPatoos { get; set; }
        public string externoPatoosNombre { get; set; }
        public bool tipoPatoos { get; set; }
        [NotMapped]
        public int idEnkontrol { get; set; }
    }
}
