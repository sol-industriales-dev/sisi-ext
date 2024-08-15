using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra
{
    public class tblCOES_EspecialidadtblX_SubContratistaMapping : EntityTypeConfiguration<tblCOES_EspecialidadtblX_SubContratista>
    {
        public tblCOES_EspecialidadtblX_SubContratistaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblCOES_EspecialidadtblX_SubContratista");
        }
    }
}
