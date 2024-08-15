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
    public class CursoMapping : EntityTypeConfiguration<tblCU_Curso>
    {
        public CursoMapping()
        {
        HasKey(x => x.id);
        Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
        Property(x => x.fecha).HasColumnName("fecha");
        Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
        Property(x => x.nombreCurso).HasColumnName("nombreCurso");
        Property(x => x.descripcion).HasColumnName("descripcion");    
        Property(x => x.nomUsuarioCap).HasColumnName("nomUsuarioCap");
        Property(x => x.usuarioCap).HasColumnName("usuarioCap");
        Property(x => x.editable).HasColumnName("editable");
        Property(x => x.estado).HasColumnName("estado");
        Property(x => x.completo).HasColumnName("completo"); 
        ToTable("tblCu_Curso");
        }
    }
}

