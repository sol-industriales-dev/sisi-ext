using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class EjecutadoGeneralDTO
    {
        public string economicoID { get; set; }
        public decimal horometroUltCapturado { get; set; }
        public string fechaUltCapturado { get; set; }
        public int tipoPM { get; set; }
        public string fechaPM { get; set; }
        public decimal horometroPM { get; set; }
        public int personalRealizo { get; set; }
        public string observaciones { get; set; }
        public decimal horometroProy { get; set; }
        public string fechaProy { get; set; }
        public int tipoMantenimientoProy { get; set; }
        public bool actual { get; set; }
        public string fechaProyFin { get; set; }
        public string fechaCaptura { get; set; }
        public int idMaquina { get; set; }
        public bool estatus { get; set; }
        public int planeador { get; set; }
        public int UsuarioCap { get; set; }
        public bool actProg { get; set; }
        public int estadoMantenimiento { get; set; }
    }
}
