using Core.Enum.Administracion.Seguridad.MedioAmbiente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class CapturaDTO
    {
        public int id { get; set; }
        public string folio { get; set; }
        public string consecutivo { get; set; }
        public int tipoCaptura { get; set; }
        public string captura { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
        public string nomAgrupacion { get; set; }
        public int idResponsableTecnico { get; set; }
        public string responsableTecnico { get; set; }
        public DateTime fechaEntrada { get; set; }
        public decimal cantidadContenedor { get; set; }
        public string strCantidadContenedor { get; set; }
        public string tipoContenedor { get; set; }
        public string codigoContenedor { get; set; }
        public int consecutivoCodContenedor { get; set; }
        public int idAspectoAmbiental { get; set; }
        public int aspectoAmbientalID { get; set; }
        public string aspectoAmbiental { get; set; }
        public decimal cantidadResiduo { get; set; }
        public decimal cantidad { get; set; }
        public List<string> lstCodigosContenedores { get; set; }
        public List<int> lstAspectosAmbientalesID { get; set; }
        public List<decimal> lstCantidadAspectosAmbientales { get; set; }
        public string plantaProcesoGeneracion { get; set; }
        public string tratamiento { get; set; }
        public string manifiesto { get; set; }
        public DateTime fechaEmbarque { get; set; }
        public string tipoTransporte { get; set; }
        public int idTransportistaTrayecto { get; set; }
        public DateTime fechaDestinoFinal { get; set; }
        public int idTransportistaDestinoFinal { get; set; }
        public int estatusCaptura { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
        public HttpPostedFileBase evidencia { get; set; }
        public string razonSocial { get; set; }
        public string strEstatusCaptura { get; set; }
        public int _idCaptura { get; set; }
        public int _tipoArchivo { get; set; }
        public DateTime mesInicio { get; set; }
        public DateTime mesFinal { get; set; }
        public int unidad { get; set; }
        public string unidadMedida { get; set; }
        public decimal cantRegistros { get; set; }
    }
}