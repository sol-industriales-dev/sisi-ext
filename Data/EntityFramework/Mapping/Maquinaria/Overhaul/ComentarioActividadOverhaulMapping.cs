using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class ComentarioActividadOverhaulMapping : EntityTypeConfiguration<tblM_ComentarioActividadOverhaul>
    {
        ComentarioActividadOverhaulMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.eventoID).HasColumnName("eventoID");
            Property(x => x.actividadID).HasColumnName("actividadID");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.numDia).HasColumnName("numDia");
            ToTable("tblM_ComentarioActividadOverhaul");
        }
    }
}