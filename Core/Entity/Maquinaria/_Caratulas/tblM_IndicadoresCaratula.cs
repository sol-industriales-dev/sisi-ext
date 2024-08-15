using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria._Caratulas
{
    public class tblM_IndicadoresCaratula
    {
        public int id { get; set; }
        public int idCC { get; set; }
        [ForeignKey("idCC")]
        public virtual tblP_CC lstCC { get; set; }
        public bool moneda { get; set; }
        public bool manoObra { get; set; }
        public decimal auxiliar { get; set; }
        public decimal indirectos { get; set; }
        
    }
}
