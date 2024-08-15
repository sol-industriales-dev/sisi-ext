using Core.Entity.Maquinaria.Barrenacion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class CatalogoBancoMapping : EntityTypeConfiguration<tblB_CatalogoBanco>
    {
        public CatalogoBancoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            ToTable("tblB_CatalogoBanco");
        }
    }
}



