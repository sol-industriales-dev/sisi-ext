using Core.Entity.GestorArchivos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Data.EntityFramework.Mapping.GestorArchivos
{
    class tblGA_AccesoDepartamentoMapping : EntityTypeConfiguration<tblGA_AccesoDepartamento>
    {
        public tblGA_AccesoDepartamentoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany(x => x.accesoDepartamentos).HasForeignKey(d => d.usuarioID);
            Property(x => x.departamentoID).HasColumnName("departamentoID");
            HasRequired(x => x.departamento).WithMany(x => x.accesosDepartamentos).HasForeignKey(d => d.departamentoID);
            ToTable("tblGA_AccesoDepartamento");
        }
    }
}
