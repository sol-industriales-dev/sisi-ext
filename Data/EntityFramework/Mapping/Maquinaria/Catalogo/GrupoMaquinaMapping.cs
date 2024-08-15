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
    class GrupoMaquinaMapping : EntityTypeConfiguration<tblM_CatGrupoMaquinaria>
    {
        public GrupoMaquinaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.prefijo).HasColumnName("prefijo");
            Property(x => x.dn).HasColumnName("dn");
            Property(x => x.bitacora).HasColumnName("bitacora");
            Property(x => x.sos).HasColumnName("sos");
            Property(x => x.setFotografico).HasColumnName("setFotografico");
            Property(x => x.rehabilitacion).HasColumnName("rehabilitacion");


            HasRequired(x => x.tipoEquipo).WithMany().HasForeignKey(y => y.tipoEquipoID);
            //HasMany(x => x.marca).WithMany(x => x.grupo).Map(m =>
            //{
            //    m.ToTable("tblM_CatMarcaEquipotblM_CatGrupoMaquinaria");
            //    m.MapLeftKey("marcaID");
            //    m.MapRightKey("grupoID");
            //});
            ToTable("tblM_CatGrupoMaquinaria");

        }

    }
}
