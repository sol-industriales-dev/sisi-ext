using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Captura
{
    public class CapHorometroDTO
    {
        public int id { get; set; }
        public string CC { get; set; }
        public string Economico { get; set; }
        public decimal HorasTrabajo { get; set; }
        public decimal Horometro { get; set; }
        public string HorometroDesc { get; set; }
        public decimal HorometroAcumulado { get; set; }
        public string HorometroAcumuladoDesc { get; set; }
        public decimal Desfase { get; set; }
        public decimal HorometroActual { get; set; }
        public string HorometroActualDesc { get; set; }
        public DateTime Fecha { get; set; }
        public bool Ritmo { get; set; }
        public int turno { get; set; }
        public bool habilidatado { get; set; }
        public decimal promedioHoras { get; set; }
        public string tipoRitmo { get; set; }
        public string fechaFormat { get; set; }
        public decimal maximoHoras { get; set; }

    }
}
