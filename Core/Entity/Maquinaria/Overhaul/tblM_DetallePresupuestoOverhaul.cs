using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_DetallePresupuestoOverhaul
    {
        public int id { get; set; }
        public int componenteID { get; set; }
        public int maquinaID { get; set; }
        public int estado { get; set; }
        public decimal costoSugerido { get; set; }
        public decimal costoPresupuesto { get; set; }
        public decimal costoReal { get; set; }
        public decimal horasCiclo { get; set; }
        public decimal horasAcumuladas { get; set; }
        public int presupuestoID { get; set; }
        public DateTime fecha { get; set; }
        public int tipo { get; set; }
        public int subconjuntoID { get; set; }
        public string obra { get; set; }
        public int vida { get; set; }
        public string comentarioAumento { get; set; }
        public bool programado { get; set; }
        public bool esServicio { get; set; }
        [JsonIgnore]
        public virtual tblM_CatComponente componente { get; set; }
        [JsonIgnore]
        public virtual tblM_CatMaquina maquina { get; set; }
        [JsonIgnore]
        public virtual tblM_PresupuestoOverhaul presupuesto { get; set; }
    }
}