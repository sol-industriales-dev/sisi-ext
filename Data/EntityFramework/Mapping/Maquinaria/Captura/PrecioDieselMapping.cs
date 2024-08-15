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
    class PrecioDieselMapping: EntityTypeConfiguration<tblM_CapPrecioDiesel>
    {
        public PrecioDieselMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.precio).HasColumnName("precio");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.fecha).HasColumnName("fecha");
            ToTable("tblM_CapPrecioDiesel");
        }
    
    }
}
