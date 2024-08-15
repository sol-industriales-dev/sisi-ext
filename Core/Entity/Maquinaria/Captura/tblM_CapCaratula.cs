using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapCaratula
    {
        public int id { get; set; }
        public int idCaratula { get; set; }
        public int idGrupo { get; set; }
        public int idModelo { get; set; }
        public string equipo { get; set; }
        public decimal costo { get; set; }
        public int unidad { get; set; }
        [ForeignKey("idCaratula")]
        public virtual tblM_EncCaratula EncCaratula { get; set; }
        public bool activo { get; set; }

        public decimal cargoFijo { get; set; }
        public decimal cOverhaul { get; set; }
        public decimal cMttoCorrectivo { get; set; }
        public decimal cCombustible { get; set; }
        public decimal cAceites { get; set; }
        public decimal cFiltros { get; set; }
        public decimal cAnsul { get; set; }
        public decimal cCarrileria { get; set; }
        public decimal cLlantas { get; set; }
        public decimal cHerramientasDesgaste { get; set; }
        public decimal cCargoOperador { get; set; }
        public decimal cPersonalMtto { get; set; }
    }
}
