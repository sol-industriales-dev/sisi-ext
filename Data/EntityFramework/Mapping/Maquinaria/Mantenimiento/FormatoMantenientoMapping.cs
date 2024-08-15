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
    class FormatoMantenientoMapping:EntityTypeConfiguration<tblM_FormatoManteniento>
    {
        public FormatoMantenientoMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.DocumentosMaquinariaID).HasColumnName("DocumentosMaquinariaID");
            Property(x => x.idAct).HasColumnName("idAct");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.idPM).HasColumnName("idPM");
            Property(x => x.idCatTipoActividad).HasColumnName("idCatTipoActividad");
            Property(x => x.idDN).HasColumnName("idDN");
            ToTable("tblM_FormatoManteniento");
        }
    }
}