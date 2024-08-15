using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class CedulaCostosDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public decimal sumaNomina { get; set; }
        public decimal valesDespensa { get; set; }
        public decimal depositoBancario { get; set; }
        public decimal descuentos { get; set; }
        public decimal prestamos { get; set; }
        public decimal famsa { get; set; }
        public decimal fonacot { get; set; }
        public decimal sindicato { get; set; }
        public decimal pensionAlimenticia { get; set; }
        public decimal fondoAhorroEmpleado { get; set; }
        public decimal fondoAhorroEmpresa { get; set; }
        public decimal infonavit { get; set; }
        public decimal sumasNomina { get; set; }
        public decimal axa { get; set; }
        public decimal apoyoColectivo { get; set; }
    }
}
