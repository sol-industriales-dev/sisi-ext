using Core.Entity.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras
{
    class tblAlm_UbicacionMapping : EntityTypeConfiguration<tblAlm_Ubicacion>
    {
        public tblAlm_UbicacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblAlm_Ubicacion");
        }
    }
}
