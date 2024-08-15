using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.Colombia
{
    public class tblC_AF_SubcuentaColombia
    {
        public int id { get; set; }
        public int cuentaId { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string descripcion { get; set; }
        public bool esMaquinaria { get; set; }
        public int? tipoMaquinaId { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("cuentaId")]
        public virtual tblC_AF_CuentaColombia cuentaColombia { get; set; }

        [ForeignKey("tipoMaquinaId")]
        public virtual tblM_CatTipoMaquinaria tipoMaquina { get; set; }
    }
}
