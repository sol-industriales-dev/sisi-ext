using Core.Entity.Principal.Usuarios;
using Core.Enum.Maquinaria.Barrenacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_HistorialPieza
    {
        public int id { get; set; }
        public int piezaID { get; set; }
        [JsonIgnore]
        public virtual tblB_PiezaBarrenadora pieza { get; set; }
        public decimal horasAcumuladas { get; set; }
        public int barrenadoraID { get; set; }
        [JsonIgnore]
        public virtual tblB_Barrenadora barrenadora { get; set; }
        public DateTime fecha { get; set; }
        public TipoMovimientoPiezaEnum tipoMovimiento { get; set; }
        public string comentario { get; set; }
        public int usuarioID { get; set; }
        [JsonIgnore]
        public virtual tblP_Usuario usuario { get; set; }
        public decimal precio { get; set; }
    }
}
