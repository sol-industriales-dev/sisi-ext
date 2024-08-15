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
    public class tblM_CaratulaMapping : EntityTypeConfiguration<tblM_Caratula>
    {
        public tblM_CaratulaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCaratula).HasColumnName("idCaratula");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.autorizada).HasColumnName("autorizada");
            Property(x => x.usuario).HasColumnName("usuario");
            Property(x => x.tipoCambio).HasColumnName("tipoCambio");

            HasRequired(x => x.lstCaratula).WithMany().HasForeignKey(y => y.idCaratula);

            ToTable("tblM_CaratulaPadre");
        }
    }
}
