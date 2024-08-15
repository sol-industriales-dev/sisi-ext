using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Caratulas
{
    public class tblM_Caratulas
    {
        
        public int id { get; set; }
        public int idGrupo { get; set; }
        [ForeignKey("idGrupo")]
        public virtual tblM_CatGrupoMaquinaria lstCatGrupo { get; set; }
        public int idModelo { get; set; }
        [ForeignKey("idModelo")]
        public virtual tblM_CatModeloEquipo lstCatModelo { get; set; }
        public int idCC { get; set; }
        [ForeignKey("idCC")]
        public virtual tblP_CC lstCC { get; set; }
        public decimal depreciacion { get; set; }
        public decimal inversion { get; set; }
        public decimal seguro { get; set; }
        public decimal filtros { get; set; }
        public decimal manoObra { get; set; }
        public decimal auxiliar { get; set; }
        public decimal indirectosMatriz { get; set; }
        public decimal depreciacionOH { get; set; }
        public decimal aceite { get; set; }
        public decimal carilleria { get; set; }
        public decimal ansul { get; set; }
        public decimal utilidad { get; set; }
        public decimal costoTotal { get; set; }
        public bool esActivo { get; set; }
        public decimal mantenimientoCo { get; set; }
        public int tipoHoraDia { get; set; }

    }
}
