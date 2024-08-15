using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento.DTO2._0
{
    public class objRestLubricantesDTO
    {
        public int id { get; set; }
        public string componente { get; set; }
        public decimal VidaUtil { get; set; }
        public int componenteID { get; set; }
        public string comentario { get; set; }
        public decimal hrsAplico { get; set; }
        public decimal VidaRestante { get; set; }
        public int lubricanteID { get; set; }
        public List<cboLubricantesDTO> Suministros { get; set; }
        public bool reset { get; set; }
        public int idMant { get; set; }
        public decimal vidaActual { get; set; }
        public bool prueba { get; set; }

    }
}
