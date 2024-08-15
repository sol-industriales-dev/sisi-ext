using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Core.Entity.Cursos;

namespace Data.EntityFramework.Mapping.Cursos
{
    public class ExamenMapping: EntityTypeConfiguration<tblCU_Examen>
    {
        public ExamenMapping()
           {
               HasKey(x => x.id);
               Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
               Property(x => x.idCurso).HasColumnName("idCurso");//llave foranea
               Property(x => x.descripcion).HasColumnName("descripcion");
               Property(x => x.editable).HasColumnName("editable");
               Property(x => x.fecha).HasColumnName("fecha");
               Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
               Property(x => x.folio).HasColumnName("folio");
               Property(x => x.nombreExamen).HasColumnName("nombreExamen");
               Property(x => x.nomUsuarioCap).HasColumnName("nomUsuarioCap");
               Property(x => x.usuarioCap).HasColumnName("usuarioCap");
               ToTable("tblCU_Examen");
           }
    }
}
