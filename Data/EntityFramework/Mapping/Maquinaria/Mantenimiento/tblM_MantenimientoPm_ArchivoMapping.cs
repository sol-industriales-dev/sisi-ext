using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Mantenimiento;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    public class tblM_MantenimientoPm_ArchivoMapping : EntityTypeConfiguration<tblM_MantenimientoPm_Archivo>
    {
        public tblM_MantenimientoPm_ArchivoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            
            Property(x => x.idMantenimiento).HasColumnName("idMantenimiento");
            Property(x => x.idActividad).HasColumnName("idActividad");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblM_MantenimientoPm_Archivo");
        }
    }
}
