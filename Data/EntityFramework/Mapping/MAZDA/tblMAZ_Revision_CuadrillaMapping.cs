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
    class tblMAZ_Revision_CuadrillaMapping : EntityTypeConfiguration<tblMAZ_Revision_Cuadrilla>
    {
        public tblMAZ_Revision_CuadrillaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cuadrillaID).HasColumnName("cuadrillaID");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.tecnico).HasColumnName("tecnico");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");

            Property(x => x.planMesDetalleID).HasColumnName("planMesDetalleID");

            ToTable("tblMAZ_Revision_Cuadrilla");
        }
    }
}
