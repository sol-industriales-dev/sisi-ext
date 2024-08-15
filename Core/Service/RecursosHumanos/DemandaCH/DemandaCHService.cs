using Core.DAO.RecursosHumanos.Demandas;
using Core.DAO.RecursosHumanos.Evaluacion360;
using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.RecursosHumanos.Demandas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.RecursosHumanos.Demandas
{
    public class DemandaCHService : IDemandaCHDAO
    {
        #region CONSTRUCTOR
        public IDemandaCHDAO e_iDemandaDAO { get; set; }
        private IDemandaCHDAO DemandaDAO
        {
            get { return e_iDemandaDAO; }
            set { e_iDemandaDAO = value; }
        }

        public DemandaCHService(IDemandaCHDAO iDemandaDAO)
        {
            this.e_iDemandaDAO = iDemandaDAO;
        }
        #endregion

        #region CAPTURAS
        public Dictionary<string, object> GetCapturas(CapturaDTO objDTO)
        {
            return DemandaDAO.GetCapturas(objDTO);
        }

        public Dictionary<string, object> GetInformacionEmpleado(CapturaDTO objDTO)
        {
            return DemandaDAO.GetInformacionEmpleado(objDTO);
        }

        public Dictionary<string, object> CECaptura(CapturaDTO objDTO)
        {
            return DemandaDAO.CECaptura(objDTO);
        }

        public Dictionary<string, object> EliminarCaptura(CapturaDTO objDTO)
        {
            return DemandaDAO.EliminarCaptura(objDTO);
        }

        public Dictionary<string, object> FillCboSemaforo()
        {
            return DemandaDAO.FillCboSemaforo();
        }

        public Dictionary<string, object> GetDatosActualizarCaptura(CapturaDTO objDTO)
        {
            return DemandaDAO.GetDatosActualizarCaptura(objDTO);
        }

        public Dictionary<string, object> FillCboJuzgados()
        {
            return DemandaDAO.FillCboJuzgados();
        }

        public Dictionary<string, object> FillCboTipoDemandas()
        {
            return DemandaDAO.FillCboTipoDemandas();
        }

        public Dictionary<string, object> CerrarDemanda(int idCaptura)
        {
            return DemandaDAO.CerrarDemanda(idCaptura);
        }

        #region ARCHIVOS ADJUNTOS
        public Dictionary<string, object> GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int FK_Captura, int tipoArchivo)
        {
            return DemandaDAO.GuardarArchivoAdjunto(lstArchivos, FK_Captura, tipoArchivo);
        }

        public Dictionary<string, object> GetArchivosAdjuntos(int FK_Captura)
        {
            return DemandaDAO.GetArchivosAdjuntos(FK_Captura);
        }

        public Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo)
        {
            return DemandaDAO.VisualizarArchivoAdjunto(idArchivo);
        }

        public Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo)
        {
            return DemandaDAO.EliminarArchivoAdjunto(idArchivo);
        }

        public Dictionary<string, object> FillCboTipoArchivos()
        {
            return DemandaDAO.FillCboTipoArchivos();
        }

        public Tuple<Stream, string> DescargarArchivo(int idArchivo)
        {
            return DemandaDAO.DescargarArchivo(idArchivo);
        }
        #endregion
        #endregion

        #region SEGUIMIENTOS
        public Dictionary<string, object> CrearSeguimiento(SeguimientoDTO objDTO)
        {
            return DemandaDAO.CrearSeguimiento(objDTO);
        }

        public Dictionary<string, object> GetEstatusFiniquitoEmpleadoDemanda(int claveEmpleado)
        {
            return DemandaDAO.GetEstatusFiniquitoEmpleadoDemanda(claveEmpleado);
        }
        #endregion

        #region HISTORICO
        public Dictionary<string, object> GetHistorico(HistoricoDTO objDTO)
        {
            return DemandaDAO.GetHistorico(objDTO);
        }

        public Dictionary<string, object> GetLstSeguimientos(int FK_Captura)
        {
            return DemandaDAO.GetLstSeguimientos(FK_Captura);
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> GetDashboard(DashboardDTO objFiltroDTO)
        {
            return DemandaDAO.GetDashboard(objFiltroDTO);
        }

        public Dictionary<string, object> FillCboAnios()
        {
            return DemandaDAO.FillCboAnios();
        }

        public Dictionary<string, object> FillCboCCDemandasRegistradas()
        {
            return DemandaDAO.FillCboCCDemandasRegistradas();
        }
        #endregion

        #region GENERALES
        public Dictionary<string, object> FillCboCC()
        {
            return DemandaDAO.FillCboCC();
        }

        public Dictionary<string, object> FillCboEstados()
        {
            return DemandaDAO.FillCboEstados();
        }

        public Dictionary<string, object> FillCboEstatus()
        {
            return DemandaDAO.FillCboEstatus();
        }

        public Dictionary<string, object> FillCboEmpleados()
        {
            return DemandaDAO.FillCboEmpleados();
        }
        #endregion
    }
}
