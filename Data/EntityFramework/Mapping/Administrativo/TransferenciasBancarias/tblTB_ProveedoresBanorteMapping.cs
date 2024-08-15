using Core.Entity.Administrativo.TransferenciasBancarias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.TransferenciasBancarias
{
    public class tblTB_ProveedoresBanorteMapping : EntityTypeConfiguration<tblTB_ProveedoresBanorte>
    {
        public tblTB_ProveedoresBanorteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblTB_ProveedoresBanorte");
        }
    }
}
