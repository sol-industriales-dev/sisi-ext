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

namespace Core.DAO.ControlObra
{
    public interface IGestionDeProyecto
    {
        #region ORDENES DE CAMBIO
        string obtenerNombreArchivo(int idOrdenDeCambio);
        List<ComboDTO> getProyecto();
        List<ordenesDeCambioDTO> obtenerOrdenesDeCambio(string cc, int idUsuario);
        List<ComboDTO> comboObtenerContratosyUltimasOrdenesDeCambio(List<string> filtroCC);
        List<ComboDTO> comboObtenerContratosyUltimasOrdenesDeCambioEditar();
        Dictionary<string, object> obtenerCamposDeOrdenDeCambio(ordenesDeCambioDTO parametros);
        Dictionary<string, object> nuevoEditarOrdenesDeCambio(ordenesDeCambioDTO parametros, int idUsuario);
        ordenDTO obtenerOrdenDeCambioByID(int idOrdenDeCambio);
        tblP_Encabezado getEncabezadoDatos();
        Dictionary<string, object> EliminarRenglon(int id);
        Dictionary<string, object> GenerandoFirmas(List<firmasDTO> lstFirmas, int idUsuario);
        List<tblCO_OC_Firmas> obtenerFirmas(int idOrdenDeCambio);
        Dictionary<string, object> RechazarOrdenDeCambio(int id);
        Dictionary<string, object> AutorizarOrdenDeCambio(int id, int tipo);
        List<Core.Entity.SubContratistas.Usuarios.tblP_Usuarios> ListUsersByNameWithException(string term, string cc);
        List<Core.Entity.Principal.Usuarios.tblP_Usuario> ListUsersByNameWithExceptionConstruplan(string user, string cc);
        Dictionary<string, object> Autorizar(int idOrdenDeCambio, int idUsuario, string firma);
        Dictionary<string, object> Rechazar(int idOrdenDeCambio, int idUsuario, string firma);
        Dictionary<string, object> obtenerTodasLasFirmas(string filtroCC, int filtroOrdenCambioID);
        Dictionary<string, object> GetEstatusGlobalOrdenesCambio(string cc);
        Dictionary<string, object> AutorizarRechazarOrdenCambio(bool esAutorizar, int idOrdenCambio, string comentarioRechazo);
        object GetInsumosSISUNAutocomplete(string term, bool busquedaPorNumero, string cc);
        Dictionary<string, object> GuardarDocumentoFirmado(firmasDTO parametros);
        Dictionary<string, object> obtenerOrdenesDeCambiabosPorAutorizar(string cc, int estatus, int idUsuario, int tipo);
        Dictionary<string, object> obtenerArchivos(int id);
        byte[] DescargarArchivos(long idDet, int tipo);
        string getFileName(long idDet, int tipo);
        List<ComboDTO> obtenerPuestos();
        Dictionary<string, object> EnviarCorreo(int idOrdenDeCambio, Byte[] archivo, int tipoCorreo);
        Dictionary<string, object> agregarPermisos(facultamientosDTO parametros);
        List<tblP_Permisos> obtenerPermisos(int idUsuario);

        Dictionary<string, object> GetDashboardOrdenCambio(string cc, int contrato_id, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> fillComboContratistasByContrato(int idContrato);

        #endregion
        Dictionary<string, object> autorizarArchivoFirmado(ordenesDeCambioDTO parametro);

        Dictionary<string, object> obtenerLstFacultamientos(string cc);
        Dictionary<string, object> agregarEditarFacultamientos(facultamientosDTO parametros);
        Dictionary<string, object> EliminarFacultamiento(int idFacultamiento);
        Dictionary<string, object> obtenerFacultamiento(int idUsuario);
        List<ComboDTO> obtenerCC();
        List<ComboDTO> obtenerUsuarios();
        /// <summary>
        /// Obtiene una lista en formato autocomplete sobre usuarios de SIGOPLAN, ya sea buscando por su nombre o por su clave de empleado.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="porClave"></param>
        /// <returns></returns>
        object GetUsuariosAutocomplete(string term, bool porClave);
        List<ComboDTO> FillComboEstados();
        List<ComboDTO> FillComboMunicipios(int estado_id);
        int GetPrivilegioUsuario();
        MemoryStream DescargarArchivo(int id);
    }
}
