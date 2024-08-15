using Core.Enum.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas
{
    public class FacultamientosDTO
    {
        public int facultamiento_id { get; set; }
        public int usuario_id { get; set; }
        public string facultamientoNombre { get; set; }
        public string proyectos { get; set; }
        public TipoFacultamientoEnum tipo { get; set; }
        public string tipoDesc { get; set; }
        public bool mostrarModalCC { get; set; }
    }
}
