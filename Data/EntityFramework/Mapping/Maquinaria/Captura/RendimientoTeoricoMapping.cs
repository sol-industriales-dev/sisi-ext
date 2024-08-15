using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class RendimientoTeoricoMapping : EntityTypeConfiguration<tblM_CatRendimientoTeorico>
    {
       public  RendimientoTeoricoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.medio).HasColumnName("medio");
            Property(x => x.bajo).HasColumnName("bajo");
            Property(x => x.alto).HasColumnName("alto");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.modeloEquipo).WithMany().HasForeignKey(y => y.modeloEquipoID);
            ToTable("tblM_CatRendimientoTeorico");
        }
    }
}
