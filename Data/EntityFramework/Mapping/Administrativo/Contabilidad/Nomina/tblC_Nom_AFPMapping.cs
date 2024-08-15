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
    public class tblC_Nom_AFPMapping : EntityTypeConfiguration<tblC_Nom_AFP>
    {
        public tblC_Nom_AFPMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipoComision).HasColumnName("tipoComision");
            Property(x => x.aporte).HasColumnName("aporte");
            Property(x => x.comision).HasColumnName("comision");
            Property(x => x.primaSeguro).HasColumnName("primaSeguro");
            Property(x => x.topePrimaSeguro).HasColumnName("topePrimaSeguro");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.usuarioRegistro).HasColumnName("usuarioRegistro");
            Property(x => x.fechaModifica).HasColumnName("fechaModifica");
            Property(x => x.usuarioModifica).HasColumnName("usuarioModifica");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            ToTable("tblC_Nom_AFP");
        }
    }
}