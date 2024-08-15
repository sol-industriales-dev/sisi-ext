using Core.Entity.Maquinaria.Captura;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    class CapturaNominaMensualCCMapping : EntityTypeConfiguration<tblM_CapNominaMensualCC>
    {
        public CapturaNominaMensualCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuentaID).HasColumnName("areaCuentaID");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.año).HasColumnName("año");
            Property(x => x.horasHombreTotales).HasColumnName("horasHombreTotales");
            Property(x => x.nominaIMSS).HasColumnName("nominaIMSS");
            Property(x => x.nominaInfonavit).HasColumnName("nominaInfonavit");
            Property(x => x.ISN).HasColumnName("ISN");
            Property(x => x.ISR).HasColumnName("ISR");
            Property(x => x.usuarioCreaID).HasColumnName("usuarioCreaID");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioEditaID).HasColumnName("usuarioEditaID");
            Property(x => x.fechaEdicion).HasColumnName("fechaEdicion");
            Property(x => x.completo).HasColumnName("completo");
            ToTable("tblM_CapNominaMensualCC");
        }
    }
}
