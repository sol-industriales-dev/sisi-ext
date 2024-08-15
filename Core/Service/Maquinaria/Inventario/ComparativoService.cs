using Core.DAO.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System.Web;
namespace Core.Service.Maquinaria.Inventario
{
    public class ComparativoService : IComparativoDAO
    {
        private IComparativoDAO m_IComparativoDAO;

        public IComparativoDAO ComparativoDAO
        {
            get { return m_IComparativoDAO; }
            set { m_IComparativoDAO = value; }
        }
        public ComparativoService(IComparativoDAO ComparativoDAO)
        {
            this.ComparativoDAO = ComparativoDAO;
        }
        public bool ActivarBtn(int id)
        {
            return ComparativoDAO.ActivarBtn(id);
        }
        public Dictionary<string, object> CargarCuadrosComparativos()
        {
            return ComparativoDAO.CargarCuadrosComparativos();
        }
        public Dictionary<string, object> GuardarAsignacionSolicitud(int idCuadro, string folio)
        {
            return ComparativoDAO.GuardarAsignacionSolicitud(idCuadro, folio);
        }
        public List<ComparativoDTO> getTablaComparativoAdquisicion(ComparativoDTO objFiltro)
        {
            return ComparativoDAO.getTablaComparativoAdquisicion(objFiltro);
        }
        public List<ComparativoDTO> getTablaComparativoAutorizar(ComparativoDTO objFiltro)
        {
            return ComparativoDAO.getTablaComparativoAutorizar(objFiltro);
        }
        public List<ComparativoDTO> getTablaComparativoFinancieroAutorizar()
        {
            return ComparativoDAO.getTablaComparativoFinancieroAutorizar();
        }
        public ComparativoDTO guardarAutorizacion(List<tblM_ComparativoAdquisicionyRentaAutorizante> lstComparativo,ComparativoDTO objFiltro, bool Financiero, int idUsuario)
        {
            return ComparativoDAO.guardarAutorizacion(lstComparativo, objFiltro, Financiero,idUsuario);
        }
        public List<tblM_ComparativoAdquisicionyRentaAutorizante> CargarAutorizadores(int idAsignacion)
        {
            return ComparativoDAO.CargarAutorizadores(idAsignacion);
        }
        public List<tblM_ComparativoFinancieroAutorizante> CargarAutorizadoresFinanciero(int idAsignacion)
        {
            return ComparativoDAO.CargarAutorizadoresFinanciero(idAsignacion);
        }
     
