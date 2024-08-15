using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class ProyectoMapping : EntityTypeConfiguration<tblX_Proyecto>
    {
        public ProyectoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numeroContrato).HasColumnName("numeroContrato");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.fechaSuscripcion).HasColumnName("fechaSuscripcion");
            Property(x => x.fechaVigencia).HasColumnName("fechaVigencia");
            Property(x => x.montoContractual).HasColumnName("montoContractual");
            Property(x => x.anticipoAplica).HasColumnName("anticipoAplica");
            Property(x => x.anticipoPorcentaje).HasColumnName("anticipoPorcentaje");
            Property(x => x.penalizacion).HasColumnName("penalizacion");
            Property(x => x.clienteID).HasColumnName("clienteID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblX_Proyecto");
        }
    }
}
