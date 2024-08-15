using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_Inspecciones
    {
        public int id { get; set; }
        public string areaCuenta { get; set; }
        public int periodo { get; set; }
        public int idGrupo { get; set; }
        public string noEconomico { get; set; }
        public decimal horometro { get; set; }
        public int idCatMaquina { get; set; }
        public DateTime fechaInicioInsp { get; set; }
        public DateTime fechaFinalInsp { get; set; }
        public DateTime fechaInspRealizada { get; set; }
        public int cantBackLogs { get; set; }
        public DateTime fechaCreacionInsp { get; set; }
        public DateTime fechaModificacionInsp { get; set; }
        public bool esActivo { get; set; }
        public List<DateTime> lstFechaInspRealizada { get; set; }
        public List<string> lstNoEconomicos { get; set; }
        public List<int> lstCantBackLogs { get; set; }
    }
}
