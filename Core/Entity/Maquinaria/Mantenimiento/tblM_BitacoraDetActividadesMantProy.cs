using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_BitacoraDetActividadesMantProy
    {
        public int id { get; set; }
        public int idAct { get; set; }
        public int modeloEquipoID { get; set; }
        public int idCompVis { get; set; }
        public int cantidad { get; set; }
        public string modelo { get; set; }
        public int idFiltro { get; set; }
        public bool aplicar { get; set; }
        public bool programado { get; set; }
        public int idMant { get; set; }
        public int tipoPMid { get; set; }

    }
}
