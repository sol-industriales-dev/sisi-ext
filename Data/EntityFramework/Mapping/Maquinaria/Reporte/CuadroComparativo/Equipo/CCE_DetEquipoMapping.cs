using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.CuadroComparativo.Equipo
{
    public class CCE_DetEquipoMapping : EntityTypeConfiguration<tblM_CCE_DetEquipo>
    {
        public CCE_DetEquipoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdCuadro).HasColumnName("IdCuadro");
            Property(x => x.IdProveedor).HasColumnName("IdProveedor");
            Property(x => x.EsSeleccionado).HasColumnName("EsSeleccionado");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            HasRequired(x => x.Cuadro).WithMany().HasForeignKey(y => y.IdCuadro);
            ToTable("tblM_CCE_DetEquipo");
        }
    }
}
