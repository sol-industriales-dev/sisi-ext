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
    public class AditivaDeductivaMapping:EntityTypeConfiguration<tblRH_AditivaDeductiva>
    {
        public AditivaDeductivaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fecha_Alta).HasColumnName("fecha_Alta");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.usuarioCap).HasColumnName("usuarioCap");
            Property(x => x.aprobado).HasColumnName("aprobado");
            Property(x => x.rechazado).HasColumnName("rechazado");
            Property(x => x.cC).HasColumnName("cC");
            Property(x => x.nomUsuarioCap).HasColumnName("nomUsuarioCap");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.cCid).HasColumnName("cCid");
            Property(x => x.editable).HasColumnName("editable");
            Property(x => x.condicionInicial).HasColumnName("condicionInicial");
            Property(x => x.condicionActual).HasColumnName("condicionActual");
            Property(x => x.soporte).HasColumnName("soporte");
            Property(x => x.link).HasColumnName("link");

            ToTable("tblRH_AditivaDeductiva");
        }

    }
}
