using Core.DAO.Principal.Usuarios;
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

namespace Core.Service.Principal.Usuarios
{
    public class UsuarioService : IUsuarioDAO
    {
        #region Atributos
        private IUsuarioDAO m_usuarioDAO;
        #endregion

        #region Propiedades
        public IUsuarioDAO UsuarioDAO
        {
            get { return m_usuarioDAO; }
            set { m_usuarioDAO = value; }
        }
        #endregion

        #region Constructores
        public UsuarioService(IUsuarioDAO usuarioDAO)
        {
            this.UsuarioDAO = usuarioDAO;
        }
        #endregion
        public UsuarioDTO IniciarSesion(string user, string password)
        {
            return UsuarioDAO.IniciarSesion(user, password);
        }

        public tblP_Usuario ObtenerPorNombreUsuario(string user)
        {
            return UsuarioDAO.ObtenerPorNombreUsuario(user);
        }

        public List<tblP_AccionesVista> getListaAcciones()
        {
            return UsuarioDAO.getListaAcciones();
        }
        public List<tblP_AccionesVista> getLstAccionesActual()
        {
            return UsuarioDAO.getLstAccionesActual();
        }
        public List<tblP_Usuario> ListUsersByName(string user)
        {
            return UsuarioDAO.ListUsersByName(user);
        }
        public List<tblP_Usuario> ListUsersByNameWithException(string user)
        {
            return UsuarioDAO.ListUsersByNameWithException(user);
        }

