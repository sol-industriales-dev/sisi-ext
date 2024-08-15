using Core.Entity.Administrativo.RecursosHumanos.Mural;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Mural
{
    public class tblRH_MuralMapping : EntityTypeConfiguration<tblRH_Mural>
    {
        public tblRH_MuralMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Titulo).HasColumnName("titulo");
            Property(x => x.Color).HasColumnName("color");
            Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.FechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.IdUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            HasRequired(x => x.UsuarioCreacion).WithMany().HasForeignKey(y => y.IdUsuarioCreacion);
            ToTable("tblRH_Mural");
        }
    }
}
