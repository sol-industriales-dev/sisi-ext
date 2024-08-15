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
    public class TipoServicioOverhaulMapping : EntityTypeConfiguration<tblM_CatTipoServicioOverhaul>
    {
        TipoServicioOverhaulMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.grupoMaquinaID).HasColumnName("grupoMaquinaID");
            Property(x => x.modeloMaquinaID).HasColumnName("modeloMaquinaID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.planeacion).HasColumnName("planeacion");
            ToTable("tblM_ArchivosNotasCredito");
        }
    }
}