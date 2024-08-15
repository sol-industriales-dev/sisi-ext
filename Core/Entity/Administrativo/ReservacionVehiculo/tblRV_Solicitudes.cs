using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;

namespace Core.Entity.Administrativo.ReservacionVehiculo
{
    public class tblRV_Solicitudes
    {
        public int id { get; set; }
        public DateTime fechaSalida { get; set; }
        public DateTime fechaEntrega { get; set; }
        public DateTime vigenciaLicencia { get; set; }
        public string motivo { get; set; }
        public string descripcion { get; set; }
        public string solicitante { get; set; }
        public bool autorizada { get; set; }
        public bool estatus { get; set; }

        public int? usuarioRegistroID { get; set; }
        public virtual tblP_Usuario usuarioRegistro { get; set; }
    }
}
