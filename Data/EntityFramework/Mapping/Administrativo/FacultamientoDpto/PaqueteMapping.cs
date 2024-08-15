using Core.Entity.Administrativo.FacultamientosDpto;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.FacultamientoDpto
{
    public class PaqueteMapping : EntityTypeConfiguration<tblFA_Paquete>
    {
        public PaqueteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ccID).HasColumnName("ccID");
            HasRequired(x => x.cc).WithMany(x => x.paquetes).HasForeignKey(d => d.ccID);
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            ToTable("tblFA_Paquete");
        }
    }
}
