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
    public class tblRH_REC_EmplFamiliaresMapping : EntityTypeConfiguration<tblRH_REC_EmplFamiliares>
    {
        public tblRH_REC_EmplFamiliaresMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.apellido_paterno).HasColumnName("apellido_paterno");
            Property(x => x.apellido_materno).HasColumnName("apellido_materno");
            Property(x => x.fecha_de_nacimiento).HasColumnName("fecha_de_nacimiento");
            Property(x => x.parentesco).HasColumnName("parentesco");
            Property(x => x.grado_de_estudios).HasColumnName("grado_de_estudios");
            Property(x => x.estado_civil).HasColumnName("estado_civil");
            Property(x => x.estudia).HasColumnName("estudia");
            Property(x => x.genero).HasColumnName("genero");
            Property(x => x.vive).HasColumnName("vive");
            Property(x => x.beneficiario).HasColumnName("beneficiario");
            Property(x => x.trabaja).HasColumnName("trabaja");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");

            ToTable("tblRH_REC_EmplFamiliares");
        }
    }
}
