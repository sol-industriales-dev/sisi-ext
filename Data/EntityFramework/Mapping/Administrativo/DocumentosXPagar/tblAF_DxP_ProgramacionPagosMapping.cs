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
    public class tblAF_DxP_ProgramacionPagosMapping : EntityTypeConfiguration<tblAF_DxP_ProgramacionPagos>
    {

        public tblAF_DxP_ProgramacionPagosMapping()
        {

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.contratoid).HasColumnName("contratoid");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.mensualidad).HasColumnName("mensualidad");
            Property(x => x.financiamiento).HasColumnName("financiamiento");
            Property(x => x.fechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.capital).HasColumnName("capital");
            Property(x => x.intereses).HasColumnName("intereses");
            Property(x => x.iva).HasColumnName("iva");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.porcentaje).HasPrecision(24, 6).HasColumnName("porcentaje");
            Property(x => x.tipoCambio).HasPrecision(18, 4).HasColumnName("tipoCambio");
            Property(x => x.importeDLLS).HasColumnName("importeDLLS");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.aplicado).HasColumnName("aplicado");
            Property(x => x.financiamiento).HasColumnName("financiamiento");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.parcialidad).HasColumnName("parcialidad");
            Property(x => x.ivaInteres).HasColumnName("ivaInteres");

            Property(x => x.liquidar).HasColumnName("liquidar");
            Property(x => x.penaConvencional).HasColumnName("penaConvencional");
            Property(x => x.opcionCompra).HasColumnName("opcionCompra");
            Property(x => x.montoOpcionCompra).HasColumnName("montoOpcionCompra");
            Property(x => x.maquinaId).HasColumnName("maquinaId");

            ToTable("tblAF_DxP_ProgramacionPagos");
        }
    }
}
