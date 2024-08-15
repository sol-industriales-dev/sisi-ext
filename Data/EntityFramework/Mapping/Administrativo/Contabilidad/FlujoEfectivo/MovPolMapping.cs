using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.FlujoEfectivo
{
    public class MovPolMapping : EntityTypeConfiguration<tblC_FE_MovPol>
    {
        public MovPolMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idConcepto).HasColumnName("idConcepto");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.tp).HasColumnName("tp");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.linea).HasColumnName("linea");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.numpro).HasColumnName("numpro");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.tm).HasColumnName("tm");
            Property(x => x.itm).HasColumnName("itm");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.esFlujoEfectivo).HasColumnName("esFlujoEfectivo");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_FE_MovPol");
        }
    }
}
