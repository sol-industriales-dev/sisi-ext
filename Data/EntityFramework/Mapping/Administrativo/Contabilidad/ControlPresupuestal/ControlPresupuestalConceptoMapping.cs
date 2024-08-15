using Core.Entity.Administrativo.Contabilidad.ControlPresupuestal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.ControlPresupuestal
{
    public class ControlPresupuestalConceptoMapping : EntityTypeConfiguration<tblM_ControlPresupuestalConcepto>
    {
        public ControlPresupuestalConceptoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblM_ControlPresupuestalConcepto");
        }
    }
}
