using Core.Entity.Administrativo.FacultamientosDpto;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.FacultamientosDpto
{
    public class FacultamientoMapping : EntityTypeConfiguration<tblFA_Facultamiento>
    {
        public FacultamientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.plantillaID).HasColumnName("plantillaID");
            HasRequired(x => x.plantilla).WithMany(x => x.facultamientos).HasForeignKey(d => d.plantillaID);
            Property(x => x.paqueteID).HasColumnName("paqueteID");
            HasRequired(x => x.paquete).WithMany(x => x.facultamientos).HasForeignKey(d => d.paqueteID);
            Property(x => x.aplica).HasColumnName("aplica");
            ToTable("tblFA_Facultamiento");
        }
    }
}