using Core.Entity.Administrativo.Comercializacion.CRM;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.Comercializacion.CRM
{
    public class tblAF_CRM_ProximasAccionesDetalleMapping : EntityTypeConfiguration<tblAF_CRM_ProximasAccionesDetalle>
    {
        public tblAF_CRM_ProximasAccionesDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblAF_CRM_ProximasAccionesDetalle");
        }
    }
}