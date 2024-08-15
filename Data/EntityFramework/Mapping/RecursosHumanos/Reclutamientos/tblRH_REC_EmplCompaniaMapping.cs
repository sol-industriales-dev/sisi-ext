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
    public class tblRH_REC_EmplCompaniaMapping : EntityTypeConfiguration<tblRH_REC_EmplCompania>
    {
        public tblRH_REC_EmplCompaniaMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.requisicion).HasColumnName("requisicion");
            Property(x => x.id_regpat).HasColumnName("id_regpat");
            Property(x => x.cc_contable).HasColumnName("cc_contable");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.duracion_contrato).HasColumnName("duracion_contrato");
            Property(x => x.jefe_inmediato).HasColumnName("jefe_inmediato");
            Property(x => x.autoriza).HasColumnName("autoriza");
            Property(x => x.usuario_compras).HasColumnName("usuario_compras");
            Property(x => x.sindicato).HasColumnName("sindicato");
            Property(x => x.clave_depto).HasColumnName("clave_depto");
            Property(x => x.nss).HasColumnName("nss");
            Property(x => x.unidad_medica).HasColumnName("unidad_medica");
            Property(x => x.tipo_formula_imss).HasColumnName("tipo_formula_imss");
            Property(x => x.fecha_contrato).HasColumnName("fecha_contrato");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");

            ToTable("tblRH_REC_EmplCompania");
        }
    }
}