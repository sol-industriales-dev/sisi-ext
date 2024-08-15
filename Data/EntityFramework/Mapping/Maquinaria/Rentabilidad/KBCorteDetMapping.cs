using Core.Entity.Maquinaria.Rentabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Rentabilidad
{
    public class KBCorteDetMapping : EntityTypeConfiguration<tblM_KBCorteDet>
    {
        KBCorteDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.corteID).HasColumnName("corteID");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.cuenta).HasColumnName("cuenta");
            //Property(x => x.scta).HasColumnName("scta");
            //Property(x => x.scta_Desc).HasColumnName("scta_Desc");
            //Property(x => x.sscta).HasColumnName("sscta");
            //Property(x => x.sscta_Desc).HasColumnName("sscta_Desc");
            Property(x => x.concepto).HasColumnName("concepto");
            //Property(x => x.tm).HasColumnName("tm");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            //Property(x => x.cuenta_oc).HasColumnName("cuenta_oc");
            Property(x => x.fechapol).HasColumnName("fechapol");
            Property(x => x.tipoEquipo).HasColumnName("tipoEquipo");
            Property(x => x.referencia).HasColumnName("referencia");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.linea).HasColumnName("linea");

            ToTable("tblM_KBCorteDet");
        }
    }
}