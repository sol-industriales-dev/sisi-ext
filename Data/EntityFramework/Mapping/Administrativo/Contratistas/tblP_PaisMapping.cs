using Core.Entity.Administrativo.Contratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contratistas
{
    public class tblP_PaisMapping : EntityTypeConfiguration<tblP_Pais>
    {
        public tblP_PaisMapping()
        {
            HasKey(x => x.idPais);
            Property(x => x.idPais).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("idPais");
            Property(x => x.Pais).HasColumnName("Pais");

            ToTable("tblP_Pais");
        }
    }
}
