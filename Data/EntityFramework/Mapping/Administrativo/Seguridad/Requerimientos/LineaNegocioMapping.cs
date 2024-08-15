using Core.Entity.Administrativo.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Requerimientos
{
    public class LineaNegocioMapping : EntityTypeConfiguration<tblS_Req_LineaNegocio>
    {
        public LineaNegocioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblS_Req_LineaNegocio");
        }
    }
}
