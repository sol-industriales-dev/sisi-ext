using Core.Entity.ControlObra.MatrizDeRiesgo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.MatrizDeRiesgo
{
    public class tblCO_MatrizDeRiesgoDetMapping : EntityTypeConfiguration<tblCO_MatrizDeRiesgoDet>
    {
        public tblCO_MatrizDeRiesgoDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idMatrizDeRiesgo).HasColumnName("idMatrizDeRiesgo");
            Property(x => x.Historial).HasColumnName("Historial");
            Property(x => x.No).HasColumnName("No");
            Property(x => x.chAmenzaOportunidad).HasColumnName("chAmenzaOportunidad");
            Property(x => x.amenazaOportunidad).HasColumnName("amenazaOportunidad");
            Property(x => x.categoriaDelRiesgo).HasColumnName("categoriaDelRiesgo");
            Property(x => x.causaBasica).HasColumnName("causaBasica");
            Property(x => x.areaDelProyecto).HasColumnName("areaDelProyecto");
            Property(x => x.costoTiempoCalidad).HasColumnName("costoTiempoCalidad");
            Property(x => x.probabilidad).HasColumnName("probabilidad");
            Property(x => x.impacto).HasColumnName("impacto");
            Property(x => x.severidadInicial).HasColumnName("severidadInicial");
            Property(x => x.severidadActual).HasColumnName("severidadActual");
            Property(x => x.tipoDeRespuesta).HasColumnName("tipoDeRespuesta");
            Property(x => x.medidasATomar).HasColumnName("medidasATomar");
            Property(x => x.dueñoDelRiesgo).HasColumnName("dueñoDelRiesgo");
            Property(x => x.fechaDeCompromiso).HasColumnName("fechaDeCompromiso");
            Property(x => x.abiertoProcesoCerrado).HasColumnName("abiertoProcesoCerrado");


            ToTable("tblCO_MatrizDeRiesgoDet");
        }
    }
}
