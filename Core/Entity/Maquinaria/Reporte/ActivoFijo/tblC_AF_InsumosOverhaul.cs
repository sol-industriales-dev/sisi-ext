using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_InsumosOverhaul
    {
        public int Id { get; set; }
        public string Insumo { get; set; }
        public string Descripcion { get; set; }
        public decimal? Porcentaje { get; set; }
        public int Meses { get; set; }
        public bool Estatus { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual tblP_Usuario Usuario { get; set; }
    }
}
