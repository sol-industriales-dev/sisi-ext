using Core.DTO;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionIHHInteresado : InfoRegistroDTO
    {
        public int id { get; set; }
        public int colaboradorAdiestradorId { get; set; }
        public int interesadoId { get; set; }

        [ForeignKey("colaboradorAdiestradorId")]
        public virtual tblS_CapacitacionIHHColaboradorCapacitacion colaboradorAdiestrador { get; set; }

        [ForeignKey("interesadoId")]
        public virtual tblP_Usuario interesado { get; set; }
    }
}
