using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.Proveedores
{
    class PreguntasProveedoresMapping : EntityTypeConfiguration<tblEN_PreguntasProveedores>
    {
        public PreguntasProveedoresMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.encuestaID).HasColumnName("encuestaID");
            Property(x => x.pregunta).HasColumnName("pregunta");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.visible).HasColumnName("visible");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.ponderacion).HasColumnName("ponderacion");
            HasRequired(x => x.encuesta).WithMany(x => x.preguntas).HasForeignKey(y => y.encuestaID);
            ToTable("tblEN_PreguntasProveedores");
        }
    }

}
