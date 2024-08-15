using Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_AreaMapping : EntityTypeConfiguration<tbl5s_Area>
    {
        public tbl5s_AreaMapping()
        {
            HasKey(x => x.id);
            ToTable("tbl5s_Area");
        }
    }
}
