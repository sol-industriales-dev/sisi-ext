using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Subcontratistas
{
    public interface ISubcontratistasDAO
    {
        #region Catálogos
        #region Subcontratistas
        Dictionary<string, object> getSubcontratistas();
        Dictionary<string, object> guardarNuevoSubcontratista(tblX_SubContratista subcontratista);
        Dictionary<string, object> editarSubcontratista(tblX_SubContratista subcontratista);
        Dictionary<string, object> eliminarSubcontratista(tblX_SubContratista subcontratista);
        #endregion

        #region Contratos
        Dictionary<string, object> getContratos();
        Dictionary<string, object> guardarNuevoContrato(tblX_Contrato contrato);
        Dictionary<string, object> editarContrato(tblX_Contrato contrato);
        Dictionary<string, object> eliminarContrato(tblX_Contrato contrato);
        #endregion

        #region Proyectos
        Dictionary<string, object> getProyectos();
        Dictionary<string, object> guardarNuevoProyecto(tblX_Proyecto proyecto);
        Dictionary<string, object> editarProyecto(tblX_Proyecto proyecto);
        Dictionary<string, object> eliminarProyecto(tblX_Proyecto proyecto);
        #endregion

        #region Clientes
        Dictionary<string, object> getClientes();
        Dictionary<string, object> guardarNuevoCliente(tblX_Cliente cliente);
        Dictionary<string, object> editarCliente(tblX_Cliente cliente);
        Dictionary<string, object> eliminarCliente(tblX_Cliente cliente);
        #endregion
        #endregion

        Dictionary<string, object> getSubcontratistasArchivos(int filtroCarga);
        Dictionary<string, object> getSubcontratistaByID(int id);
        Dictionary<string, object> guardarArchivoSubcontratista(HttpPostedFileBase archivo, int documentacionID, string justificacion, DateTime? fechaVencimiento);
        Dictionary<string, object> guardarArchivoEditadoSubcontratista(HttpPostedFileBase archivo, int archivoCargadoID, DateTime? fechaVencimiento);
        Dictionary<string, object> guardarArchivoRenovadoSubcontratista(HttpPostedFileBase archivo, int archivoCargadoID, DateTime? fechaVencimiento);
        tblX_RelacionSubContratistaDocumentacion getArchivoSubcontratista(int id);
        Dictionary<string, object> getProveedor(int numeroProveedor);
        Dictionary<string, object> getDocumentacionPendiente(int subcontratistaID);
        Dictionary<string, object> getHistorialRechazado(int subcontratistaID);
        Dictionary<string, object> guardarValidacion(List<tblX_RelacionSubContratistaDocumentacion> listaValidacion, List<HttpPostedFileBase> archivos);
        Dictionary<string, object> CargarArchivosFijos();
        Dictionary<string, object> getJustificacionOpcional(int archivoID);
    }
}
