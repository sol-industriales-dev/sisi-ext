using Core.Entity.Maquinaria.Rentabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Rentabilidad
{
    public class KBFletesMapping : EntityTypeConfiguration<tblM_KBFletes>
    {
        public KBFletesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.economicoID).HasColumnName("economicoID");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            ToTable("tblM_KBFletes");
        }
    }
}
