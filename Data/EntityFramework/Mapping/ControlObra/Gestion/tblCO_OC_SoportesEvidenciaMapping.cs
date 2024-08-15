using Core.Entity.ControlObra.GestionDeCambio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.Gestion
{
    public class tblCO_OC_SoportesEvidenciaMapping : EntityTypeConfiguration<tblCO_OC_SoportesEvidencia>
    {
        public tblCO_OC_SoportesEvidenciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idOrdenDeCambio).HasColumnName("idOrdenDeCambio");
            Property(x => x.alcancesNuevos).HasColumnName("alcancesNuevos");
            Property(x => x.modificacionesPorCambio).HasColumnName("modificacionesPorCambio");
            Property(x => x.requerimientosDeCampo).HasColumnName("requerimientosDeCampo");
            Property(x => x.ajusteDeVolumenes).HasColumnName("ajusteDeVolumenes");
            Property(x => x.serviciosYSuministros).HasColumnName("serviciosYSuministros");
            Property(x => x.fechaInicial).HasColumnName("fechaInicial");
            Property(x => x.FechaFinal).HasColumnName("FechaFinal");
            Property(x => x.alcancesNuevosDescripcion).HasColumnName("alcancesNuevosDescripcion");
            Property(x => x.modificacionesPorCambioDescripcion).HasColumnName("modificacionesPorCambioDescripcion");
            Property(x => x.requerimientosDeCampoDescripcion).HasColumnName("requerimientosDeCampoDescripcion");
            Property(x => x.ajusteDeVolumenesDescripcion).HasColumnName("ajusteDeVolumenesDescripcion");
            Property(x => x.serviciosYSuministrosDescripcion).HasColumnName("serviciosYSuministrosDescripcion");
            Property(x => x.fechaDescripcion).HasColumnName("fechaDescripcion");

            ToTable("tblCO_OC_SoportesEvidencia");
        }
    }
}
