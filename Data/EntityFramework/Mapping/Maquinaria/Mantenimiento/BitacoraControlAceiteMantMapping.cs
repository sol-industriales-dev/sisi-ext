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
    class BitacoraControlAceiteMantMapping : EntityTypeConfiguration<tblM_BitacoraControlAceiteMant>
    {
        public BitacoraControlAceiteMantMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Hrsaplico).HasColumnName("Hrsaplico");
            Property(x => x.idComp).HasColumnName("idComp");
            Property(x => x.idMisc).HasColumnName("idMisc");
            Property(x => x.prueba).HasColumnName("prueba");
            Property(x => x.vidaActual).HasColumnName("vidaActual");
            Property(x => x.VidaRestante).HasColumnName("VidaRestante");
            Property(x => x.Vigencia).HasColumnName("Vigencia");
            Property(x => x.idMant).HasColumnName("idMant");
            Property(x => x.Aplicado).HasColumnName("Aplicado");
            Property(x => x.UsuarioCap).HasColumnName("UsuarioCap");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.alta).HasColumnName("alta");
            Property(x => x.idAct).HasColumnName("idAct");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblM_BitacoraControlAceiteMant");
        }

    }
}
