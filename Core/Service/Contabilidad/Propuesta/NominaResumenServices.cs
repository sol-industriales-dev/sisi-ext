using Core.DAO.Contabilidad.Propuesta;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Nomina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Propuesta
{
    public class NominaResumenServices : INominaResumenDAO
    {
        #region Atributos
        private INominaResumenDAO p_nominaDAO;
        #endregion
        #region Propiedades
        public INominaResumenDAO NominaDAO
        {
            get { return p_nominaDAO; }
            set { p_nominaDAO = value; }
        }
        #endregion
        #region Constructores
        public NominaResumenServices(INominaResumenDAO nominaDAO)
        {
            this.NominaDAO = nominaDAO;
        }
        #endregion
        public bool guardarNominaPoliza(List<tblC_NominaPoliza> lst)
        {
            return NominaDAO.guardarNominaPoliza(lst);
        }
        public bool guardarNominaResumen(List<tblC_NominaResumen> lst)
        {
            return NominaDAO.guardarNominaResumen(lst);
        }
        public List<tblC_NominaPoliza> getLstPolizaNominaActiva(int mes)
        {
            return NominaDAO.getLstPolizaNominaActiva(mes);
        }
        public List<tblC_NominaPoliza> getLstPolizaNominaActiva(DateTime fecha_inicio, DateTime fecha_fin)
        {
            return NominaDAO.getLstPolizaNominaActiva(fecha_inicio, fecha_fin);
        }
        public List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio)
        {
            return NominaDAO.getLstResumenNominaActiva(fecha_inicio);
        }
        public List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio, DateTime fecha_fin)
        {
            return NominaDAO.getLstResumenNominaActiva(fecha_inicio, fecha_fin);
        }
        public List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio, DateTime fecha_fin, int tipoNomina)
        {
            return NominaDAO.getLstResumenNominaActiva(fecha_inicio, fecha_fin, tipoNomina);
        }
        public List<NominaPolizaDTO> getLstPolizaNomina(DateTime fecha_inicio, DateTime fecha_fin)
        {
            return NominaDAO.getLstPolizaNomina(fecha_inicio, fecha_fin);
        }
        public List<PeriodosNominaDTO> getLstPeriodoNomina()
        {
            return NominaDAO.getLstPeriodoNomina();
        }
        public List<PeriodosNominaDTO> getLstPeriodoNominaAnt()
        {
            return NominaDAO.getLstPeriodoNominaAnt();
        }
        public List<PeriodosNominaDTO> getLstSig4Semanas(DateTime fecha)
        {
            return NominaDAO.getLstSig4Semanas(fecha);
        }
        public Dictionary<string, object> GetNominasQuincenalesSemanales(DateTime fechaInicio, DateTime fechaFinal, int tipoNomina)
        {
            return NominaDAO.GetNominasQuincenalesSemanales(fechaInicio, fechaFinal, tipoNomina);
        }
        public Dictionary<string, object> GetNominasOtros(DateTime fechaInicio, DateTime fechaFinal)
        {
            return NominaDAO.GetNominasOtros(fechaInicio, fechaFinal);
        }
    }
}
