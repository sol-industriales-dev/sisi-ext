using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    class tblM_ComparativoFinancieroAutorizanteMapping: EntityTypeConfiguration<tblM_ComparativoFinancieroAutorizante>
    {
        public tblM_ComparativoFinancieroAutorizanteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAsignacion).HasColumnName("idAsignacion");
            Property(x => x.idFinanciero).HasColumnName("idFinanciero");
            Property(x => x.autorizanteID).HasColumnName("autorizanteID");
            Property(x => x.autorizanteNombre).HasColumnName("autorizanteNombre");
            Property(x => x.autorizantePuesto).HasColumnName("autorizantePuesto");
            Property(x => x.autorizanteStatus).HasColumnName("autorizanteStatus");
            Property(x => x.autorizanteFinal).HasColumnName("autorizanteFinal");
            Property(x => x.autorizanteFecha).HasColumnName("autorizanteFecha");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.comentario).HasColumnName("comentario");
             

            ToTable("tblM_ComparativoFinancieroAutorizante");
        }
    }
}
