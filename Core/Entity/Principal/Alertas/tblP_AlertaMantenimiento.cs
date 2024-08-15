using System;

namespace Core.Entity.Principal.Alertas
{
    public class tblP_AlertaMantenimiento
    {
        public int id { get; set; }
        public string mensaje { get; set; }
        public bool activo { get; set; }
        public DateTime fechaProgramada { get; set; }
    }
}
