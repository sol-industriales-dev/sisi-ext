using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento.DTO2._0
{
    public class getActividadModeloDTO
    {
        public int id { get; set; }
        public int orden { get; set; }
        public string descripcion { get; set; }
        public int idAct { get; set; }
        public string Tipo { get; set; }
        public int idTipo { get; set; }
        public int PM { get; set; }
        public tblM_DocumentosMaquinaria idformato { get; set; }
        public bool leyenda { get; set; }
        public bool Componente { get; set; }
        public decimal perioricidad { get; set; }
        public int DN { get; set; }
    }
}
