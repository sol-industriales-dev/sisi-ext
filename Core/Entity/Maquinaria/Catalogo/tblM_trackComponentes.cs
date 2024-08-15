using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Core.Entity.Maquinaria.Overhaul;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_trackComponentes
    {
        public int id { get; set; }
        public int componenteID { get; set; }
        public bool ? tipoLocacion { get; set; }
        public int ? maquinariaID { get; set; }
        public int ? locacionID { get; set; }
        public DateTime ? fecha { get; set; }
        public int estatus { get; set; }
        public string JsonFechasCRC { get; set; }
        public string JsonArchivos { get; set; }
        public string locacion { get; set; }
        public bool reciclado { get; set; }
        public decimal costoCRC { get; set; }
        public decimal horasAcumuladas { get; set; }
        public decimal horasCiclo { get; set; }
        [JsonIgnore]
        public virtual tblM_CatComponente componente { get; set; }
    }
}