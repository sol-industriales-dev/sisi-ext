using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class RepProvisionDTO
    {
        public int id { get; set; }
        public string Equipo { get; set; }
        public string NoEconomico { get; set; }
        public string Moneda { get; set; }
        public string Inicio { get; set; }
        public string Termino { get; set; }
        public decimal CostoRenta { get; set; }
        public string strCostoRenta { get; set; }
        public decimal PU { get; set; }
        public string strPU { get; set; }
        public decimal HrsFac { get; set; }
        public decimal HrsCons { get; set; }
        public decimal ImporteConsumido { get; set; }
        public string strImporteConsumido { get; set; }
        public decimal ImporteTotal { get; set; }
        public string strImporteTotal { get; set; }
        public string FacturaExtra { get; set; }
        public decimal HrsExtra { get; set; }
        public decimal PUHrsExtra { get; set; }
        public string strPUHrsExtra { get; set; }
        public decimal ImporteConsumidoExtra { get; set; }
        public string strImporteConsumidoExtra { get; set; }
        public decimal ImporteTotalExtra { get; set; }
        public string strImporteTotalExtra { get; set; }
        public string strAnotacion { get; set; }
    }
}
