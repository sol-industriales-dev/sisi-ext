using Core.Entity.Enkontrol.Compras.Requisicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.Requisicion
{
    public class tblCom_ReqTipoMapping : EntityTypeConfiguration<tblCom_ReqTipo>
    {
        public tblCom_ReqTipoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblCom_ReqTipo");
        }
    }
}
