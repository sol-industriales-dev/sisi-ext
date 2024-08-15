using Core.Entity.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.KPI
{
    public class HomologadoMapping : EntityTypeConfiguration<tblM_KPI_Homologado>
    {
        public HomologadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.año).HasColumnName("año");
            Property(x => x.semana).HasColumnName("semana");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.turno).HasColumnName("turno");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.economico).HasColumnName("economico");
            Property(x => x.codigoParo).HasColumnName("codigoParo");
            Property(x => x.valor).HasColumnName("valor");
            Property(x => x.idParo).HasColumnName("idParo");
            Property(x => x.idGrupo).HasColumnName("idGrupo");
            Property(x => x.idModelo).HasColumnName("idModelo");
            Property(x => x.idEconomico).HasColumnName("idEconomico");
            Property(x => x.authEstado).HasColumnName("authEstado");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.idTipoParo).HasColumnName("idTipoParo");
            Property(x => x.validado).HasColumnName("validado");
            ToTable("tblM_KPI_Homologado");
        }
    }
}
