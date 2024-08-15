using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Nomina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Propuesta
{
    public interface INominaResumenDAO
    {
        bool guardarNominaPoliza(List<tblC_NominaPoliza> lst);
        bool guardarNominaResumen(List<tblC_NominaResumen> lst);
        List<tblC_NominaPoliza> getLstPolizaNominaActiva(int mes);
        List<tblC_NominaPoliza> getLstPolizaNominaActiva(DateTime fecha_inicio, DateTime fecha_fin);
        List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio);
        List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio, DateTime fecha_fin);
        List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio, DateTime fecha_fin, int tipoNomina);
        List<NominaPolizaDTO> getLstPolizaNomina(DateTime fecha_inicio, DateTime fecha_fin);
        List<PeriodosNominaDTO> getLstPeriodoNomina();
        List<PeriodosNominaDTO> getLstPeriodoNominaAnt();
        List<PeriodosNominaDTO> getLstSig4Semanas(DateTime fecha);
        Dictionary<string, object> GetNominasQuincenalesSemanales(DateTime fechaInicio, DateTime fechaFinal, int tipoNomina);
        Dictionary<string, object> GetNominasOtros(DateTime fechaInicio, DateTime fechaFinal);

    }
}
