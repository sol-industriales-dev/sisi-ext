using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Proyecciones
{
    public class CapturadeObrasMapping: EntityTypeConfiguration<tblPro_CapturadeObras>
    {
        public CapturadeObrasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CadenaJson).HasColumnName("CadenaJson");
            Property(x => x.EjercicioInicial).HasColumnName("EjercicioInicial");
            Property(x => x.MesInicio).HasColumnName("MesInicio");
            Property(x => x.Escenario).HasColumnName("Escenario");
            ToTable("tblPro_CapturadeObras");
        }
    }
}
