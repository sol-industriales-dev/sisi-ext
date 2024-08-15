using Core.DAO.Subcontratistas;
using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Subcontratistas
{
    public class SubcontratistasService : ISubcontratistasDAO
    {
        public ISubcontratistasDAO iSubcontratistasDAO;
        public ISubcontratistasDAO subcontratistasDAO
        {
            get { return iSubcontratistasDAO; }
            set { iSubcontratistasDAO = value; }
        }
        public SubcontratistasService(ISubcontratistasDAO subDAO)
        {
            this.subcontratistasDAO = subDAO;
        }

        #region Catálogos
        #region Subcontratistas
        public Dictionary<string, object> getSubcontratistas()
        {
            return subcontratistasDAO.getSubcontratistas();
        }

        public Dictionary<string, object> guardarNuevoSubcontratista(tblX_SubContratista subcontratista)
        {
            return subcontratistasDAO.guardarNuevoSubcontratista(subcontratista);
        }

        public Dictionary<string, object> editarSubcontratista(tblX_SubContratista subcontratista)
        {
            return subcontratistasDAO.editarSubcontratista(subcontratista);
        }

        public Dictionary<string, object> eliminarSubcontratista(tblX_SubContratista subcontratista)
        {
            return subcontratistasDAO.eliminarSubcontratista(subcontratista);
        }
        #endregion

        #region Contratos
        public Dictionary<string, object> getContratos()
        {
            return subcontratistasDAO.getContratos();
        }

        public Dictionary<string, object> guardarNuevoContrato(tblX_Contrato contrato)
        {
            return subcontratistasDAO.guardarNuevoContrato(contrato);
        }

        public Dictionary<string, object> editarContrato(tblX_Contrato contrato)
        {
            return subcontratistasDAO.editarContrato(contrato);
        }

        public Dictionary<string, object> eliminarContrato(tblX_Contrato contrato)
        {
            return subcontratistasDAO.eliminarContrato(contrato);
        }
        #endregion

        #region Proyectos
        public Dictionary<string, object> getProyectos()
        {
            return subcontratistasDAO.getProyectos();
        }

        public Dictionary<string, object> guardarNuevoProyecto(tblX_Proyecto proyecto)
        {
            return subcontratistasDAO.guardarNuevoProyecto(proyecto);
        }

        public Dictionary<string, object> editarProyecto(tblX_Proyecto proyecto)
        {
            return subcontratistasDAO.editarProyecto(proyecto);
        }

        public Dictionary<string, object> eliminarProyecto(tblX_Proyecto proyecto)
        {
            return subcontratistasDAO.eliminarProyecto(proyecto);
        }
        #endregion

        #region Clientes
        public Dictionary<string, object> getClientes()
        {
            return subcontratistasDAO.getClientes();
        }

        public Dictionary<string, object> guardarNuevoCliente(tblX_Cliente cliente)
        {
            return subcontratistasDAO.guardarNuevoCliente(cliente);
        }

        public Dictionary<string, object> editarCliente(tblX_Cliente cliente)
        {
            return subcontratistasDAO.editarCliente(cliente);
        }

        public Dictionary<string, object> eliminarCliente(tblX_Cliente cliente)
        {
            return subcontratistasDAO.eliminarCliente(cliente);
        }
        #endregion
        #endregion

        public Dictionary<string, object> getSubcontratistasArchivos(int filtroCarga)
        {
            return subcontratistasDAO.getSubcontratistasArchivos(filtroCarga);
        }

        public Dictionary<string, object> getSubcontratistaByID(int id)
        {
            return subcontratistasDAO.getSubcontratistaByID(id);
        }

        public Dictionary<string, object> guardarArchivoSubcontratista(HttpPostedFileBase archivo, int documentacionID, string justificacion, DateTime? fechaVencimiento)
        {
            return subcontratistasDAO.guardarArchivoSubcontratista(archivo, documentacionID, justificacion, fechaVencimiento);
        }

        public Dictionary<string, object> guardarArchivoEditadoSubcontratista(HttpPostedFileBase archivo, int archivoCargadoID, DateTime? fechaVencimiento)
        {
            return subcontratistasDAO.guardarArchivoEditadoSubcontratista(archivo, archivoCargadoID, fechaVencimiento);
        }

        public Dictionary<string, object> guardarArchivoRenovadoSubcontratista(HttpPostedFileBase archivo, int archivoCargadoID, DateTime? fechaVencimiento)
        {
            return subcontratistasDAO.guardarArchivoRenovadoSubcontratista(archivo, archivoCargadoID, fechaVencimiento);
        }

        public tblX_RelacionSubContratistaDocumentacion getArchivoSubcontratista(int id)
        {
            return subcontratistasDAO.getArchivoSubcontratista(id);
        }

        public Dictionary<string, object> getProveedor(int numeroProveedor)
        {
            return subcontratistasDAO.getProveedor(numeroProveedor);
        }

        public Dictionary<string, object> getDocumentacionPendiente(int subcontratistaID)
        {
            return subcontratistasDAO.getDocumentacionPendiente(subcontratistaID);
        }

        public Dictionary<string, object> getHistorialRechazado(int subcontratistaID)
        {
            return subcontratistasDAO.getHistorialRechazado(subcontratistaID);
        }

        public Dictionary<string, object> guardarValidacion(List<tblX_RelacionSubContratistaDocumentacion> listaValidacion, List<HttpPostedFileBase> archivos)
        {
            return subcontratistasDAO.guardarValidacion(listaValidacion, archivos);
        }
        public Dictionary<string, object> CargarArchivosFijos()
        {
            return subcontratistasDAO.CargarArchivosFijos();
        }

        public Dictionary<string, object> getJustificacionOpcional(int archivoID)
        {
            return subcontratistasDAO.getJustificacionOpcional(archivoID);
        }
    }
}
