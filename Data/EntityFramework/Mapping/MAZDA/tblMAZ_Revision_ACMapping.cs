using Core.Entity.MAZDA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.MAZDA
{
    class tblMAZ_Revision_ACMapping : EntityTypeConfiguration<tblMAZ_Revision_AC>
    {
        public tblMAZ_Revision_ACMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.equipoID).HasColumnName("equipoID");
            Property(x => x.tonelaje).HasColumnName("tonelaje");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.tecnico).HasColumnName("tecnico");
            Property(x => x.ayudantes).HasColumnName("ayudantes");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");

            Property(x => x.planMesDetalleID).HasColumnName("planMesDetalleID");

            ToTable("tblMAZ_Revision_AC");
        }
    }
}
