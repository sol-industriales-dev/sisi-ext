using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_Contrato
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public string Descripcion { get; set; }
        public int InstitucionId { get; set; }
        public int Plazo { get; set; }
        public DateTime FechaInicio { get; set; }
        public int FechaVencimientoTipoId { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public decimal Credito { get; set; }
        public decimal AmortizacionCapital { get; set; }
        public decimal IVA { get; set; }
        public decimal TasaInteres { get; set; }
        public decimal InteresMoratorio { get; set; }
        public decimal TipoCambio { get; set; }
        public bool Domiciliado { get; set; }
        public decimal? PagoInterino { get; set; }
        public decimal? PagoInterino2 { get; set; }
        public decimal? DepGarantia { get; set; }
        public string FileContrato { get; set; }
        public string FilePagare { get; set; }
        public bool Terminado { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModificacionId { get; set; }
        public string rfc { get; set; }

        public decimal montoOpcioncompra { get; set; }
        public int monedaContrato { get; set; }
        public string penaConvencional { get; set; }

        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public int? ctaLp { get; set; }
        public int? sctaLp { get; set; }
        public int? ssctaLp { get; set; }
        public int? digitoLp { get; set; }
        public string nombreCorto { get; set; }
        public int empresa { get; set; }
        public bool tasaFija { get; set; }
        public bool aplicaInteres { get; set; }
        public bool? aplicaContratoInteres { get; set; }

        public DateTime? fechaFirma { get; set; }
        public bool arrendamientoPuro { get; set; }
        [ForeignKey("InstitucionId")]
        public virtual tblAF_DxP_Institucion Institucion { get; set; }
        [ForeignKey("FechaVencimientoTipoId")]
        public virtual tblAF_DxP_TipoFechaVencimiento TipoFechaVencimiento { get; set; }
        [ForeignKey("UsuarioCreacionId")]
        public virtual tblP_Usuario UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioModificacionId")]
        public virtual tblP_Usuario UsuarioModificacion { get; set; }

        [ForeignKey("ContratoId")]
        public virtual ICollection<tblAF_DxP_ContratoDetalle> detalles { get; set; }

        public int ctaIA { get; set; }
        public int sctaIA { get; set; }
        public int ssctaIA { get; set; }
        public int digitoIA { get; set; }
    }
}
