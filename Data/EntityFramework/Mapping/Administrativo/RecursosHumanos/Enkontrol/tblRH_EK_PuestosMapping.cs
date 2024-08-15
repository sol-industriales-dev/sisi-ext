using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_PuestosMapping : EntityTypeConfiguration<tblRH_EK_Puestos>
    {
        public tblRH_EK_PuestosMapping()
        {
            HasKey(x => x.puesto);
            Property(x => x.puesto).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("puesto");
            //HasOptional(x => x.virtualTipoNomina).WithMany().HasForeignKey(x => x.FK_TipoNomina);
            ToTable("tblRH_EK_Puestos");
        }
    }
}
