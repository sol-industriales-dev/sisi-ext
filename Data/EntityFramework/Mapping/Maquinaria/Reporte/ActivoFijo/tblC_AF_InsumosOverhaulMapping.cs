using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_InsumosOverhaulMapping : EntityTypeConfiguration<tblC_AF_InsumosOverhaul>
    {
        public tblC_AF_InsumosOverhaulMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Insumo).HasColumnName("insumo");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            Property(x => x.Porcentaje).HasColumnName("porcentaje");
            Property(x => x.Meses).HasColumnName("meses");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.IdUsuario).HasColumnName("idUsuario");
            Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(y => y.IdUsuario);
            ToTable("tblC_AF_InsumosOverhaul");
        }
    }
}
