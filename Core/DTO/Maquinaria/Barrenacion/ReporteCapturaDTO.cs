using Core.Entity.Maquinaria.Barrenacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion
{
    public class ReporteCapturaDTO
    {
        public int id { get; set; }
        public string areaCuenta { get; set; }
        public string noEconomico { get; set; }
        public decimal horasTrabajadas { get; set; }
        public string turno { get; set; }
        public string operador { get; set; }
        public string ayudante { get; set; }
        public string tipoCaptura { get; set; }
        public string fechaCaptura { get; set; }
        public decimal metrosLineales { get; set; }
        public decimal metrosLinealesHora { get; set; }
        public List<tblB_DetalleCaptura> detalles { get; set; }
    }
}
