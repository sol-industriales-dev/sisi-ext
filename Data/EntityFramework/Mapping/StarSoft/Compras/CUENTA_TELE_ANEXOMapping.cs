using Core.Entity.StarSoft.Requisiciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Compras
{
    public class CUENTA_TELE_ANEXOMapping : EntityTypeConfiguration<CUENTA_TELE_ANEXO>
    {
        public CUENTA_TELE_ANEXOMapping()
        {
            HasKey(x => new { x.ANEXO, x.BAN_CODIGO, x.CTABAN_CODIGO });
            Property(x => x.ANEXO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("ANEXO");
            Property(x => x.BAN_CODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("BAN_CODIGO");
            Property(x => x.CTABAN_CODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("CTABAN_CODIGO");
            ToTable("CUENTA_TELE_ANEXO");
        }
    }
}
