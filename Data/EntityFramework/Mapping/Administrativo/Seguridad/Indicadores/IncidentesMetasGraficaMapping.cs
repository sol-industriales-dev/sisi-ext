using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Indicadores
{
    public class IncidentesMetasGraficaMapping : EntityTypeConfiguration<tblS_IncidentesMetasGrafica>
    {
        public IncidentesMetasGraficaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.valor).HasColumnName("valor");
            Property(x => x.año).HasColumnName("año");
            Property(x => x.colorString).HasColumnName("colorString");
            Property(x => x.tipoGrafica).HasColumnName("tipoGrafica");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            ToTable("tblS_IncidentesMetasGrafica");
        }
    }

}
