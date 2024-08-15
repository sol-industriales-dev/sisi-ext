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
    public class CxCMapping : EntityTypeConfiguration<tblPro_CxC>
    {
        public CxCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CadenaJson).HasColumnName("CadenaJson");
            Property(x => x.Anio).HasColumnName("EjercicioInicial");
            Property(x => x.Mes).HasColumnName("MesInicio");
            Property(x => x.Estatus).HasColumnName("Escenario");
            ToTable("tblPro_CxC");
        }
    }
}
