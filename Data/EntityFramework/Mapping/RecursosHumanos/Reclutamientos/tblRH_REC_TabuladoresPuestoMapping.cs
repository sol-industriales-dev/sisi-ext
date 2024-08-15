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
    public class tblRH_REC_TabuladoresPuestoMapping : EntityTypeConfiguration<tblRH_REC_TabuladoresPuesto>
    {
        public tblRH_REC_TabuladoresPuestoMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tabulador).HasColumnName("tabulador");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.salario_base).HasColumnName("salario_base");
            Property(x => x.complemento).HasColumnName("complemento");
            Property(x => x.bono_de_zona).HasColumnName("bono_de_zona");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");

            ToTable("tblRH_REC_TabuladoresPuesto");
        }
    }
}