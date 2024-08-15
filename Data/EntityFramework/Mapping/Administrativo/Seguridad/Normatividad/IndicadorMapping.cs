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
    public class IndicadorMapping : EntityTypeConfiguration<tblNOM_Indicador>
    {
        public IndicadorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.verificacion).HasColumnName("verificacion");
            Property(x => x.porcentaje).HasColumnName("porcentaje");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.indice).HasColumnName("indice");
            Property(x => x.periodicidad).HasColumnName("periodicidad");
            Property(x => x.normaID).HasColumnName("normaID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblNOM_Indicador");
        }
    }
}
