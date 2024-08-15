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
    public class tblRH_REC_TabuladoresMapping : EntityTypeConfiguration<tblRH_REC_Tabuladores>
    {
        public tblRH_REC_TabuladoresMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idEK).HasColumnName("idEK");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.id_puesto).HasColumnName("id_puesto");
            Property(x => x.salario_base).HasColumnName("salario_base");
            Property(x => x.complemento).HasColumnName("complemento");
            Property(x => x.bono_de_zona).HasColumnName("bono_de_zona");
            Property(x => x.bono_trab_especiales).HasColumnName("bono_trab_especiales");
            Property(x => x.bono_por_produccion).HasColumnName("bono_por_produccion");
            Property(x => x.bono_otros).HasColumnName("bono_otros");
            Property(x => x.hora_extra).HasColumnName("hora_extra");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.nomina).HasColumnName("nomina");
            Property(x => x.libre).HasColumnName("libre");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");

            ToTable("tblRH_REC_Tabuladores");
        }
    }
}