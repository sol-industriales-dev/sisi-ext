using Core.Entity.Maquinaria._Caratulas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria._Caratulas
{
    public class tblM_IndicadoresCaratulaMapping : EntityTypeConfiguration<tblM_IndicadoresCaratula>
    {
        public tblM_IndicadoresCaratulaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCC).HasColumnName("idCC");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.manoObra).HasColumnName("manoObra");
            Property(x => x.auxiliar).HasColumnName("auxiliar");
            Property(x => x.indirectos).HasColumnName("indirectos");            

            HasRequired(x => x.lstCC).WithMany().HasForeignKey(y => y.idCC);

            ToTable("tblM_IndicadoresCaratula");


        }
    }
}
