using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_EmplContEmergenciasMapping : EntityTypeConfiguration<tblRH_REC_EmplContEmergencias>
    {
        public tblRH_REC_EmplContEmergenciasMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.en_accidente_nombre).HasColumnName("en_accidente_nombre");
            Property(x => x.en_accidente_telefono).HasColumnName("en_accidente_telefono");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblRH_REC_EmplContEmergencias");
        }
    }
}
