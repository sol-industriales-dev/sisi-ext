using Core.Entity.StarSoft.PlantillasCatalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.PlantillasCatalogos
{
    public class RTPS_REGIMEN_ESSALUDMapping : EntityTypeConfiguration<RTPS_REGIMEN_ESSALUD>
    {
        public RTPS_REGIMEN_ESSALUDMapping()
        {
            HasKey(x => x.CODIGO);
            Property(x => x.CODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("CODIGO");
            ToTable("RTPS_REGIMEN_ESSALUD");
        }
    }
}