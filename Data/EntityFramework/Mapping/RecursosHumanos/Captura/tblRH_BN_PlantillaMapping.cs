using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_BN_PlantillaMapping : EntityTypeConfiguration<tblRH_BN_Plantilla>
    {
        public tblRH_BN_PlantillaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.ccNombre).HasColumnName("ccNombre");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.usuarioCapturoID).HasColumnName("usuarioCapturoID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.version).HasColumnName("version");
            Property(x => x.versionActiva).HasColumnName("versionActiva");
            HasRequired(x => x.usuarioCapturo).WithMany().HasForeignKey(y => y.usuarioCapturoID);

            HasMany(x => x.listDetalle).WithRequired(x => x.plantilla).HasForeignKey(x => x.plantillaID);
            HasMany(x => x.listAutorizadores).WithRequired(x => x.plantilla).HasForeignKey(x => x.plantillaID);

            ToTable("tblRH_BN_Plantilla");
        }
    }
}
