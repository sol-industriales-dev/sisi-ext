using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.GestorArchivos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.GestorArchivos
{
    public class tblGA_VistasMapping : EntityTypeConfiguration<tblGA_Vistas>
    {
        public tblGA_VistasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany(x => x.vistasDirectorios).HasForeignKey(d => d.usuarioID);
            Property(x => x.directorioID).HasColumnName("directorioID");
            HasRequired(x => x.directorio).WithMany(x => x.vistas).HasForeignKey(d => d.directorioID);
            Property(x => x.estatusVista).HasColumnName("estatusVista");
            ToTable("tblGA_Vistas");
        }
    }
}
