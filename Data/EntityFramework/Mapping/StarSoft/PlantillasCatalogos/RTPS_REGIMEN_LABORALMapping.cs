using Core.Entity.StarSoft.PlantillasCatalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Plantillas
{
    public class RTPS_REGIMEN_LABORALMapping : EntityTypeConfiguration<RTPS_REGIMEN_LABORAL>
    {
        public RTPS_REGIMEN_LABORALMapping() 
        {
            HasKey(x => x.CODIGO_REGIMEN_LABORAL);
            Property(x => x.CODIGO_REGIMEN_LABORAL).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("CODIGO_REGIMEN_LABORAL");
            ToTable("RTPS_REGIMEN_LABORAL");
        }
    }
}