using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.ControlPresupuestal
{
    public class FiltrosControlPresupuestalDTO
    {
        public int area { get; set; }
        public int cuenta { get; set; }
        public string areaCuenta { get; set; }
        public int tipo { get; set; }
        public List<int> listaGrupos { get; set; }
        public List<int> listaModelos { get; set; }
        public List<string> listaEconomico { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public int TipoBusqueda { get; set; }
        public bool acumulado { get; set; }
        public int tipoHoraDia { get; set; }
    }
}
