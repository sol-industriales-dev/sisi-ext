using Core.Entity.GestorArchivos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.GestorArchivos
{
    public class tblGA_DirectorioMapping : EntityTypeConfiguration<tblGA_Directorio>
    {
        public tblGA_DirectorioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.padreID).HasColumnName("padreID");
            Property(x => x.nivel).HasColumnName("nivel");
            Property(x => x.esCarpeta).HasColumnName("esCarpeta");
            Property(x => x.departamentoID).HasColumnName("departamentoID");
            HasRequired(x => x.departamento).WithMany(x => x.listaDirectorios).HasForeignKey(d => d.departamentoID);
            ToTable("tblGA_Directorio");
        }
    }
}
