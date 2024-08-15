using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{
    public class ModeloEquipoMapping : EntityTypeConfiguration<tblM_CatModeloEquipo>
    {
        ModeloEquipoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.marcaEquipoID).HasColumnName("marcaEquipoID");
            Property(x => x.nomCorto).HasColumnName("nomCorto");
            Property(x => x.noComponente).HasColumnName("noComponente");
            Property(x => x.idGrupo).HasColumnName("idGrupo");
            Property(x => x.Ruta).HasColumnName("Ruta");
            HasRequired(x => x.marcaEquipo).WithMany().HasForeignKey(y => y.marcaEquipoID);
            ToTable("tblM_CatModeloEquipo");
        }
    }
}
