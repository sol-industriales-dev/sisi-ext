using Core.Entity.FileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.FileManager
{
    public class tblFM_ArchivotblFM_TipoArchivo_BaseMapping : EntityTypeConfiguration<tblFM_ArchivotblFM_TipoArchivo_Base>
    {
        public tblFM_ArchivotblFM_TipoArchivo_BaseMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblFM_ArchivotblFM_TipoArchivo_Base");
        }
    }
}
