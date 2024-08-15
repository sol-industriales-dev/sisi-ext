using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    class BitacoraControlDNMapping : EntityTypeConfiguration<tblM_BitacoraControlActExt>
    {
        public BitacoraControlDNMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Hrsaplico).HasColumnName("Hrsaplico");
            Property(x => x.idAct).HasColumnName("idAct");
            Property(x => x.idMant).HasColumnName("idMant");
            Property(x => x.idPerioricidad).HasColumnName("idPerioricidad");
            Property(x => x.vidaActual).HasColumnName("vidaActual");
            Property(x => x.vidaRestante).HasColumnName("vidaRestante");
            Property(x => x.idMant).HasColumnName("idMant");
            Property(x => x.Aplicado).HasColumnName("Aplicado");
            Property(x => x.UsuarioCap).HasColumnName("UsuarioCap");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.alta).HasColumnName("alta");
            ToTable("tblM_BitacoraControlActExt");
        } 

    }
}
