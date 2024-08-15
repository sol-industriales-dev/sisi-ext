using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.ReporteEmpleadoCC
{
    public class ReporteEmpleadoCCDTO
    {
        //JOIN
        public int numEmpleados { get; set; }
        //
        public string anioNom { get; set; }
        public string mesNom { get; set; }
        public string tipoNominaNom { get; set; }
        public string tipoNominaDesc { get; set; }
        public string periodoNom { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string ccNom { get; set; }
        public string ccDesc { get; set; }
        public string numeroEmpleado { get; set; }
        public string nombreCompleto { get; set; }
        public string sueldoUnidades { get; set; }
        public string sueldoImporte { get; set; }
        public string septimoDiaUnidades { get; set; }
        public string septimoDiaImporte { get; set; }
        public string horasExtraDoblesImporte { get; set; }
        public string horasExtrasTriplesImporte { get; set; }
        public string primaDominicalImporte { get; set; }
        public string ansoFestivoTrabajadoImporte { get; set; }
        public string incapacidadEnfermedad3DiasImporte { get; set; }
        public string premiosAsistenciaImporte { get; set; }
        public string bonoPuntualidadImporte { get; set; }
        public string permisosConGoceImporte { get; set; }
        public string despensa2Importe { get; set; }
        public string fondoAhorroEmpresaImporte { get; set; }
        public string compensacionIsptImssImporte { get; set; }
        public string vacacionTrabajadaImporte { get; set; }
        public string vacacionFiniquitoImporte { get; set; }
        public string vacacionDisfrutadaImporte { get; set; }
        public string primaVacacionalImporte { get; set; }
        public string primaVacacional2Importe { get; set; }
        public string aguinaldoGravadoImporte { get; set; }
        public string aguinaldoExentoImporte { get; set; }
        public string primaAntiguedadImporte { get; set; }
        public string indemnizacion3MesesImporte { get; set; }
        public string indemnizacion20DiasImporte { get; set; }
        public string indemnizacionOtrosImporte { get; set; }
        public string repartoUtilidadGraImporte { get; set; }
        public string repartoUtilidadExeImporte { get; set; }
        public string prestamoAhorroImporte { get; set; }
        public string fondoAhorroEmpresa2Importe { get; set; }
        public string fondoAhorroEmpleadoImporte { get; set; }
        public string interesPorFondoAhorroImporte { get; set; }
        public string diferenciaCategoriaImporte { get; set; }
        public string bonosImporte { get; set; }
        public string retroactivoDiversoImporte { get; set; }
        public string comisionesSueldoImporte { get; set; }
        public string incentivoProductivoImporte { get; set; }
        public string gratificacionEspecialImporte { get; set; }
        public string prevSocialImporte { get; set; }
        public string prevSocialAlimentosImporte { get; set; }
        public string prevSocialHabitacionImporte { get; set; }
        public string otrasPercepcionesImporte { get; set; }
        public string compensacionUnicaExtraordinariaImporte { get; set; }
        public string bonoProduccionImporte { get; set; }
        public string despensaImporte { get; set; }
        public string previsionSocialImporte { get; set; }
        public string altoCostoVidaImporte { get; set; }
        public string despensa3Importe { get; set; }
        public string prevSocialAlimentos2Importe { get; set; }
        public string altoCostoVida2Importe { get; set; }
        public string subsidioEmpleoImporte { get; set; }
        public string totalPercepciones { get; set; }
        public string isrImporte { get; set; }
        public string imssImporte { get; set; }
        public string isptCompImporte { get; set; }
        public string cuotaSindicalImporte { get; set; }
        public string cuotaSindicalExtraImporte { get; set; }
        public string infonavitImporte { get; set; }
        public string fonacotImporte { get; set; }
        public string pensionAlimenticiaImporte { get; set; }
        public string vejezSarImporte { get; set; }
        public string descuentosAlimentosImporte { get; set; }
        public string descuentosAlimentosPendImporte { get; set; }
        public string adeudoEmpresaImporte { get; set; }
        public string interesesPrestamoImporte { get; set; }
        public string fondoAhorroEmpleado2Importe { get; set; }
        public string fondoAhorroEmpresa3Importe { get; set; }
        public string infonavitMesAnteriorImporte { get; set; }
        public string anticiposDiversosImporte { get; set; }
        public string isptSaldoAnteriorImporte { get; set; }
        public string descuentosImporte { get; set; }
        public string famsaImporte { get; set; }
        public string prestamosImporte { get; set; }
        public string axaImporte { get; set; }
        public string ajusteNominaImporte { get; set; }
        public string totalDeducciones { get; set; }
        public string netoPagar { get; set; }
        public decimal total { get; set; }
        public string fondoAhorroPagado { get; set; }
    }
}
