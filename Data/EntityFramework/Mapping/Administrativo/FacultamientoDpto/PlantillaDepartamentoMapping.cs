using Core.Entity.Administrativo.FacultamientosDpto;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.FacultamientosDpto
{
    public class PlantillaDepartamentoMapping : EntityTypeConfiguration<tblFA_PlantillatblFA_Grupos>
    {
        public PlantillaDepartamentoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.plantillaID).HasColumnName("plantillaID");
            HasRequired(x => x.plantilla).WithMany(x => x.plantillasDepartamento).HasForeignKey(d => d.plantillaID);
            Property(x => x.grupoID).HasColumnName("grupoID");
            HasRequired(x => x.grupo).WithMany(x => x.plantillasFaDepartamento).HasForeignKey(d => d.grupoID);
            ToTable("tblFA_PlantillatblFA_Grupos");
        }
    }
}