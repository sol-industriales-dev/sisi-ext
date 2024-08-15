using Core.DTO.Encuestas;
using Core.DTO.Principal.Usuarios;
using Core.DTO.Sistemas;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.Sistemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Principal.Usuarios
{
    public interface IUsuarioDAO
    {
        List<tblRH_CatEmpleados> getPersonalByClaveEmpleado(string claveEmpleado);
        List<tblP_AccionesVista> getListaAcciones();
        List<tblP_AccionesVista> getLstAccionesActual();
        List<usuariosEncuestasDTO> getUsuariosData(int tipoCliente);
        List<string> correoPerfil(int perfilID, string areaCuenta);
        UsuarioDTO IniciarSesion(string user, string password);
        tblP_Usuario ObtenerPorNombreUsuario(string user);
        List<tblP_Usuario> ListUsersByName(string user);
        List<tblP_Usuario> ListUsersByNameWithException(string user);
        List<tblP_Usuario> ListUsersByNameAndMinute(string user, int id);
        tblP_Usuario UserByName(string user);
        List<tblP_Usuario> ListUsersByNameAndActivity(string user, int id);
        List<tblP_Usuario> ListResponsablesByNameAndActivity(string user, int id);
        List<tblP_Usuario> ListUsuariosAutoComplete(string user, int id);
        List<tblP_Usuario> ListUsersById(int id);
        List<tblP_Usuario> ListUsersActivos();
        tblP_Usuario getPassByID(int id);
        List<tblP_Usuario> ListUsersAll();
        List<tblP_Sistema> ListSistemasAll();
        string getNombreSistemaActual();

        tblP_Usuario SaveUsuario(tblP_Usuario user);
        Dictionary<string, object> GuardarUsuario(tblP_Usuario usuario, List<tblP_MenutblP_Usuario> permisos, List<string> ccs, List<tblP_AccionesVistatblP_Usuario> accVistas, int sistema, List<int> empresa);
        List<tblP_MenutblP_Usuario> getVistasUsuario(int id, int idVista);
        List<tblP_AccionesVistatblP_Usuario> getAccionesUsuario(int id);
        List<tblP_CC_Usuario> getCCsUsuario(int id);
        List<tblP_CC> getCCsByUsuario(int id);
        List<tblP_UsuariotblP_Empresas> getUsuarioEmpresa(int IdUsuario);
        void SavePermisos(List<tblP_MenutblP_Usuario> permiso);
        void SavePermisosVista(List<tblP_AccionesVistatblP_Usuario> permiso);
        void SaveCCsUsuario(tblP_CC_Usuario Ccs);
        void saveUsuarioEmpresa(int idUsuario, List<int> lstEmpresa);
        void DeletePermisos(int sistema, int idUsuario, List<tblP_MenutblP_Usuario> lstPermisos, List<tblP_AccionesVistatblP_Usuario> lstAcciones);
        void DeleteCcsUsuario(int idUsuario);
        void DeleteUsuarioEmpresa(int idUsuario);
        bool getViewAction(int viewID, string accion);
        void replacePass();
        List<tblP_Usuario> GetUsuariosByPuesto(List<int> idPuesto);
        List<tblP_Usuario> getListUsuarioByName(string user);
        void enviarAcceso();
        List<tblP_Autoriza> getPerfilesUsuario(int tipoControl, string cc);
        List<string> getListaCorreosEnvioGlobal(int modulo, string cc);
        List<string> getListaCorreosEnvioGlobal(string cc);
        UsuarioDTO setCambioEmpresa(string user, string password);
        tblP_EmpresasDTO getURLEmpresa(int empresaID);

        void removerAlerta(int idAlerta);
        List<tblP_Departamento> getLstDept();
        List<tblP_Puesto> getLstPuesto(int idDept);
        List<tblP_Puesto> getAllPuesto();
        List<tblP_Perfil> getLstPerfilActivo();
        List<tblP_Empresas> getLstEmpresasActivas();
        List<int> getIdCCsUsuario(int id);
        List<tblP_CC> getUsuarioCC(int id);
        tblP_Usuario_Enkontrol getUserEk(int idUsuario);
        tblP_Usuario_Enkontrol saveUsuarioEnkontrol(tblP_Usuario_Enkontrol usuario);
        List<tblP_PermisosAutorizaCorreo> getPermisosAutorizaCorreo(int permiso);

        bool NecesitaIngresarDatosEnKontrol();
        Dictionary<string, object> ValidarDatosUsuarioEnKontrol(string password, int claveEmpleado);

        // Actualiza los centros de costo de un usuario.
        void ActualizarCCsUsuario(List<int> ccsIDs, int usuarioID);

        /// <summary>
        /// Obtiene una lista de los ids de los centros de costo en los que el usuario está como autorizante.
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <returns></returns>
        List<int> ObtenerIDsCCsUsuarioAutoriza(int usuarioID);
        string getNombreUsuario(int id);
        tblP_Medico GetInformacionMedico(int usuario_id);

        Tuple<Stream, string> DescargarArchivosComprimidos();
    }
}
