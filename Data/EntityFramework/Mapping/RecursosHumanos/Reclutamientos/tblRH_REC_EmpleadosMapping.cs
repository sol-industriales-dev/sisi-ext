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
    public class tblRH_REC_EmpleadosMapping : EntityTypeConfiguration<tblRH_REC_Empleados>
    {
        public tblRH_REC_EmpleadosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idEstatus).HasColumnName("idEstatus");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ape_paterno).HasColumnName("ape_paterno");
            Property(x => x.ape_materno).HasColumnName("ape_materno");
            Property(x => x.fecha_nac).HasColumnName("fecha_nac");
            Property(x => x.clave_pais_nac).HasColumnName("clave_pais_nac");
            Property(x => x.clave_estado_nac).HasColumnName("clave_estado_nac");
            Property(x => x.clave_ciudad_nac).HasColumnName("clave_ciudad_nac");
            Property(x => x.localidad_nacimiento).HasColumnName("localidad_nacimiento");
            Property(x => x.fecha_alta).HasColumnName("fecha_alta");
            Property(x => x.sexo).HasColumnName("sexo");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.curp).HasColumnName("curp");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.num_cta_pago).HasColumnName("num_cta_pago");
            Property(x => x.num_cta_pago_aho).HasColumnName("num_cta_pago_aho");
            Property(x => x.idCandidato).HasColumnName("idCandidato");
            Property(x => x.esPendiente).HasColumnName("esPendiente");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.esReingresoEmpleado).HasColumnName("esReingresoEmpleado");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");

            ToTable("tblRH_REC_Empleados");
        }
    }
}
