using Core.Entity.Administrativo.FacultamientosDpto;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.FacultamientosDpto
{
    public class ConceptoPlantillaMapping : EntityTypeConfiguration<tblFA_ConceptoPlantilla>
    {
        public ConceptoPlantillaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.plantillaID).HasColumnName("plantillaID");
            HasRequired(x => x.plantilla).WithMany(x => x.conceptosPlantilla).HasForeignKey(d => d.plantillaID);
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.esAutorizacion).HasColumnName("esAutorizacion");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblFA_ConceptoPlantilla");
        }
    }
}