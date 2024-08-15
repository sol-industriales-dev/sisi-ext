using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.RecursosHumanos.Demandas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.RecursosHumanos.Demandas
{
    public interface IDemandaCHDAO
    {
        #region CAPTURAS
        Dictionary<string, object> GetCapturas(CapturaDTO objDTO);

        Dictionary<string, object> GetInformacionEmpleado(CapturaDTO objDTO);

        Dictionary<string, object> CECaptura(CapturaDTO objDTO);

        Dictionary<string, object> EliminarCaptura(CapturaDTO objDTO);

        Dictionary<string, object> FillCboSemaforo();

        Dictionary<string, object> GetDatosActualizarCaptura(CapturaDTO objDTO);

        Dictionary<string, object> FillCboJuzgados();

        Dictionary<string, object> FillCboTipoDemandas();

        Dictionary<string, object> CerrarDemanda(int idCaptura);

        #region ARCHIVOS ADJUNTOS
        Dictionary<string, object> GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int idActo, int tipoArchivo);

        Dictionary<string, object> GetArchivosAdjuntos(int idActo);

        Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo);

        Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo);

        Dictionary<string, object> FillCboTipoArchivos();

        Tuple<Stream, string> DescargarArchivo(int idArchivo);
        #endregion
        #endregion

        #region SEGUIMIENTOS
        Dictionary<string, object> CrearSeguimiento(SeguimientoDTO objDTO);

        Dictionary<string, object> GetEstatusFiniquitoEmpleadoDemanda(int claveEmpleado);
        #endregion

        #region HISTORICO
        Dictionary<string, object> GetHistorico(HistoricoDTO objDTO);

        Dictionary<string, object> GetLstSeguimientos(int FK_Captura);
        #endregion

        #region DASHBOARD
        Dictionary<string, object> GetDashboard(DashboardDTO objFiltroDTO);

        Dictionary<string, object> FillCboAnios();

        Dictionary<string, object> FillCboCCDemandasRegistradas();
        #endregion

        #region GENERALES
        Dictionary<string, object> FillCboCC();

        Dictionary<string, object> FillCboEstados();

        Dictionary<string, object> FillCboEstatus();

        Dictionary<string, object> FillCboEmpleados();
        #endregion
    }
}
