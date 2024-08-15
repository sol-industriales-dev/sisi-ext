using Core.Enum.Maquinaria.Barrenacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_DetalleCaptura
    {
        public int id { get; set; }
        public decimal bordo { get; set; }
        public decimal espaciamiento { get; set; }
        public int barrenos { get; set; }
        public decimal profundidad { get; set; }
        public string banco { get; set; }
        public decimal densidadMaterial { get; set; }
        public TipoBarrenoEnum tipoBarreno { get; set; }
        public decimal subbarreno { get; set; }
        public int capturaID { get; set; }
        public virtual tblB_CapturaDiaria captura { get; set; }
    }
}
