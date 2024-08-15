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
    public class tblRH_REC_EmplGenContactoMapping : EntityTypeConfiguration<tblRH_REC_EmplGenContacto>
    {
        public tblRH_REC_EmplGenContactoMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.estado_civil).HasColumnName("estado_civil");
            Property(x => x.fecha_planta).HasColumnName("fecha_planta");
            Property(x => x.ocupacion).HasColumnName("ocupacion");
            Property(x => x.ocupacion_abrev).HasColumnName("ocupacion_abrev");
            Property(x => x.num_cred_elector).HasColumnName("num_cred_elector");
            Property(x => x.domicilio).HasColumnName("domicilio");
            Property(x => x.numero_exterior).HasColumnName("numero_exterior");
            Property(x => x.numero_interior).HasColumnName("numero_interior");
            Property(x => x.colonia).HasColumnName("colonia");
            Property(x => x.estado_dom).HasColumnName("estado_dom");
            Property(x => x.ciudad_dom).HasColumnName("ciudad_dom");
            Property(x => x.codigo_postal).HasColumnName("codigo_postal");
            Property(x => x.tel_casa).HasColumnName("tel_casa");
            Property(x => x.tel_cel).HasColumnName("tel_cel");
            Property(x => x.email).HasColumnName("email");
            Property(x => x.tipo_casa).HasColumnName("tipo_casa");
            Property(x => x.tipo_sangre).HasColumnName("tipo_sangre");
            Property(x => x.alergias).HasColumnName("alergias");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");

            ToTable("tblRH_REC_EmplGenContacto");
        }
    }
}
