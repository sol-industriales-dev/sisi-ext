using Core.Entity.SeguimientoAcuerdos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class SeguimientoAcuerdosDTO
    {
        public tblSA_Minuta objMinuta { get; set; }
        public List<tblSA_Actividades> objActividades { get; set; }
        public List<tblSA_Comentarios> objComentarios { get; set; }
    }
}