        public List<usuariosEncuestasDTO> getUsuariosData(int tipoCliente)
        {
            return UsuarioDAO.getUsuariosData(tipoCliente);
        }
        public List<tblRH_CatEmpleados> getPersonalByClaveEmpleado(string claveEmpleado)
        {
            return UsuarioDAO.getPersonalByClaveEmpleado(claveEmpleado);
        }
        public List<tblP_Usuario> ListUsersByNameAndMinute(string user, int id)
        {
            return UsuarioDAO.ListUsersByNameAndMinute(user, id);
        }
        public tblP_Usuario UserByName(string user)
        {
            return UsuarioDAO.UserByName(user);
        }
        public List<tblP_Usuario> ListUsersByNameAndActivity(string user, int id)
        {
            return UsuarioDAO.ListUsersByNameAndActivity(user, id);
        }
        public List<tblP_Usuario> ListResponsablesByNameAndActivity(string user, int id)
        {
            return UsuarioDAO.ListUsersByNameAndActivity(user, id);
        }
        public List<tblP_Usuario> ListUsuariosAutoComplete(string user, int id)
        {
            return UsuarioDAO.ListUsuariosAutoComplete(user, id);
        }
        public List<tblP_Usuario> ListUsersById(int user)
        {
            return UsuarioDAO.ListUsersById(user);
        }
        public List<tblP_Usuario> ListUsersActivos()
        {
            return UsuarioDAO.ListUsersActivos();
        }
        public List<string> correoPerfil(int perfilID, string areaCuenta)
        {
            return UsuarioDAO.correoPerfil(perfilID, areaCuenta);
        }
        public tblP_Usuario getPassByID(int id)
        {
            return UsuarioDAO.getPassByID(id);
        }
        public string getNombreSistemaActual()
        {
            return UsuarioDAO.getNombreSistemaActual();
        }
        public List<tblP_Usuario> ListUsersAll()
        {
            return UsuarioDAO.ListUsersAll();
        }
        public List<tblP_Sistema> ListSistemasAll()
        {
            return UsuarioDAO.ListSistemasAll();
        }
        public List<tblP_UsuariotblP_Empresas> getUsuarioEmpresa(int IdUsuario)
        {
            return UsuarioDAO.getUsuarioEmpresa(IdUsuario);
        }
        public tblP_Usuario SaveUsuario(tblP_Usuario user)
        {
            return UsuarioDAO.SaveUsuario(user);
        }
        public Dictionary<string, object> GuardarUsuario(tblP_Usuario usuario, List<tblP_MenutblP_Usuario> permisos, List<string> ccs, List<tblP_AccionesVistatblP_Usuario> accVistas, int sistema, List<int> empresa)
        {
            return UsuarioDAO.GuardarUsuario(usuario, permisos, ccs, accVistas, sistema, empresa);
        }
        public void SavePermisos(List<tblP_MenutblP_Usuario> permiso)
        {
            UsuarioDAO.SavePermisos(permiso);
        }
        public void SaveCCsUsuario(tblP_CC_Usuario Ccs)
        {
            UsuarioDAO.SaveCCsUsuario(Ccs);
        }
        public void SavePermisosVista(List<tblP_AccionesVistatblP_Usuario> permiso)
        {
            UsuarioDAO.SavePermisosVista(permiso);
        }
        public void saveUsuarioEmpresa(int idUsuario, List<int> lstEmpresa)
        {
            UsuarioDAO.saveUsuarioEmpresa(idUsuario, lstEmpresa);
        }
        public List<tblP_MenutblP_Usuario> getVistasUsuario(int id, int idVista)
        {
            return UsuarioDAO.getVistasUsuario(id, idVista);
        }
        public List<tblP_AccionesVistatblP_Usuario> getAccionesUsuario(int id)
        {
            return UsuarioDAO.getAccionesUsuario(id);
        }
        public List<tblP_CC_Usuario> getCCsUsuario(int id)
        {
            return UsuarioDAO.getCCsUsuario(id);
        }
        public List<tblP_CC> getCCsByUsuario(int id)
        {
            return UsuarioDAO.getCCsByUsuario(id);
        }
        public void DeletePermisos(int sistema, int idUsuario, List<tblP_MenutblP_Usuario> lstPermisos, List<tblP_AccionesVistatblP_Usuario> lstAcciones)
        {
            UsuarioDAO.DeletePermisos(sistema, idUsuario, lstPermisos, lstAcciones);
        }
        public void DeleteCcsUsuario(int idUsuario)
        {
            UsuarioDAO.DeleteCcsUsuario(idUsuario);
        }
        public void DeleteUsuarioEmpresa(int idUsuario)
        {
            UsuarioDAO.DeleteUsuarioEmpresa(idUsuario);
        }
        public bool getViewAction(int viewID, string accion)
        {
            return UsuarioDAO.getViewAction(viewID, accion);
        }
        public void replacePass()
        {
            UsuarioDAO.replacePass();
        }
        public List<tblP_Usuario> GetUsuariosByPuesto(List<int> idPuesto)
        {
            return UsuarioDAO.GetUsuariosByPuesto(idPuesto);
        }
        public List<tblP_Usuario> getListUsuarioByName(string user)
        {
            return UsuarioDAO.getListUsuarioByName(user);
        }
        public void enviarAcceso()
        {
            UsuarioDAO.enviarAcceso();
        }
        public List<tblP_Autoriza> getPerfilesUsuario(int tipoControl, string cc)
        {
            return UsuarioDAO.getPerfilesUsuario(tipoControl, cc);
        }
        public List<string> getListaCorreosEnvioGlobal(int modulo, string cc)
        {
            return UsuarioDAO.getListaCorreosEnvioGlobal(modulo, cc);
        }
        public List<string> getListaCorreosEnvioGlobal(string cc)
        {
            return UsuarioDAO.getListaCorreosEnvioGlobal( cc);
        }
        public UsuarioDTO setCambioEmpresa(string user, string password)
        {
            return UsuarioDAO.setCambioEmpresa(user, password);
        }
        public tblP_EmpresasDTO getURLEmpresa(int empresaID)
        {
            return UsuarioDAO.getURLEmpresa(empresaID);
        }
        public List<tblP_Departamento> getLstDept()
        {
            return UsuarioDAO.getLstDept();
        }
        public List<tblP_Puesto> getLstPuesto(int idDept)
        {
            return UsuarioDAO.getLstPuesto(idDept);
        }
        public List<tblP_Puesto> getAllPuesto()
        {
            return UsuarioDAO.getAllPuesto();
        }
        public List<tblP_Perfil> getLstPerfilActivo()
        {
            return UsuarioDAO.getLstPerfilActivo();
        }
        public List<tblP_Empresas> getLstEmpresasActivas()
        {
            return UsuarioDAO.getLstEmpresasActivas();
        }
        public List<int> getIdCCsUsuario(int id)
        {
            return UsuarioDAO.getIdCCsUsuario(id);
        }
        public List<tblP_CC> getUsuarioCC(int id)
        {
            return UsuarioDAO.getUsuarioCC(id);
        }
        public void removerAlerta(int idAlerta)
        {
            UsuarioDAO.removerAlerta(idAlerta);
        }
        public tblP_Usuario_Enkontrol getUserEk(int idUsuario)
        {
            return UsuarioDAO.getUserEk(idUsuario);
        }
        public tblP_Usuario_Enkontrol saveUsuarioEnkontrol(tblP_Usuario_Enkontrol usuario)
        {
            return UsuarioDAO.saveUsuarioEnkontrol(usuario);
        }

        public List<tblP_PermisosAutorizaCorreo> getPermisosAutorizaCorreo(int permiso)
        {
            return UsuarioDAO.getPermisosAutorizaCorreo(permiso);
        }

        public bool NecesitaIngresarDatosEnKontrol()
        {
            return UsuarioDAO.NecesitaIngresarDatosEnKontrol();
        }
        public Dictionary<string, object> ValidarDatosUsuarioEnKontrol(string password, int claveEmpleado)
        {
            return UsuarioDAO.ValidarDatosUsuarioEnKontrol(password, claveEmpleado);
        }


        public void ActualizarCCsUsuario(List<int> ccsIDs, int usuarioID)
        {
            UsuarioDAO.ActualizarCCsUsuario(ccsIDs, usuarioID);
        }

        public List<int> ObtenerIDsCCsUsuarioAutoriza(int usuarioID)
        {
            return UsuarioDAO.ObtenerIDsCCsUsuarioAutoriza(usuarioID);
        }
        public string getNombreUsuario(int id)
        {
            return UsuarioDAO.getNombreUsuario(id);
        }

        public tblP_Medico GetInformacionMedico(int usuario_id)
        {
            return UsuarioDAO.GetInformacionMedico(usuario_id);
        }

        public Tuple<Stream, string> DescargarArchivosComprimidos()
        {
            return UsuarioDAO.DescargarArchivosComprimidos();
        }
    }
}
