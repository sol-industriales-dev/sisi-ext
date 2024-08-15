using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class RNConceptoDTO
    {
        public int id { get; set; }
        public int idRNAgrupacion { get; set; }
        public string agrupacion { get; set; }
        public string descAgrupacion { get; set; }
        public int anio { get; set; }
        public int idCC { get; set; }
        public string concepto { get; set; }
        public decimal cantidad { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public bool esAgrupacion { get; set; }
        public bool esMatch { get; set; }
        public decimal total { get; set; }
    }
}
