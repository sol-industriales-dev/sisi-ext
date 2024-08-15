using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Contabilidad.Nomina;

namespace Core.DTO.Contabilidad.Nomina
{
    public class CedulaCostosReporteDTO
    {
        public List<CedulaCostosDTO> tabla { get; set; }

        public string nombreEmpresa { get; set; }
        public string periodo { get; set; }        
        public decimal totalSumaNomina { get; set; }
        public decimal totalValesDespensa { get; set; }
        public decimal totalDepositoBancario { get; set; }
        public decimal totalDescuentos { get; set; }
        public decimal totalPrestamos { get; set; }
        public decimal totalFamsa { get; set; }
        public decimal totalFonacot { get; set; }
        public decimal totalSindicato { get; set; }
        public decimal totalPensionAlimenticia { get; set; }
        public decimal totalFondoAhorroEmpleado { get; set; }
        public decimal totalFondoAhorroEmpresa { get; set; }
        public decimal totalInfonavit { get; set; }
        public decimal totalSumasNomina { get; set; }
        public decimal comisionOCSI { get; set; }
        public decimal ivaComisionOCSI { get; set; }
        public decimal totalComisionOCSI { get; set; }
        public decimal totalOCSI { get; set; }
        public decimal totalAxa { get; set; }
        public string tipoBanco { get; set; }
        public decimal totalApoyoColectivo { get; set; }
    }
}
