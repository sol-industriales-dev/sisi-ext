using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.KPI
{
    public class tblM_KPI_Homologado
    {
        public int id { get; set; }
        public int año { get; set; }
        public int semana { get; set; }
        public DateTime fecha { get; set; }
        public int turno { get; set; }
        public string ac { get; set; }
        public string economico { get; set; }
        public string codigoParo { get; set; }
        public decimal valor { get; set; }
        public int idParo { get; set; }
        public int idGrupo { get; set; }
        public int idModelo { get; set; }
        public int idEconomico { get; set; }
        public authEstadoEnum authEstado { get; set; }
        public bool activo { get; set; }
        public int idTipoParo { get; set; }
        public bool validado { get; set; }
    }
}
