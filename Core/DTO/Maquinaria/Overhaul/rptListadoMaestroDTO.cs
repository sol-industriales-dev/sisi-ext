using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class rptListadoMaestroDTO
    {
        public int idPlaneacionOH { get; set; }
        public int idMaquina { get; set; }
        public string equipo { get; set; }
        public string cc { get; set; }
        public string obra { get; set; }
        public int ritmo { get; set; }
        public decimal hrsComponente { get; set; }
        public decimal target { get; set; }
        public string fechaPCR { get; set; }
        public string tipo { get; set; }
    }
}