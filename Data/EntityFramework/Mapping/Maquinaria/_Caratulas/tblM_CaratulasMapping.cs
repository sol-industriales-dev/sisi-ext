using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Caratulas;

namespace Data.EntityFramework.Mapping.Maquinaria.Caratula 
{
    public class tblM_CaratulasMapping : EntityTypeConfiguration<tblM_Caratulas>
    {
        public tblM_CaratulasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idGrupo).HasColumnName("idGrupo");
            Property(x => x.idModelo).HasColumnName("idModelo");
            Property(x => x.depreciacion).HasColumnName("depreciacion");
            Property(x => x.inversion).HasColumnName("inversion");
            Property(x => x.seguro).HasColumnName("seguro");
            Property(x => x.filtros).HasColumnName("filtros");
            Property(x => x.manoObra).HasColumnName("manoObra");
            Property(x => x.indirectosMatriz).HasColumnName("indirectosMatriz");
            Property(x => x.depreciacionOH).HasColumnName("depreciacionOH");
            Property(x => x.aceite).HasColumnName("aceite");
            Property(x => x.carilleria).HasColumnName("carilleria");
            Property(x => x.ansul).HasColumnName("ansul");
            Property(x => x.utilidad).HasColumnName("utilidad");
            Property(x => x.costoTotal).HasColumnName("costoTotal");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.idCC).HasColumnName("idCC");
            Property(x => x.mantenimientoCo).HasColumnName("mantenimientoCo");
            Property(x => x.tipoHoraDia).HasColumnName("tipoHoraDia");            
        
           

            //TABLAS VIRTUALES
            HasRequired(x => x.lstCatGrupo).WithMany().HasForeignKey(y => y.idGrupo);
            HasRequired(x => x.lstCatModelo).WithMany().HasForeignKey(y => y.idModelo);
            HasRequired(x => x.lstCC).WithMany().HasForeignKey(y => y.idCC);



            ToTable("tblM_Caratulas");

        }
    }
}
