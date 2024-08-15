using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_AuditoriasMapping : EntityTypeConfiguration<tbl5s_Auditorias>
    {
        public tbl5s_AuditoriasMapping()
        {
            HasKey(x => x.id);
            HasRequired(x => x.checkList).WithMany().HasForeignKey(x => x.checkListId);
            ToTable("tbl5s_Auditorias");
        }
    }
}