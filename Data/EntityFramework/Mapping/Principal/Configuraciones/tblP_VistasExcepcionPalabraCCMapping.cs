using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Configuraciones
{

    public class tblP_VistasExcepcionPalabraCCMapping : EntityTypeConfiguration<tblP_VistasExcepcionPalabraCC>
    {
        public tblP_VistasExcepcionPalabraCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.vistaID).HasColumnName("vistaID");


            ToTable("tblP_VistasExcepcionPalabraCC");
        }
    }
}
