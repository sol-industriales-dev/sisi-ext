using Core.Entity.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras
{
    public class tblCom_sp_proveedoresColombiaMapping : EntityTypeConfiguration<tblCom_sp_proveedoresColombia>
    {
        public tblCom_sp_proveedoresColombiaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblCom_sp_proveedoresColombia");
        }
    }
}
