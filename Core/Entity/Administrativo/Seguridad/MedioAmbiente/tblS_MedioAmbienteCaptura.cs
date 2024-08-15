using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.MedioAmbiente;

namespace Core.Entity.Administrativo.Seguridad.MedioAmbiente
{
    public class tblS_MedioAmbienteCaptura
    {
        public int id { get; set; }
        public string folio { get; set; }
        public string consecutivo { get; set; }
        public int tipoCaptura { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
        public int idResponsableTecnico { get; set; }
        public DateTime fechaEntrada { get; set; }
        public decimal cantidadContenedor { get; set; }
        public string tipoContenedor { get; set; }
        public string plantaProcesoGeneracion { get; set; }
        public string tratamiento { get; set; }
        public string manifiesto { get; set; }
        public DateTime? fechaEmbarque { get; set; }
        public string tipoTransporte { get; set; }
        public int? idTransportistaTrayecto { get; set; }
        public DateTime? fechaDestinoFinal { get; set; }
        public int? idTransportistaDestinoFinal { get; set; }
        public int estatusCaptura { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
