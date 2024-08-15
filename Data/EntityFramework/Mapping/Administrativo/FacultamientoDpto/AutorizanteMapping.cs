using Core.Entity.Administrativo.FacultamientosDpto;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.FacultamientoDpto
{
    public class AutorizanteMapping : EntityTypeConfiguration<tblFA_Autorizante>
    {
        public AutorizanteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.esAutorizante).HasColumnName("esAutorizante");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany(x => x.listaAutorizantesFa).HasForeignKey(d => d.usuarioID);
            Property(x => x.autorizado).HasColumnName("autorizado");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.paqueteID).HasColumnName("paqueteID");
            HasRequired(x => x.paquete).WithMany(x => x.autorizantes).HasForeignKey(d => d.paqueteID);
            Property(x => x.firma).HasColumnName("firma");
            ToTable("tblFA_Autorizante");
        }
    }
}
