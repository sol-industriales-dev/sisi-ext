using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Proyecciones
{
    public class TESTDATA : EntityTypeConfiguration<tbl_TESTDATA>
    {
        TESTDATA()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CONCEPTO).HasColumnName("CONCEPTO");
            Property(x => x.FECHA1).HasColumnName("FECHA1");
            Property(x => x.FECHA2).HasColumnName("FECHA2");
            Property(x => x.FECHA3).HasColumnName("FECHA3");
            Property(x => x.FECHA4).HasColumnName("FECHA4");
            Property(x => x.FECHA5).HasColumnName("FECHA5");
            Property(x => x.FECHA6).HasColumnName("FECHA6");
            Property(x => x.FECHA7).HasColumnName("FECHA7");
            Property(x => x.FECHA8).HasColumnName("FECHA8");
            Property(x => x.FECHA9).HasColumnName("FECHA9");
            Property(x => x.FECHA10).HasColumnName("FECHA10");
            Property(x => x.FECHA11).HasColumnName("FECHA11");
            Property(x => x.FECHA12).HasColumnName("FECHA12");
            Property(x => x.TOTAL).HasColumnName("TOTAL");
            Property(x => x.tipo).HasColumnName("tipo");
            ToTable("tbl_TESTDATA");
        }
    }
}
