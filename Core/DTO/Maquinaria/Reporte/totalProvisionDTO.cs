using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class totalProvisionDTO
    {
        public decimal TotalDlls { get; set; }
        public decimal TC { get; set; }
        public decimal TotalPesos { get; set; }
        public decimal TotalMN { get; set; }
        public string strTotalDlls { get; set; }
        public string strTC { get; set; }
        public string strTotalPesos { get; set; }
        public string strTotalMN { get; set; }
    }
}
