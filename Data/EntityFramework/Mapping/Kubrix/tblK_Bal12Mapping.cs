using Core.Entity.Kubrix;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Kubrix
{
    public class tblK_Bal12Mapping : EntityTypeConfiguration<tblK_Bal12>
    {
        public tblK_Bal12Mapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.centroCosto).HasColumnName("centroCosto");
            Property(x => x.descContable).HasColumnName("descContable");
            Property(x => x.mesAct).HasColumnName("mesAct");
            Property(x => x.saldoInicial).HasColumnName("saldoInicial");
            Property(x => x.enero).HasColumnName("enero");
            Property(x => x.febrero).HasColumnName("febrero");
            Property(x => x.marzo).HasColumnName("marzo");
            Property(x => x.abril).HasColumnName("abril");
            Property(x => x.mayo).HasColumnName("mayo");
            Property(x => x.junio).HasColumnName("junio");
            Property(x => x.julio).HasColumnName("julio");
            Property(x => x.agosto).HasColumnName("agosto");
            Property(x => x.septiembre).HasColumnName("septiembre");
            Property(x => x.octubre).HasColumnName("octubre");
            Property(x => x.noviembre).HasColumnName("noviembre");
            Property(x => x.diciembre).HasColumnName("diciembre");
            ToTable("tblK_Bal12", "Kubrix");
        }
    }
}
