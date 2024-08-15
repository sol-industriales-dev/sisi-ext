using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class ProgramaInspTMCDTO
    {
        public int id { get; set; }
        public string areaCuenta { get; set; }
        public int periodo { get; set; }
        public string noEconomico { get; set; }
        public bool esRehabilitar { get; set; }
        public int idMotivo { get; set; }
        public string motivo { get; set; }
        public string descripcion { get; set; }
        public string modelo { get; set; }
        public decimal horas { get; set; }
        public DateTime fechaRequerido { get; set; }
        public DateTime fechaCreacionInsp { get; set; }
        public DateTime fechaModificacionInsp { get; set; }
        public bool esActivo { get; set; }
        public int tipoMaquina { get; set; }
        public bool noEconomicoEnPrograma { get; set; } //ECONOMICO YA EN REHABILITACIÓN
        public int partida { get; set; }
        public DateTime fechaProgramacion { get; set; }
        public DateTime fechaPromesa { get; set; }
    }
}
