using Core.Enum.Maquinaria.Barrenacion;
using Newtonsoft.Json;
using System;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_ManoObra
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public TipoOperadorEnum tipoOperador { get; set; }
        public int turno { get; set; }
        public DateTime fechaAlta { get; set; }
        public DateTime? fechaBaja { get; set; }
        public bool activo { get; set; }
        public int barrenadoraID { get; set; }

        public decimal sueldo { get; set; }
        public int jornada { get; set; }
        public int fsr { get; set; }

        [JsonIgnore]
        public virtual tblB_Barrenadora barrenadora { get; set; }
        public int usuarioCreadorID { get; set; }
    }
}
