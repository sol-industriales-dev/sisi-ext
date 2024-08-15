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
    class BitacoraControlAceiteMantMappingProy : EntityTypeConfiguration<tblM_BitacoraControlAceiteMantProy>
    {
        public BitacoraControlAceiteMantMappingProy()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Hrsaplico).HasColumnName("Hrsaplico");
            Property(x => x.Vigencia).HasColumnName("Vigencia");
            Property(x => x.idComp).HasColumnName("idComp");
            Property(x => x.idMisc).HasColumnName("idMisc");
            Property(x => x.prueba).HasColumnName("prueba");
            Property(x => x.programado).HasColumnName("programado");
            Property(x => x.idMant).HasColumnName("idMant");
            Property(x => x.Observaciones).HasColumnName("Observaciones");
            Property(x => x.FechaServicio).HasColumnName("FechaServicio");
            Property(x => x.UsuarioCap).HasColumnName("UsuarioCap");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");

            Property(x => x.aplicado).HasColumnName("aplicado");
            Property(x => x.idAct).HasColumnName("idAct");
            ToTable("tblM_BitacoraControlAceiteMantProy");
        }

    }
}
