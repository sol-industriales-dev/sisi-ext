using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    class tblC_Nom_CatSolicitudChequeMapping : EntityTypeConfiguration<tblC_Nom_CatSolicitudCheque>
    {
        public tblC_Nom_CatSolicitudChequeMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoSolicitudCheque).HasColumnName("tipoSolicitudCheque");
            Property(x => x.tipoSolicitudChequeDescripcion).HasColumnName("tipoSolicitudChequeDescripcion");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.bancoDescripcion).HasColumnName("bancoDescripcion");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            ToTable("tblC_Nom_CatSolicitudCheque");
        }
    }
}