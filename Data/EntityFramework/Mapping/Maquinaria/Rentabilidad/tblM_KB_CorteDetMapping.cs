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
    public class tblM_KB_CorteDetMapping : EntityTypeConfiguration<tblM_KB_CorteDet>
    {
        public tblM_KB_CorteDetMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.corteID).HasColumnName("corteID");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.tp).HasColumnName("tp");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.linea).HasColumnName("linea");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.ccSIGOPLAN).HasColumnName("ccSIGOPLAN");
            Property(x => x.acSIGOPLAN).HasColumnName("acSIGOPLAN");
            Property(x => x.divisionID).HasColumnName("divisionID");
            Property(x => x.fechapol).HasColumnName("fechapol");
            Property(x => x.tipoEquipo).HasColumnName("tipoEquipo");
            Property(x => x.referencia).HasColumnName("referencia");
            Property(x => x.conciliacionID).HasColumnName("conciliacionID");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.esEstimado).HasColumnName("esEstimado");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.esCancelacion).HasColumnName("esCancelacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            ToTable("tblM_KB_CorteDet");
        }
    }
}
