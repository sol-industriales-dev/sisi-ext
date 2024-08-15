using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
     class BitacoraActividadesMantProyMappingProy : EntityTypeConfiguration<tblM_BitacoraActividadesMantProy>
    {
        public BitacoraActividadesMantProyMappingProy()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idMant).HasColumnName("idMant");
            Property(x => x.Observaciones).HasColumnName("Observaciones");
            Property(x => x.UsuarioCap).HasColumnName("UsuarioCap");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.idPm).HasColumnName("idPm");
            Property(x => x.aplicar).HasColumnName("aplicar");
            ToTable("tblM_BitacoraActividadesMantProy");
        }

    }
}
