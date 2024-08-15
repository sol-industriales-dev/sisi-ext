using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class RentabilidadCostoHoraDTO
    {
        public string noEconomico { get; set; }
        public string modelo { get; set; }
        public decimal horasTrabajadas { get; set; }
        public decimal materialesLubricacion { get; set; }
        public decimal refacciones { get; set; }
        public decimal herramientas { get; set; }
        public decimal combustibles { get; set; }
        public decimal talleresExternos { get; set; }
        public decimal servicios { get; set; }
        public decimal serviciosAdministrativos { get; set; }
        public decimal subcontratos { get; set; }
        public decimal fletes { get; set; }
        public decimal traspasoMM { get; set; }
        public decimal rentaMaquinaria { get; set; }
        public decimal intereses { get; set; }
        public decimal totalCostoHorario { get; set; }        
        public decimal total { get; set; }
        public List<RentabilidadDTO> detalles { get; set; }
    }
}
