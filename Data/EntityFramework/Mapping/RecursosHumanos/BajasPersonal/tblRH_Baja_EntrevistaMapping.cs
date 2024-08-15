using Core.Entity.RecursosHumanos.Bajas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Data.EntityFramework.Mapping.RecursosHumanos.Bajas
{
    public class tblRH_Baja_EntrevistaMapping : EntityTypeConfiguration<tblRH_Baja_Entrevista>
    {
        public tblRH_Baja_EntrevistaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.registroID).HasColumnName("registroID");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.cc_nombre).HasColumnName("cc_nombre");
            Property(x => x.gerente_clave).HasColumnName("gerente_clave");
            Property(x => x.fecha_ingreso).HasColumnName("fecha_ingreso");
            Property(x => x.fecha_salida).HasColumnName("fecha_salida");
            Property(x => x.fecha_nacimiento).HasColumnName("fecha_nacimiento");
            Property(x => x.anios).HasColumnName("anios");
            Property(x => x.estado_civil_clave).HasColumnName("estado_civil_clave");
            Property(x => x.estado_civil_nombre).HasColumnName("estado_civil_nombre");
            Property(x => x.escolaridad_clave).HasColumnName("escolaridad_clave");
            Property(x => x.escolaridad_nombre).HasColumnName("escolaridad_nombre");
            Property(x => x.p1_clave).HasColumnName("p1_clave");
            Property(x => x.p1_concepto).HasColumnName("p1_concepto");
            Property(x => x.p2_clave).HasColumnName("p2_clave");
            Property(x => x.p2_concepto).HasColumnName("p2_concepto");
            Property(x => x.p3_1_clave).HasColumnName("p3_1_clave");
            Property(x => x.p3_1_concepto).HasColumnName("p3_1_concepto");
            Property(x => x.p3_2_clave).HasColumnName("p3_2_clave");
            Property(x => x.p3_2_concepto).HasColumnName("p3_2_concepto");
            Property(x => x.p3_3_clave).HasColumnName("p3_3_clave");
            Property(x => x.p3_3_concepto).HasColumnName("p3_3_concepto");
            Property(x => x.p3_4_clave).HasColumnName("p3_4_clave");
            Property(x => x.p3_4_concepto).HasColumnName("p3_4_concepto");
            Property(x => x.p3_5_clave).HasColumnName("p3_5_clave");
            Property(x => x.p3_5_concepto).HasColumnName("p3_5_concepto");
            Property(x => x.p3_6_clave).HasColumnName("p3_6_clave");
            Property(x => x.p3_6_concepto).HasColumnName("p3_6_concepto");
            Property(x => x.p3_7_clave).HasColumnName("p3_7_clave");
            Property(x => x.p3_7_concepto).HasColumnName("p3_7_concepto");
            Property(x => x.p3_8_clave).HasColumnName("p3_8_clave");
            Property(x => x.p3_8_concepto).HasColumnName("p3_8_concepto");
            Property(x => x.p3_9_clave).HasColumnName("p3_9_clave");
            Property(x => x.p3_9_concepto).HasColumnName("p3_9_concepto");
            Property(x => x.p3_10_clave).HasColumnName("p3_10_clave");
            Property(x => x.p3_10_concepto).HasColumnName("p3_10_concepto");
            Property(x => x.p4_clave).HasColumnName("p4_clave");
            Property(x => x.p4_concepto).HasColumnName("p4_concepto");
            Property(x => x.p5_clave).HasColumnName("p5_clave");
            Property(x => x.p5_concepto).HasColumnName("p5_concepto");
            Property(x => x.p6_concepto).HasColumnName("p6_concepto");
            Property(x => x.p7_concepto).HasColumnName("p7_concepto");
            Property(x => x.p8_clave).HasColumnName("p8_clave");
            Property(x => x.p8_concepto).HasColumnName("p8_concepto");
            Property(x => x.p8_porque).HasColumnName("p8_porque");
            Property(x => x.p9_clave).HasColumnName("p9_clave");
            Property(x => x.p9_concepto).HasColumnName("p9_concepto");
            Property(x => x.p9_porque).HasColumnName("p9_porque");
            Property(x => x.p10_clave).HasColumnName("p10_clave");
            Property(x => x.p10_concepto).HasColumnName("p10_concepto");
            Property(x => x.p10_porque).HasColumnName("p10_porque");
            Property(x => x.p11_1_clave).HasColumnName("p11_1_clave");
            Property(x => x.p11_1_concepto).HasColumnName("p11_1_concepto");
            Property(x => x.p11_2_clave).HasColumnName("p11_2_clave");
            Property(x => x.p11_2_concepto).HasColumnName("p11_2_concepto");
            Property(x => x.p12_clave).HasColumnName("p12_clave");
            Property(x => x.p12_concepto).HasColumnName("p12_concepto");
            Property(x => x.p12_porque).HasColumnName("p12_porque");
            Property(x => x.p13_clave).HasColumnName("p13_clave");
            Property(x => x.p13_concepto).HasColumnName("p13_concepto");
            Property(x => x.p14_clave).HasColumnName("p14_clave");
            Property(x => x.p14_concepto).HasColumnName("p14_concepto");
            Property(x => x.p14_fecha).HasColumnName("p14_fecha");
            Property(x => x.p14_porque).HasColumnName("p14_porque");

            ToTable("tblRH_Baja_Entrevista");
        }
    }
}
