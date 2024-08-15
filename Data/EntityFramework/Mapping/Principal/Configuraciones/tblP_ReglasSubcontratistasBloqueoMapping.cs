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
    class tblP_ReglasSubcontratistasBloqueoMapping : EntityTypeConfiguration<tblP_ReglasSubcontratistasBloqueo>
    {
        public tblP_ReglasSubcontratistasBloqueoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.regla).HasColumnName("regla");
            Property(x => x.aplicar).HasColumnName("aplicar");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblP_ReglasSubcontratistasBloqueo");
        }
    }
}
