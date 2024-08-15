using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class OrdenTrabajoDTO
    {
        public string Economico { get; set; }
        public string Modelo { get; set; }
        public string Obra { get; set; }
        public decimal Horometro { get; set; }
        public string Fecha { get; set; }
        public string Turno { get; set; }
        public string TipoParo1 { get; set; }
        public string TipoParo2 { get; set; }
        public string TipoParo3 { get; set; }
        public string MotivoParo { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }
        public string TiempoTotal { get; set; }
        public string TiempoReparacion { get; set; }
        public string TiempoMuerto { get; set; }
        public string DescripcionTM { get; set; }
        public string Comentarios { get; set; }

        public string TiempoHorasTotal { get; set; }
        public string TiempoHorasReparacion { get; set; }
        public string TiempoHorasMuerto { get; set; }
        public string TiempoMinutosTotal { get; set; }
        public string TiempoMinutosReparacion { get; set; }
        public string TiempoMinutosMuerto { get; set; }

    }
}
