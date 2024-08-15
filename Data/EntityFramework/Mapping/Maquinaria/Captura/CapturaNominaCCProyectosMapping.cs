using Core.Entity.Maquinaria.Captura;
using Core.Service.Maquinaria.Capturas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    class CapturaNominaCCProyectosMapping : EntityTypeConfiguration<tblM_CapNominaCC_Proyectos>
    {
        public CapturaNominaCCProyectosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.capNominaCCID).HasColumnName("capNominaCCID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblM_CapNominaCC_Proyectos");
        }
    }
}
