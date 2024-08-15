using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.BackLogs
{
    public class tblBL_PartesMapping : EntityTypeConfiguration<tblBL_Partes>
    {
        public tblBL_PartesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.parte).HasColumnName("parte");
            Property(x => x.articulo).HasColumnName("articulo");
            Property(x => x.unidad).HasColumnName("unidad");
            Property(x => x.idBacklog).HasColumnName("idBacklog");
            Property(x => x.costoPromedio).HasColumnName("costoPromedio");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblBL_Partes");
        }
    }
}
