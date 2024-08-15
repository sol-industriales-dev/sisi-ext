using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_BN_Incidencia_detMapping : EntityTypeConfiguration<tblRH_BN_Incidencia_det>
    {
        public tblRH_BN_Incidencia_detMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.incidenciaID).HasColumnName("incidenciaID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.evaluacion_detID).HasColumnName("evaluacion_detID");
            Property(x => x.bonoUnico_detID).HasColumnName("bonoUnico_detID");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.id_incidencia).HasColumnName("id_incidencia");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");

            Property(x => x.dia1).HasColumnName("dia1");
            Property(x => x.dia2).HasColumnName("dia2");
            Property(x => x.dia3).HasColumnName("dia3");
            Property(x => x.dia4).HasColumnName("dia4");
            Property(x => x.dia5).HasColumnName("dia5");
            Property(x => x.dia6).HasColumnName("dia6");
            Property(x => x.dia7).HasColumnName("dia7");
            Property(x => x.dia8).HasColumnName("dia8");
            Property(x => x.dia9).HasColumnName("dia9");
            Property(x => x.dia10).HasColumnName("dia10");
            Property(x => x.dia11).HasColumnName("dia11");
            Property(x => x.dia12).HasColumnName("dia12");
            Property(x => x.dia13).HasColumnName("dia13");
            Property(x => x.dia14).HasColumnName("dia14");
            Property(x => x.dia15).HasColumnName("dia15");
            Property(x => x.dia16).HasColumnName("dia16");

            Property(x => x.he_dia1).HasColumnName("he_dia1");
            Property(x => x.he_dia2).HasColumnName("he_dia2");
            Property(x => x.he_dia3).HasColumnName("he_dia3");
            Property(x => x.he_dia4).HasColumnName("he_dia4");
            Property(x => x.he_dia5).HasColumnName("he_dia5");
            Property(x => x.he_dia6).HasColumnName("he_dia6");
            Property(x => x.he_dia7).HasColumnName("he_dia7");
            Property(x => x.he_dia8).HasColumnName("he_dia8");
            Property(x => x.he_dia9).HasColumnName("he_dia9");
            Property(x => x.he_dia10).HasColumnName("he_dia10");
            Property(x => x.he_dia11).HasColumnName("he_dia11");
            Property(x => x.he_dia12).HasColumnName("he_dia12");
            Property(x => x.he_dia13).HasColumnName("he_dia13");
            Property(x => x.he_dia14).HasColumnName("he_dia14");
            Property(x => x.he_dia15).HasColumnName("he_dia15");
            Property(x => x.he_dia16).HasColumnName("he_dia16");

            Property(x => x.bono).HasColumnName("bono");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.archivo_enviado).HasColumnName("archivo_enviado");
            Property(x => x.dias_extras).HasColumnName("dias_extras");
            Property(x => x.prima_dominical).HasColumnName("prima_dominical");
            Property(x => x.bonoDM).HasColumnName("bonoDM");
            Property(x => x.bonoCuadrado).HasColumnName("bonoCuadrado");
            Property(x => x.bonoDM_Obs).HasColumnName("bonoDM_Obs");
            Property(x => x.bonoU).HasColumnName("bonoU");
            Property(x => x.bono_Obs).HasColumnName("bono_Obs");
            Property(x => x.total_Dias).HasColumnName("total_Dias");
            Property(x => x.totalo_Horas).HasColumnName("totalo_Horas");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ape_paterno).HasColumnName("ape_paterno");
            Property(x => x.ape_materno).HasColumnName("ape_materno");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.puestoDesc).HasColumnName("puestoDesc");
            Property(x => x.clave_depto).HasColumnName("clave_depto");
            Property(x => x.horas_extras).HasColumnName("horas_extras");
            Property(x => x.deptoDesc).HasColumnName("deptoDesc");
            Property(x => x.primaDominical).HasColumnName("primaDominical");
            Property(x => x.dias_extra_concepto).HasColumnName("dias_extra_concepto");

            HasRequired(x => x.incidencia).WithMany().HasForeignKey(y => y.incidenciaID);
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);


            ToTable("tblRH_BN_Incidencia_det");
        }
    }
}
