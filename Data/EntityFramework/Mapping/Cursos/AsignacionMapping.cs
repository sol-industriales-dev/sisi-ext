using Core.Entity.Cursos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Cursos
{
    public class AsignacionMapping : EntityTypeConfiguration<tblCU_Asignacion>
    {
        public AsignacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCurso).HasColumnName("idCurso");
            Property(x => x.claveUsuario).HasColumnName("claveUsuario");
            Property(x => x.estado).HasColumnName("estado");
            ToTable("tblCU_Asignacion");
        }
    }
}

