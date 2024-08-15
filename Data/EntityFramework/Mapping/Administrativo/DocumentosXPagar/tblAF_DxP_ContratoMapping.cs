using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_ContratoMapping : EntityTypeConfiguration<tblAF_DxP_Contrato>
    {
        public tblAF_DxP_ContratoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Folio).HasColumnName("folio");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            Property(x => x.InstitucionId).HasColumnName("institucionId");
            Property(x => x.Plazo).HasColumnName("plazo");
            Property(x => x.FechaInicio).HasColumnName("fechaInicio");
            Property(x => x.FechaVencimientoTipoId).HasColumnName("fechaVencimientoTipoId");
            Property(x => x.FechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.Credito).HasPrecision(24, 2).HasColumnName("credito");
            Property(x => x.AmortizacionCapital).HasPrecision(24, 2).HasColumnName("amortizacionCapital");
            Property(x => x.IVA).HasPrecision(24, 6).HasColumnName("iva");
            Property(x => x.TasaInteres).HasPrecision(24, 2).HasColumnName("tasaInteres");
            Property(x => x.InteresMoratorio).HasPrecision(24, 2).HasColumnName("interesMoratorio");
            Property(x => x.PagoInterino).HasPrecision(24, 2).HasColumnName("pagoInterino");
            Property(x => x.PagoInterino2).HasPrecision(24, 2).HasColumnName("pagoInterino2");
            Property(x => x.DepGarantia).HasPrecision(24, 2).HasColumnName("depGarantia");
            Property(x => x.TipoCambio).HasPrecision(24, 4).HasColumnName("tipoCambio");
            Property(x => x.Domiciliado).HasColumnName("domiciliado");
            Property(x => x.FileContrato).HasColumnName("fileContrato");
            Property(x => x.FilePagare).HasColumnName("filePagare");
            Property(x => x.Terminado).HasColumnName("terminado");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.UsuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.FechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.UsuarioModificacionId).HasColumnName("usuarioModificacionId");

            Property(x => x.montoOpcioncompra).HasColumnName("montoOpcioncompra");
            Property(x => x.monedaContrato).HasColumnName("monedaContrato");
            Property(x => x.penaConvencional).HasColumnName("penaConvencional");

            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.digito).HasColumnName("digito");
            Property(x => x.ctaLp).HasColumnName("ctaLp");
            Property(x => x.sctaLp).HasColumnName("sctaLp");
            Property(x => x.ssctaLp).HasColumnName("ssctaLp");
            Property(x => x.digitoLp).HasColumnName("digitoLp");
            Property(x => x.nombreCorto).HasColumnName("nombreCorto");

            Property(x => x.fechaFirma).HasColumnName("fechaFirma");
            Property(x => x.empresa).HasColumnName("empresa");

            Property(x => x.ctaIA).HasColumnName("ctaIA");
            Property(x => x.sctaIA).HasColumnName("sctaIA");
            Property(x => x.ssctaIA).HasColumnName("ssctaIA");
            Property(x => x.digitoIA).HasColumnName("digitoIA");

            HasRequired(x => x.Institucion).WithMany().HasForeignKey(y => y.InstitucionId);
            HasRequired(x => x.UsuarioCreacion).WithMany().HasForeignKey(y => y.UsuarioCreacionId);
            HasRequired(x => x.UsuarioModificacion).WithMany().HasForeignKey(y => y.UsuarioModificacionId);
            ToTable("tblAF_DxP_Contrato");
        }
    }
}
