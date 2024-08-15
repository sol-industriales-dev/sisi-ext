using Core.Entity.Principal.Catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Catalogos
{
    public class tblP_ArchivosComprimirMapping : EntityTypeConfiguration<tblP_ArchivosComprimir>
    {
        public tblP_ArchivosComprimirMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblP_ArchivosComprimir");
        }
    }
}
