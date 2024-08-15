using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class StatusGeneralMnttoDTO
    {
        public string descripcion { get; set; }
        public int cantidadFrecuencias { get; set; }
        public decimal porcentajeRelativo { get; set; }
        public int tipoParo { get; set; }
        public string CC { get; set; }
        public string CCNombre { get; set; }
        public int Mes { get; set; }
        public decimal tiempo { get; set; }
        public decimal porcentajeTiempo { get; set; }
    }
}
