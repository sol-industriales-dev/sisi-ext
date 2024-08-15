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
    public class tblRH_BN_Incidencia_det_PeruMapping : EntityTypeConfiguration<tblRH_BN_Incidencia_det_Peru>
    {
        public tblRH_BN_Incidencia_det_PeruMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.incidenciaID).HasColumnName("incidenciaID");
            Property(x => x.incidencia_detID).HasColumnName("incidencia_detID");
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
            Property(x => x.archivoEvidencia).HasColumnName("archivoEvidencia");
            Property(x => x.usuarioCreacion).HasColumnName("usuarioCreacion");
            Property(x => x.usuarioModificacion).HasColumnName("usuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblRH_BN_Incidencia_det_Peru");
        }       
    }
}
