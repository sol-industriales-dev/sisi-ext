using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_AuditoriasArchivosMapping : EntityTypeConfiguration<tbl5s_AuditoriasArchivos>
    {
        public tbl5s_AuditoriasArchivosMapping()
        {
            HasKey(x => x.id);
            ToTable("tbl5s_AuditoriasArchivos");
        }
    }
}