using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class KPIDTO
    {
        public int id { get; set; }
        public string btnEquipo { get; set; }
        public string economico { get; set; }
        public string horasIdealMensual { get; set; }
        public string pDisponibilidad { get; set; }
        public string horasTrabajado { get; set; }
        public string horasParo { get; set; }
        public string pMProgramadoTiempo { get; set; }
        public string pMProgramadoCantidad { get; set; }
        public string pPreventivoHoras { get; set; }
        public string pCorrectivoHoras { get; set; }
        public string pPredictivoHoras { get; set; }
        public string horasHombre { get; set; }
        public string MTBS { get; set; }
        public string MTTR { get; set; }
        public string pUtilizacion { get; set; }
        public string parosPrincipal1 { get; set; }
        public string parosPrincipal2 { get; set; }
        public string parosPrincipal3 { get; set; }
        public decimal THT { get; set; }
        public decimal THP { get; set; }
        public decimal TPD { get; set; }
        public int idGrupoMaquinaria { get; set; }
        public decimal promedioDisponibilidad { get; set; }
        public string nombreGrupoMaquinaria { get; set; }
    }
}
