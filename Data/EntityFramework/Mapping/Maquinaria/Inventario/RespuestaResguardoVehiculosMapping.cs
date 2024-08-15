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
    public class RespuestaResguardoVehiculosMapping : EntityTypeConfiguration<tblM_RespuestaResguardoVehiculos>
    {

        RespuestaResguardoVehiculosMapping()
        {

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Bueno).HasColumnName("Bueno");
            Property(x => x.HasDocumento).HasColumnName("HasDocumento");
            Property(x => x.Malo).HasColumnName("Malo");
            Property(x => x.NA).HasColumnName("NA");
            Property(x => x.Observaciones).HasColumnName("Observacion");
            Property(x => x.Regular).HasColumnName("Regular");
            Property(x => x.ResguardoID).HasColumnName("ResguardoID");
            Property(x => x.RespuestaID).HasColumnName("RespuestaID");

            ToTable("tblM_RespuestaResguardoVehiculos");

        }
    }
}
