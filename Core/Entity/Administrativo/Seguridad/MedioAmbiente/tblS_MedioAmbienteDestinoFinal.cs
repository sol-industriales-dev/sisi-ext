using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.MedioAmbiente
{
    public class tblS_MedioAmbienteDestinoFinal
    {
        public int id { get; set; }
        public int idAgrupacion { get; set; }
        public int idAspectoAmbiental { get; set; }
        public DateTime fechaDestinoFinal { get; set; }
        public int idTransportistaDestinoFinal { get; set; }
        public int idArchivoTrayecto { get; set; }
        public decimal cantidad { get; set; }
        public string aaID { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
