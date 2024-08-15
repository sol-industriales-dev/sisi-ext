using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PreNominaPeru_DetMapping : EntityTypeConfiguration<tblC_Nom_PreNominaPeru_Det>
    {
        public tblC_Nom_PreNominaPeru_DetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.prenominaID).HasColumnName("prenominaID");
            Property(x => x.nombre_empleado).HasColumnName("nombre_empleado");

            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.nombre_empleado).HasColumnName("nombre_empleado");
            Property(x => x.puesto).HasColumnName("puesto");

            Property(x => x.basico).HasColumnName("basico");
            Property(x => x.jornada_semanal).HasColumnName("jornada_semanal");
            Property(x => x.horas_extra_60).HasColumnName("horas_extra_60");
            Property(x => x.horas_extra_100).HasColumnName("horas_extra_100");
            Property(x => x.horas_nocturnas).HasColumnName("horas_nocturnas");
            Property(x => x.descuento_medico).HasColumnName("descuento_medico");
            Property(x => x.feriados).HasColumnName("feriados");
            Property(x => x.subsidios).HasColumnName("subsidios");
            Property(x => x.buc).HasColumnName("buc");
            Property(x => x.bono_altitud).HasColumnName("bono_altitud");
            Property(x => x.indemnizacion).HasColumnName("indemnizacion");
            Property(x => x.dominical).HasColumnName("dominical");
            Property(x => x.bonificacion_extraordinaria).HasColumnName("bonificacion_extraordinaria");
            Property(x => x.bonificacion_alta_especial).HasColumnName("bonificacion_alta_especial");
            Property(x => x.vacaciones_truncas).HasColumnName("vacaciones_truncas");
            Property(x => x.asignacion_escolar).HasColumnName("asignacion_escolar");
            Property(x => x.bono_por_altura).HasColumnName("bono_por_altura");
            Property(x => x.devolucion_5ta).HasColumnName("devolucion_5ta");
            Property(x => x.gratificacion_proporcional).HasColumnName("gratificacion_proporcional");
            Property(x => x.adelanto_quincena).HasColumnName("adelanto_quincena");
            Property(x => x.adelanto_gratificacion_semestre).HasColumnName("adelanto_gratificacion_semestre");
            Property(x => x.total_percepciones).HasColumnName("total_percepciones");

            Property(x => x.AFP_obligatoria).HasColumnName("AFP_obligatoria");
            Property(x => x.AFP_voluntaria).HasColumnName("AFP_voluntaria");
            Property(x => x.AFP_comision).HasColumnName("AFP_comision");
            Property(x => x.AFP_prima).HasColumnName("AFP_prima");
            Property(x => x.conafovicer).HasColumnName("conafovicer");
            Property(x => x.essalud_vida).HasColumnName("essalud_vida");
            Property(x => x.onp).HasColumnName("onp");
            Property(x => x.renta_5ta).HasColumnName("renta_5ta");
            Property(x => x.total_deducciones).HasColumnName("total_deducciones");

            Property(x => x.essalud_aportes).HasColumnName("essalud_aportes");
            Property(x => x.AFP_aportes).HasColumnName("AFP_aportes");
            Property(x => x.total_aportaciones).HasColumnName("total_aportaciones");

            Property(x => x.total_pagar).HasColumnName("total_pagar");
            Property(x => x.total_deposito).HasColumnName("total_deposito");

            ToTable("tblC_Nom_PreNominaPeru_Det");
        }
    }
}