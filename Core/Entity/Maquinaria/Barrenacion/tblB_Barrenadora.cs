using Core.Entity.Maquinaria.Catalogo;
using Newtonsoft.Json;
using System;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_Barrenadora
    {
        public int id { get; set; }
        public bool piezasAsignadas { get; set; }
        public bool operadoresAsignados { get; set; }
        public bool activa { get; set; }
        public int maquinaID { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
        [JsonIgnore]
        public virtual tblM_CatMaquina maquina { get; set; }
    }
}
