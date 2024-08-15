using Core.DAO.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entity.Maquinaria.Inventario;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Inventario;
using System.Web;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IComparativoDAO
    {
        List<ComparativoDTO> getTablaComparativoAdquisicion(ComparativoDTO objFiltro);
        List<ComparativoDTO> getTablaComparativoAutorizar(ComparativoDTO objFiltro);
        ComparativoDTO guardarAutorizacion(List<tblM_ComparativoAdquisicionyRentaAutorizante> lstComparativo,ComparativoDTO objFiltro, bool Financiero,int idUsuario);
        List<tblM_ComparativoAdquisicionyRentaAutorizante> CargarAutorizadores(int idAsignacion);
        List<tblM_ComparativoFinancieroAutorizante> CargarAutorizadoresFinanciero(int idAsignacion);
        List<ComparativoDTO> getTablaComparativoAdquisicionDetalle(int idAsignacion, int idUsuario);
        List<ComparativoDTO> getTablaComparativoAdquisicionDetallePorCuadro(int idCuadro, int idUsuario);
        Dictionary<string, object> getTablaComparativoFinancieroDetalle(int idFinanciero, int idUsuario);
        string getDescripcion(string AreaCuenta);
        ComparativoDTO addeditTablaComparativoAdiquisicion(List<HttpPostedFileBase> file,List<ComparativoAdquisicion> objComparativo);
        bool deleteTablaComparativoAdiquisicion(int id);
        Dictionary<string, object> CargarCuadrosComparativos();
        Dictionary<string, object> GuardarAsignacionSolicitud(int idCuadro, string folio);
        ComparativoDTO addeditTablaComparativoFinanciero(List<ComparativoDTO> objComparativo);
        bool deleteTablaComparativoFinanciero(int id);
        ComparativoDTO addAdquisisionP(ComparativoDTO objComparativo);
        List<ComparativoDTO> CargarCuadroComparativo();
     
        ComparativoDTO AutorizandoComparativo(int idComparativoDetalle, int idAsignacion, int idCuadro, int idUsuario);
        ComparativoDTO indicadorColumnaMaximoVoto(int idAsignacion, int Tipo);
        List<ComparativoDTO> getTablaComparativoFinanciero();
        List<ComparativoDTO> getTablaComparativoFinancieroAutorizar();
        ComparativoDTO AutorizandoComparativoFinanciera(int idRow, int idAsignacion, int idUsuario);
        List<ComparativoDTO> getAutorizanteBotonPlus(int idAsignacion, int AutFin);
        List<ComparativoDTO> getAutFin(int idAsignacion, int AutFin);
        bool ActivarBtn(int id);
        
        List<ComparativoDTO> getAutorizanteAdquisicion(int idAsignacion);
        List<ComparativoDTO> getAutorizanteAdquisicionPorCuadro(int idCuadro);
        List<ComparativoDTO> getAutorizanteFinanciero(int idAsignacion);
        List<tblM_Comp_CatFinanciero> FillFinanciero();
        Dictionary<string, object> GuardarFinanciero(tblM_Comp_CatFinanciero financiero);
        Dictionary<string, object> GuardarPlazo(tblM_Comp_CapPlazo plazo);
        Dictionary<string, object> GetPlazo(int financieroID, int plazoMeses);
        Dictionary<string, object> GetPlazoByID(int plazoID);
        Dictionary<string, object> EditarPlazo(tblM_Comp_CapPlazo plazo);
        Dictionary<string, object> FillCboFinancieros();
        Dictionary<string, object> LlenarDatosFinanciero(int financieraID, int plazoMeses, decimal precio, int mesesRestantes);
        Dictionary<string, object> ObtenerMensualidades(int financieraID, int plazoMeses, decimal precio);
        int ObtenerEstatus(int idAsignacion);
        byte[] descargarArchivo(long examen_id);
        string getFileName(long examen_id);
        int obtenerEstatusAutorizado(int Financiero,int idAsignacion,int idUsuario);
        Tuple<DateTime?, string> GetUltimaAutorizacionCuadro(int idCuadro);

        Dictionary<string, object> ObtenerInformacionCuadro(int idCuadro);
        Dictionary<string, object> GuardarCuadroIndependiente(ComparativoDTO comparativo, List<ComparativoAdquisicion> detalle, List<tblM_ComparativoAdquisicionyRentaAutorizante> listaAutorizantes, List<HttpPostedFileBase> listaArchivos);
    }
}
