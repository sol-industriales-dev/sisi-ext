using Core.Entity.Administrativo.Contabilidad.Facturas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Facturas
{

    public class tblF_EK_InsumosMapping : EntityTypeConfiguration<tblF_EK_Insumos>
    {
        public tblF_EK_InsumosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblF_EK_Insumos");
        }
    }
}