        public List<ComparativoDTO> getTablaComparativoAdquisicionDetalle(int idAsignacion, int idUsuario)
        {
            return ComparativoDAO.getTablaComparativoAdquisicionDetalle(idAsignacion, idUsuario);
        }
        public List<ComparativoDTO> getTablaComparativoAdquisicionDetallePorCuadro(int idCuadro, int idUsuario)
        {
            return ComparativoDAO.getTablaComparativoAdquisicionDetallePorCuadro(idCuadro, idUsuario);
        }
        public Dictionary<string, object> getTablaComparativoFinancieroDetalle(int idFinanciero, int idUsuario)
        {
            return ComparativoDAO.getTablaComparativoFinancieroDetalle(idFinanciero, idUsuario);
        }
        public string getDescripcion(string AreaCuenta)
        {
            return ComparativoDAO.getDescripcion(AreaCuenta);
        }
        public ComparativoDTO addeditTablaComparativoAdiquisicion(List<HttpPostedFileBase> file,List<ComparativoAdquisicion> objComparativo)
        {
            return ComparativoDAO.addeditTablaComparativoAdiquisicion(file,objComparativo);
        }
        public bool deleteTablaComparativoAdiquisicion(int id)
        {
            return ComparativoDAO.deleteTablaComparativoAdiquisicion(id);
        }
        public ComparativoDTO addeditTablaComparativoFinanciero(List<ComparativoDTO> objComparativo)
        {
            return ComparativoDAO.addeditTablaComparativoFinanciero(objComparativo);
        }
        public bool deleteTablaComparativoFinanciero(int id)
        {
            return ComparativoDAO.deleteTablaComparativoFinanciero(id);
        }
        public ComparativoDTO addAdquisisionP(ComparativoDTO objComparativo)
        {
            return ComparativoDAO.addAdquisisionP(objComparativo);
        }
        public List<ComparativoDTO> CargarCuadroComparativo()
        {
            return ComparativoDAO.CargarCuadroComparativo();
        }
        public ComparativoDTO AutorizandoComparativo(int idComparativoDetalle, int idAsignacion, int idCuadro, int idUsuario)
        {
            return ComparativoDAO.AutorizandoComparativo(idComparativoDetalle, idAsignacion, idCuadro, idUsuario);
        }
        public ComparativoDTO indicadorColumnaMaximoVoto(int idAsignacion, int Tipo)
        {
            return ComparativoDAO.indicadorColumnaMaximoVoto(idAsignacion, Tipo);
        }
        public List<ComparativoDTO> getTablaComparativoFinanciero()
        {
            return ComparativoDAO.getTablaComparativoFinanciero();
        }
        public ComparativoDTO AutorizandoComparativoFinanciera(int idRow, int idAsignacion, int idUsuario)
        {
            return ComparativoDAO.AutorizandoComparativoFinanciera(idRow, idAsignacion, idUsuario);
        }
        public List<ComparativoDTO> getAutorizanteBotonPlus(int idAsignacion, int AutFin)
        {
            return ComparativoDAO.getAutorizanteBotonPlus(idAsignacion,AutFin);
        }
        public List<ComparativoDTO> getAutFin(int idAsignacion, int AutFin)
        {
            return ComparativoDAO.getAutFin(idAsignacion, AutFin);
        }
        public List<ComparativoDTO> getAutorizanteAdquisicion(int idAsignacion)
        {
            return ComparativoDAO.getAutorizanteAdquisicion(idAsignacion);
        }
        public List<ComparativoDTO> getAutorizanteAdquisicionPorCuadro(int idCuadro)
        {
            return ComparativoDAO.getAutorizanteAdquisicionPorCuadro(idCuadro);
        }
        public List<ComparativoDTO> getAutorizanteFinanciero(int idAsignacion)
        {
            return ComparativoDAO.getAutorizanteFinanciero(idAsignacion);
        }
        public List<tblM_Comp_CatFinanciero> FillFinanciero()
        {
            return ComparativoDAO.FillFinanciero();
        }
        public Dictionary<string, object> GuardarFinanciero(tblM_Comp_CatFinanciero financiero)
        {
            return ComparativoDAO.GuardarFinanciero(financiero);
        }
        public Dictionary<string, object> GuardarPlazo(tblM_Comp_CapPlazo plazo)
        {
            return ComparativoDAO.GuardarPlazo(plazo);
        }
        public Dictionary<string, object> GetPlazo(int financieroID, int plazoMeses)
        {
            return ComparativoDAO.GetPlazo(financieroID, plazoMeses);
        }
        public Dictionary<string, object> GetPlazoByID(int plazoID)
        {
            return ComparativoDAO.GetPlazoByID(plazoID);
        }
        public Dictionary<string, object> EditarPlazo(tblM_Comp_CapPlazo plazo)
        {
            return ComparativoDAO.EditarPlazo(plazo);
        }
        public Dictionary<string, object> FillCboFinancieros()
        {
            return ComparativoDAO.FillCboFinancieros();
        }
        public Dictionary<string, object> LlenarDatosFinanciero(int financieraID, int plazoMeses, decimal precio, int mesesRestantes)
        {
            return ComparativoDAO.LlenarDatosFinanciero(financieraID, plazoMeses, precio, mesesRestantes);
        }
        public Dictionary<string, object> ObtenerMensualidades(int financieraID, int plazoMeses, decimal precio)
        {
            return ComparativoDAO.ObtenerMensualidades(financieraID, plazoMeses, precio);
        }
        public int ObtenerEstatus(int idAsignacion)
        {
            return ComparativoDAO.ObtenerEstatus(idAsignacion);
        }
        public byte[] descargarArchivo(long examen_id)
        {
            return ComparativoDAO.descargarArchivo(examen_id);
        }
        public string getFileName(long examen_id)
        {
            return ComparativoDAO.getFileName(examen_id);
        }
        public int obtenerEstatusAutorizado(int Financiero, int idAsignacion, int idUsuario)
        {
            return ComparativoDAO.obtenerEstatusAutorizado(Financiero,idAsignacion, idUsuario);
        }

        public Tuple<DateTime?, string> GetUltimaAutorizacionCuadro(int idCuadro)
        {
            return ComparativoDAO.GetUltimaAutorizacionCuadro(idCuadro);
        }

        public Dictionary<string, object> ObtenerInformacionCuadro(int idCuadro)
        {
            return ComparativoDAO.ObtenerInformacionCuadro(idCuadro);
        }

        public Dictionary<string, object> GuardarCuadroIndependiente(ComparativoDTO comparativo, List<ComparativoAdquisicion> detalle, List<tblM_ComparativoAdquisicionyRentaAutorizante> listaAutorizantes, List<HttpPostedFileBase> listaArchivos)
        {
            return ComparativoDAO.GuardarCuadroIndependiente(comparativo, detalle, listaAutorizantes, listaArchivos);
        }
    }
}
