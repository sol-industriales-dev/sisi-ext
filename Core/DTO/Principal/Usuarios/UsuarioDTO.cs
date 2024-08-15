using Core.DTO.Principal.Menus;
using Core.DTO.Sistemas;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Sistemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Usuarios
{
    public class UsuarioDTO
    {
        #region INIT
        public UsuarioDTO()
        {
            lstID_Usuarios = new List<int>();
        }
        #endregion

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
        public List<PermisosDTO> permisos2 { get; set; }
        public List<tblP_EmpresasDTO> empresas { get; set; }
        public bool tipoSGC { get; set; }
        public string usuarioSGC { get; set; }
        public string contrasena { get; set; }
        public string nombreUsuario { get; set; }
        public string cveEmpleado { get; set; }
        public int empresaActual { get; set; }
        public bool tipoSeguridad { get; set; }
        public string usuarioSeguridad { get; set; }
        public bool usuarioMAZDA { get; set; }
        public bool dashboardMaquinariaPermiso { get; set; }
        public bool dashboardMaquinariaAdmin { get; set; }
        public bool esAuditor { get; set; }
        public bool externoSeguridad { get; set; }
        public bool esCliente { get; set; }
        public bool VistaCalendario { get; set; }
        public bool sistemasGenerales { get; set; }
        public bool gestionRH { get; set; }
        public bool usuarioGeneral { get; set; }
        public bool estatus { get; set; }
        public bool externoGestor { get; set; }
        public List<UsuariosVinculadosDTO> correosVinculados { get; set; }
        public bool esColombia { get; set; }
        public bool isBajio { get; set; }
        public bool externoPatoos { get; set; }
        public string externoPatoosNombre { get; set; }
        public bool tipoPatoos { get; set; }

        public UsuarioFacultamientoFacturaDTO facultamientoFacturas { get; set; }

        #region ADICIONAL
        public List<int> lstID_Usuarios { get; set; }
        #endregion
    }
}