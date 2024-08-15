using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DTO;
using Core.Entity.Principal.Usuarios;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_Raya : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public int nominaID { get; set; }
        public int numeroEmpleado { get; set; }
        public string nombreCompleto { get; set; }
        public decimal sueldoUnidades { get; set; }
        public decimal sueldoImporte { get; set; }
        public decimal septimoDiaUnidades { get; set; }
        public decimal septimoDiaImporte { get; set; }
        public decimal horasExtraDoblesUnidades { get; set; }
        public decimal horasExtraDoblesImporte { get; set; }
        public decimal horasExtrasTriplesUnidades { get; set; }
        public decimal horasExtrasTriplesImporte { get; set; }
        public decimal primaDominicalUnidades { get; set; }
        public decimal primaDominicalImporte { get; set; }
        public decimal ansoFestivoTrabajadoUnidades { get; set; }
        public decimal ansoFestivoTrabajadoImporte { get; set; }
        public decimal retardosUnidades { get; set; }
        public decimal faltaInjustificadaUnidades { get; set; }
        public decimal faltaPorSancionUnidades { get; set; }
        public decimal incapacidadEnfermedad3DiasUnidades { get; set; }
        public decimal incapacidadEnfermedad3DiasImporte { get; set; }
        public decimal incapacidadRiesgoTrabajoUnidades { get; set; }
        public decimal incapacidadPorMaternindadUnidades { get; set; }
        public decimal incapacidadTrayectoUnidades { get; set; }
        public decimal premiosAsistenciaUnidades { get; set; }
        public decimal premiosAsistenciaImporte { get; set; }
        public decimal bonoPuntualidadUnidades { get; set; }
        public decimal bonoPuntualidadImporte { get; set; }
        public decimal permisosSinGoceUnidades { get; set; }
        public decimal permisosConGoceUnidades { get; set; }
        public decimal permisosConGoceImporte { get; set; }
        public decimal despensa2Importe { get; set; }
        public decimal fondoAhorroEmpresaImporte { get; set; }
        public decimal compensacionIsptImssImporte { get; set; }
        public decimal vacacionTrabajadaUnidades { get; set; }
        public decimal vacacionTrabajadaImporte { get; set; }
        public decimal vacacionFiniquitoUnidades { get; set; }
        public decimal vacacionFiniquitoImporte { get; set; }
        public decimal vacacionDisfrutadaUnidades { get; set; }
        public decimal vacacionDisfrutadaImporte { get; set; }
        public decimal primaVacacionalUnidades { get; set; }
        public decimal primaVacacionalImporte { get; set; }
        public decimal primaVacacional2Unidades { get; set; }
        public decimal primaVacacional2Importe { get; set; }
        public decimal aguinaldoGravadoUnidades { get; set; }
        public decimal aguinaldoGravadoImporte { get; set; }
        public decimal aguinaldoExentoUnidades { get; set; }
        public decimal aguinaldoExentoImporte { get; set; }
        public decimal primaAntiguedadUnidades { get; set; }
        public decimal primaAntiguedadImporte { get; set; }
        public decimal indemnizacion3MesesUnidades { get; set; }
        public decimal indemnizacion3MesesImporte { get; set; }
        public decimal indemnizacion20DiasUnidades { get; set; }
        public decimal indemnizacion20DiasImporte { get; set; }
        public decimal indemnizacionOtrosUnidades { get; set; }
        public decimal indemnizacionOtrosImporte { get; set; }
        public decimal repartoUtilidadGraImporte { get; set; }
        public decimal repartoUtilidadExeImporte { get; set; }
        public decimal prestamoAhorroImporte { get; set; }
        public decimal fondoAhorroEmpresa2Importe { get; set; }
        public decimal fondoAhorroEmpleadoImporte { get; set; }
        public decimal interesPorFondoAhorroImporte { get; set; }
        public decimal diferenciaCategoriaUnidades { get; set; }
        public decimal diferenciaCategoriaImporte { get; set; }
        public decimal bonosUnidades { get; set; }
        public decimal bonosImporte { get; set; }
        public decimal retroactivoDiversoImporte { get; set; }
        public decimal comisionesSueldoImporte { get; set; }
        public decimal incentivoProductivoImporte { get; set; }
        public decimal gratificacionEspecialImporte { get; set; }
        public decimal prevSocialImporte { get; set; }
        public decimal prevSocialAlimentosImporte { get; set; }
        public decimal prevSocialHabitacionImporte { get; set; }
        public decimal otrasPercepcionesImporte { get; set; }
        public decimal compensacionUnicaExtraordinariaImporte { get; set; }
        public decimal bonoProduccionUnidades { get; set; }
        public decimal bonoProduccionImporte { get; set; }
        public decimal despensaImporte { get; set; }
        public decimal previsionSocialImporte { get; set; }
        public decimal altoCostoVidaImporte { get; set; }
        public decimal despensa3Importe { get; set; }
        public decimal prevSocialAlimentos2Importe { get; set; }
        public decimal altoCostoVida2Importe { get; set; }
        public decimal subsidioEmpleoImporte { get; set; }
        public decimal totalPercepciones { get; set; }
        public decimal isrImporte { get; set; }
        public decimal imssImporte { get; set; }
        public decimal isptCompImporte { get; set; }
        public decimal cuotaSindicalImporte { get; set; }
        public decimal cuotaSindicalExtraImporte { get; set; }
        public decimal infonavitImporte { get; set; }
        public decimal fonacotImporte { get; set; }
        public decimal pensionAlimenticiaImporte { get; set; }
        public decimal vejezSarImporte { get; set; }
        public decimal descuentosAlimentosImporte { get; set; }
        public decimal descuentosAlimentosPendImporte { get; set; }
        public decimal adeudoEmpresaImporte { get; set; }
        public decimal interesesPrestamoImporte { get; set; }
        public decimal fondoAhorroEmpleado2Importe { get; set; }
        public decimal fondoAhorroEmpresa3Importe { get; set; }
        public decimal infonavitMesAnteriorImporte { get; set; }
        public decimal anticiposDiversosImporte { get; set; }
        public decimal isptSaldoAnteriorImporte { get; set; }
        public decimal descuentosImporte { get; set; }
        public decimal famsaImporte { get; set; }
        public decimal prestamosUnidades { get; set; }
        public decimal prestamosImporte { get; set; }
        public decimal axaUnidades { get; set; }
        public decimal axaImporte { get; set; }
        public decimal ajusteNominaUnidades { get; set; }
        public decimal ajusteNominaImporte { get; set; }
        public decimal totalDeducciones { get; set; }
        public decimal netoPagar { get; set; }
        public decimal pteExeCompSeparacionUnidades { get; set; }
        public decimal pteExeCompSeparacionImporte { get; set; }
        public decimal parteExentaIsptUnidades { get; set; }
        public decimal parteExentaIsptImporte { get; set; }
        public decimal percGravPatronUnidades { get; set; }
        public decimal percGravPatronImporte { get; set; }
        public decimal isptRetenPatronUnidades { get; set; }
        public decimal isptRetenPatronImporte { get; set; }
        public decimal basesRepartoUnidades { get; set; }
        public decimal basesRepartoImporte { get; set; }
        public decimal sueldoVariableImssUnidades { get; set; }
        public decimal sueldoVariableImssImporte { get; set; }
        public decimal prevSocialGravableServUnidades { get; set; }
        public decimal prevSocialGravableServImporte { get; set; }
        public decimal isptSalAnualUnidades { get; set; }
        public decimal isptSalAnualImporte { get; set; }
        public decimal vacacIncapUnidades { get; set; }
        public decimal vacacIncapImporte { get; set; }
        public decimal recalculoIsptMesUnidades { get; set; }
        public decimal recalculoIsptMesImporte { get; set; }
        public decimal prevSocialGravableUnidades { get; set; }
        public decimal prevSocialGravableImporte { get; set; }
        public decimal credSalTablaUnidades { get; set; }
        public decimal credSalTablaImporte { get; set; }
        public decimal vacacionesAnteriorUnidades { get; set; }
        public decimal vacacionesAnteriorImporte { get; set; }
        public decimal vacacionesUnidades { get; set; }
        public decimal vacacionesImporte { get; set; }
        public decimal difInfonBimAnteriorUnidades { get; set; }
        public decimal difInfonBimAnteriorImporte { get; set; }
        public decimal difInfonMesAnteriorUnidades { get; set; }
        public decimal difInfonMesAnteriorImporte { get; set; }
        public decimal sueldoGarantizadoUnidades { get; set; }
        public decimal sueldoGarantizadoImporte { get; set; }
        public decimal infonavitRepetitivosUnidades { get; set; }
        public decimal infonavitRepetitivosImporte { get; set; }
        public decimal saldoRepetitivoNoCobradoUnidades { get; set; }
        public decimal saldoRepetitivoNoCobradoImporte { get; set; }
        public decimal porcentajeFiniquitoUnidades { get; set; }
        public decimal porcentajeFiniquitoImporte { get; set; }
        public decimal subsidioEmpleo2Importe { get; set; }
        public decimal calculoAnualUnidades { get; set; }
        public decimal calculoAnualImporte { get; set; }
        public decimal isptAcumulablesUnidades { get; set; }
        public decimal isptAcumulablesImporte { get; set; }
        public decimal repartoUtilidadesUnidades { get; set; }
        public decimal repartoUtilidadesImporte { get; set; }
        public decimal parteExentaUnidades { get; set; }
        public decimal parteExentaImporte { get; set; }
        public decimal parteExentaPrimaDominicalUnidades { get; set; }
        public decimal parteExentaPrimaDominicalImporte { get; set; }
        public decimal parteExentaFestivoTrabajadoUnidades { get; set; }
        public decimal parteExentaFestivoTrabajadoImporte { get; set; }
        public decimal netoPagar2Importe { get; set; }
        public decimal fondoAhorroPagado { get; set; }

        [ForeignKey("nominaID")]
        public virtual tblC_Nom_Nomina nomina { get; set; }
    }
}
