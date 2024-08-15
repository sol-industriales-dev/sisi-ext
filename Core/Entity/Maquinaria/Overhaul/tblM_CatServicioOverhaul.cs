using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity.Maquinaria.Catalogo;
using Newtonsoft.Json;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CatServicioOverhaul
    {
        public int id { get; set; }
        public int tipoServicioID { get; set; }
        public int maquinaID { get; set; }
        public string centroCostos { get; set; }
        public decimal cicloVidaHoras { get; set; }
        public decimal horasCicloActual { get; set; }
        public DateTime fechaAsignacion { get; set; }
        public DateTime ? fechaAplicacion { get; set; }
        public bool estatus { get; set; }
        [JsonIgnore]
        public virtual tblM_CatMaquina maquina { get; set; }
        [ForeignKey("tipoServicioID")]
        public virtual tblM_CatTipoServicioOverhaul servicio { get; set; }
    }
}