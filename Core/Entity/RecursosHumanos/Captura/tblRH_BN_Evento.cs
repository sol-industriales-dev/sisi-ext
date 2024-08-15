using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Evento
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int tipoNomina { get; set; }
        public string justificacion { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaAplicacion { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public int estatus { get; set; }
        public int periodo { get; set; }
        public bool aplicado { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}
