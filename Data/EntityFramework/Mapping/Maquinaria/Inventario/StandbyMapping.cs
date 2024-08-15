using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class StandbyMapping : EntityTypeConfiguration<tblM_CapStandBy>
    {
        public StandbyMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.FechaCaptura).HasColumnName("FechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.FechaFin).HasColumnName("FechaFin");
            Property(x => x.FechaInicio).HasColumnName("FechaInicio");
            Property(x => x.UsuarioElabora).HasColumnName("UsuarioElabora");
            Property(x => x.UsuarioGerente).HasColumnName("UsuarioGerente");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.folio).HasColumnName("folio");

            ToTable("tblM_CapStandBy");
        }
    }
}
