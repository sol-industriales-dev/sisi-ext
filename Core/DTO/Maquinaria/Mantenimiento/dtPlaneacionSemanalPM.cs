using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class dtPlaneacionSemanalPM
    {
        public string economico { get; set; }
        public string tipoServicio { get; set; }
        public DateTime fechaProgramado { get; set; }

        public decimal horometroProgramado { get; set; }
        public DateTime fechaEjecutado { get; set; }
        public decimal horometroEjecutado { get; set; }
        public string observacion { get; set; }
        public decimal diferencia { get; set; }

        public decimal porError { get; set; }
        public int estatusPM { get; set; }

        public DateTime fechaPM { get; set; }
        public int idMant { get; set; }
        public int idModelo { get; set; }
        public int idPlaneador { get; set; }
        public List<ComboDTO> componentes { get; set; }
    }

}
