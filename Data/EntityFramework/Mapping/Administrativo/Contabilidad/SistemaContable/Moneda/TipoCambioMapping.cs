using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.SistemaContable.Moneda
{
    public class TipoCambioMapping : EntityTypeConfiguration<tblC_SC_TipoCambio>
    {
        public TipoCambioMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Moneda).HasColumnName("Moneda");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.TipoCambio).HasColumnName("TipoCambio").HasPrecision(22,6);
            Property(x => x.Empleado_modifica).HasColumnName("Empleado_modifica");
            Property(x => x.FechaModifica).HasColumnName("FechaModifica");
            Property(x => x.HoraModifica).HasColumnName("HoraModifica");
            Property(x => x.TcAnterior).HasColumnName("TcAnterior");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            HasRequired(x => x.Divisa).WithMany().HasForeignKey(y => y.Moneda);
            ToTable("tblC_SC_TipoCambio");
        }
    }
}
