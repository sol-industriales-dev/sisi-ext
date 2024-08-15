using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class EficienciaMapping : EntityTypeConfiguration<tblM_Eficiencia>
    {
        public EficienciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.IdEquipo).HasColumnName("IdEquipo");
            Property(x => x.IdGrupo).HasColumnName("IdGrupo");
            Property(x => x.IdTipoEquipo).HasColumnName("IdTipoEquipo");
            Property(x => x.IdObra).HasColumnName("IdObra");
            Property(x => x.Turno).HasColumnName("Turno");
            Property(x => x.HrsInicial).HasColumnName("HrsInicial");
            Property(x => x.HrsFinal).HasColumnName("HrsFinal");
            Property(x => x.HrsTrabajada).HasColumnName("HrsTrabajada");
            Property(x => x.FallaTrenRodaje).HasColumnName("FallaTrenRodaje");
            Property(x => x.FallaElectrica).HasColumnName("FallaElectrica");
            Property(x => x.FallaHidraulica).HasColumnName("FallaHidraulica");
            Property(x => x.FallaOtros).HasColumnName("FallaOtros");
            Property(x => x.FallaOperacion).HasColumnName("FallaOperacion");
            Property(x => x.FaltaOperador).HasColumnName("FaltaOperador");
            Property(x => x.TramoFMat).HasColumnName("TramoFMat");
            Property(x => x.TramoFDat).HasColumnName("TramoFDat");
            Property(x => x.TramoFAvan).HasColumnName("TramoFAvan");
            Property(x => x.TramoIClie).HasColumnName("TramoIClie");
            Property(x => x.HrsTotal).HasColumnName("HrsTotal");
            Property(x => x.HrsTotalReparacion).HasColumnName("HrsTotalReparacion");
            Property(x => x.Paro).HasColumnName("Paro");
            Property(x => x.Comentarios).HasColumnName("Comentarios");
            Property(x => x.Semana).HasColumnName("Semana");
            Property(x => x.HrsBase).HasColumnName("HrsBase");
            Property(x => x.HrsDiferencia).HasColumnName("HrsDiferencia");
            Property(x => x.Economico).HasColumnName("Economico");
            ToTable("tblM_Eficiencia"); 
        }
    }
}

