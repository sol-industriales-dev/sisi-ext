using Core.DAO.ControlObra;
using Core.DTO.ControlObra.Gestion;
using Core.DTO.Principal.Generales;
using Core.Entity.ControlObra;
using Core.Entity.ControlObra.GestionDeCambio;
using Core.Entity.Maquinaria.Reporte;
using Core.Entity.Principal.Menus;
using Core.Entity.SubContratistas.Usuarios;
using Core.Entity.Principal.Usuarios;
using Core.Entity.SubContratistas.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Core.Service.ControlObra
{
    public class GestionDeProyectoService : IGestionDeProyecto
    {
        private IGestionDeProyecto m_GestionDeProyectoDAO;
        public IGestionDeProyecto GestionDeProyectoDAO
        {
            get { return m_GestionDeProyectoDAO; }
            set { m_GestionDeProyectoDAO = value; }
        }
        public GestionDeProyectoService(IGestionDeProyecto GestionDeProyectoDAO)
        {
            this.GestionDeProyectoDAO = GestionDeProyectoDAO;
        }
        #region ORDENES DE CAMBIO
        public string obtenerNombreArchivo(int idOrdenDeCambio)
        {
            return GestionDeProyectoDAO.obtenerNombreArchivo(idOrdenDeCambio);
        }
        public List<ComboDTO> getProyecto()
        {
            return GestionDeProyectoDAO.getProyecto();
        }
        public List<ordenesDeCambioDTO> obtenerOrdenesDeCambio(string cc, int idUsuario)
        {
            return GestionDeProyectoDAO.obtenerOrdenesDeCambio(cc, idUsuario);
        }
        public List<ComboDTO> comboObtenerContratosyUltimasOrdenesDeCambio(List<string> filtroCC)
        {
            return GestionDeProyectoDAO.comboObtenerContratosyUltimasOrdenesDeCambio(filtroCC);
        }
        public Dictionary<string, object> obtenerCamposDeOrdenDeCambio(ordenesDeCambioDTO parametros)
        {
            return GestionDeProyectoDAO.obtenerCamposDeOrdenDeCambio(parametros);
        }
        public List<ComboDTO> comboObtenerContratosyUltimasOrdenesDeCambioEditar()
        {
            return GestionDeProyectoDAO.comboObtenerContratosyUltimasOrdenesDeCambioEditar();
        }
        public Dictionary<string, object> nuevoEditarOrdenesDeCambio(ordenesDeCambioDTO parametros, int idUsuario)
        {
            return GestionDeProyectoDAO.nuevoEditarOrdenesDeCambio(parametros, idUsuario);
        }
        public ordenDTO obtenerOrdenDeCambioByID(int idOrdenDeCambio)
        {
            return GestionDeProyectoDAO.obtenerOrdenDeCambioByID(idOrdenDeCambio);
        }
        public tblP_Encabezado getEncabezadoDatos()
        {
            return GestionDeProyectoDAO.getEncabezadoDatos();
        }
        public Dictionary<string, object> EliminarRenglon(int id)
        {
            return GestionDeProyectoDAO.EliminarRenglon(id);
        }
        public Dictionary<string, object> GenerandoFirmas(List<firmasDTO> lstFirmas, int idUsuario)
        {
            return GestionDeProyectoDAO.GenerandoFirmas(lstFirmas, idUsuario);
        }
        public List<tblCO_OC_Firmas> obtenerFirmas(int idOrdenDeCambio)
        {
            return GestionDeProyectoDAO.obtenerFirmas(idOrdenDeCambio);
        }
        public Dictionary<string, object> RechazarOrdenDeCambio(int id)
        {
            return GestionDeProyectoDAO.RechazarOrdenDeCambio(id);
        }
        public Dictionary<string, object> AutorizarOrdenDeCambio(int id, int tipo)
        {
            return GestionDeProyectoDAO.AutorizarOrdenDeCambio(id, tipo);
        }
        public List<Core.Entity.SubContratistas.Usuarios.tblP_Usuarios> ListUsersByNameWithException(string term, string cc)
        {
            return GestionDeProyectoDAO.ListUsersByNameWithException(term, cc);
        }
        public List<Core.Entity.Principal.Usuarios.tblP_Usuario> ListUsersByNameWithExceptionConstruplan(string user, string cc)
        {
            return GestionDeProyectoDAO.ListUsersByNameWithExceptionConstruplan(user, cc);
        }
        public Dictionary<string, object> Autorizar(int idOrdenDeCambio, int idUsuario, string firma)
        {
            return GestionDeProyectoDAO.Autorizar(idOrdenDeCambio, idUsuario, firma);
        }
        public Dictionary<string, object> Rechazar(int idOrdenDeCambio, int idUsuario, string firma)
        {
            return GestionDeProyectoDAO.Rechazar(idOrdenDeCambio, idUsuario, firma);
        }
        public Dictionary<string, object> obtenerTodasLasFirmas(string filtroCC, int filtroOrdenCambioID)
        {
            return GestionDeProyectoDAO.obtenerTodasLasFirmas(filtroCC, filtroOrdenCambioID);
        }
        public Dictionary<string, object> GetEstatusGlobalOrdenesCambio(string cc)
        {
            return GestionDeProyectoDAO.GetEstatusGlobalOrdenesCambio(cc);
        }
        public Dictionary<string, object> AutorizarRechazarOrdenCambio(bool esAutorizar, int idOrdenCambio, string comentarioRechazo)
        {
            return GestionDeProyectoDAO.AutorizarRechazarOrdenCambio(esAutorizar, idOrdenCambio, comentarioRechazo);
        }
        public object GetInsumosSISUNAutocomplete(string term, bool busquedaPorNumero, string cc)
        {
            return GestionDeProyectoDAO.GetInsumosSISUNAutocomplete(term, busquedaPorNumero, cc);
        }
        public Dictionary<string, object> GuardarDocumentoFirmado(firmasDTO parametros)
        {
            return GestionDeProyectoDAO.GuardarDocumentoFirmado(parametros);
        }
        public Dictionary<string, object> obtenerOrdenesDeCambiabosPorAutorizar(string cc, int estatus, int idUsuario, int tipo)
        {
            return GestionDeProyectoDAO.obtenerOrdenesDeCambiabosPorAutorizar(cc, estatus, idUsuario, tipo);
        }
        public Dictionary<string, object> obtenerArchivos(int id)
        {
            return GestionDeProyectoDAO.obtenerArchivos(id);
        }
        public byte[] DescargarArchivos(long idDet, int tipo)
        {
            return GestionDeProyectoDAO.DescargarArchivos(idDet, tipo);
        }
        public string getFileName(long idDet, int tipo)
        {
            return GestionDeProyectoDAO.getFileName(idDet, tipo);
        }
        public List<ComboDTO> obtenerPuestos()
        {
            return GestionDeProyectoDAO.obtenerPuestos();
        }
        public Dictionary<string, object> EnviarCorreo(int idOrdenDeCambio, Byte[] archivo, int tipoCorreo)
        {
            return GestionDeProyectoDAO.EnviarCorreo(idOrdenDeCambio, archivo, tipoCorreo);
        }
        public Dictionary<string, object> autorizarArchivoFirmado(ordenesDeCambioDTO parametro)
        {
            return GestionDeProyectoDAO.autorizarArchivoFirmado(parametro);
        }
        public Dictionary<string, object> agregarPermisos(facultamientosDTO parametros)
        {
            return GestionDeProyectoDAO.agregarPermisos(parametros);
        }

        public List<tblP_Permisos> obtenerPermisos(int idUsuario)
        {
            return GestionDeProyectoDAO.obtenerPermisos(idUsuario);
        }

        public Dictionary<string, object> GetDashboardOrdenCambio(string cc, int contrato_id, DateTime fechaInicio, DateTime fechaFin)
        {
            return GestionDeProyectoDAO.GetDashboardOrdenCambio(cc, contrato_id, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> fillComboContratistasByContrato(int idContrato)
        {
            return GestionDeProyectoDAO.fillComboContratistasByContrato(idContrato);
        }
        #endregion


        public Dictionary<string, object> obtenerLstFacultamientos(string cc)
        {
            return GestionDeProyectoDAO.obtenerLstFacultamientos(cc);
        }
        public Dictionary<string, object> agregarEditarFacultamientos(facultamientosDTO parametros)
        {
            return GestionDeProyectoDAO.agregarEditarFacultamientos(parametros);
        }
        public Dictionary<string, object> EliminarFacultamiento(int idFacultamiento)
        {
            return GestionDeProyectoDAO.EliminarFacultamiento(idFacultamiento);
        }
        public Dictionary<string, object> obtenerFacultamiento(int idUsuario)
        {
            return GestionDeProyectoDAO.obtenerFacultamiento(idUsuario);
        }
        public List<ComboDTO> obtenerCC()
        {
            return GestionDeProyectoDAO.obtenerCC();
        }
        public List<ComboDTO> obtenerUsuarios()
        {
            return GestionDeProyectoDAO.obtenerUsuarios();
        }
        public object GetUsuariosAutocomplete(string term, bool porClave)
        {
            return GestionDeProyectoDAO.GetUsuariosAutocomplete(term, porClave);
        }
        public List<ComboDTO> FillComboEstados()
        {
            return GestionDeProyectoDAO.FillComboEstados();
        }
        public List<ComboDTO> FillComboMunicipios(int estado_id)
        {
            return GestionDeProyectoDAO.FillComboMunicipios(estado_id);
        }
        public int GetPrivilegioUsuario()
        {
            return GestionDeProyectoDAO.GetPrivilegioUsuario();
		}
        public System.IO.MemoryStream DescargarArchivo(int id)
        {
            return GestionDeProyectoDAO.DescargarArchivo(id);
        }
    }
}
