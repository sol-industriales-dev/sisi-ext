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
    public class AF_CatalogoMaquinaMapping : EntityTypeConfiguration<tblC_AF_CatalogoMaquina>
    {
        AF_CatalogoMaquinaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Cuenta).HasColumnName("Cuenta");
            Property(x => x.Descripcion).HasColumnName("Descripción");
            ToTable("tblC_AF_CatalogoMaquina");
        }
    }
}