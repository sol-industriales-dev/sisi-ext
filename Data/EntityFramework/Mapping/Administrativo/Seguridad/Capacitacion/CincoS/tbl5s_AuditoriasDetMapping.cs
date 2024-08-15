using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_AuditoriasDetMapping : EntityTypeConfiguration<tbl5s_AuditoriasDet>
    {
        public tbl5s_AuditoriasDetMapping()
        {
            HasKey(x => x.id);
            HasRequired(x => x.auditoria).WithMany().HasForeignKey(x => x.auditoriaId);
            ToTable("tbl5s_AuditoriasDet");
        }
    }
}