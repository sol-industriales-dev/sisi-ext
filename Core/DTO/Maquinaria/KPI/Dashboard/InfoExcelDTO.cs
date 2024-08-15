using Core.Enum.Maquinaria.KPI.CatalogoCodigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class InfoExcelDTO
    {
        public int idTurno { get; set; }
        public string turnoDescripcion { get; set; }
        public Tipo_ParoEnum tipoParo { get; set; }
        public string codigo { get; set; }
        public string codigoDescripcion { get; set; }
        public decimal valor { get; set; }
        public int idEconomico { get; set; }
        public string noEconomico { get; set; }
        public int idGrupo { get; set; }
        public string grupoDescripcion { get; set; }
        public int idModelo { get; set; }
        public string modeloDescripcion { get; set; }
    }
}
