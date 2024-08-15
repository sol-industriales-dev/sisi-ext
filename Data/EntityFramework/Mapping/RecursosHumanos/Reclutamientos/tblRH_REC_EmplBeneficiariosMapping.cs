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
    public class tblRH_REC_EmplBeneficiariosMapping : EntityTypeConfiguration<tblRH_REC_EmplBeneficiarios>
    {
        public tblRH_REC_EmplBeneficiariosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.parentesco_ben).HasColumnName("parentesco_ben");
            Property(x => x.codigo_postal_ben).HasColumnName("codigo_postal_ben");
            Property(x => x.fecha_nac_ben).HasColumnName("fecha_nac_ben");
            Property(x => x.paterno_ben).HasColumnName("paterno_ben");
            Property(x => x.materno_ben).HasColumnName("materno_ben");
            Property(x => x.nombre_ben).HasColumnName("nombre_ben");
            Property(x => x.estado_ben).HasColumnName("estado_ben");
            Property(x => x.ciudad_ben).HasColumnName("ciudad_ben");
            Property(x => x.colonia_ben).HasColumnName("colonia_ben");
            Property(x => x.domicilio_ben).HasColumnName("domicilio_ben");
            Property(x => x.num_ext_ben).HasColumnName("num_ext_ben");
            Property(x => x.num_int_ben).HasColumnName("num_int_ben");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblRH_REC_EmplBeneficiarios");
        }
    }
}
