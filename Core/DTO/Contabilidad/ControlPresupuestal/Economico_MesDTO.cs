using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.ControlPresupuestal
{
    public class Economico_MesDTO
    {
        public string noEconomico { get; set; }
        public int divisionID { get; set; }
        public int acID { get; set; }
        public string ac { get; set; }
        public int modeloID { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int conceptoID { get; set; }
        public string concepto { get; set; }
        public decimal horas { get; set; }
        public decimal caratula_tc { get; set; }
        public decimal caratula_moneda { get; set; }
        public decimal caratula_monto { get; set; }
        public decimal presupuesto { get; set; }
        public decimal real { get; set; }
        public decimal total { get; set; }
    }
}
