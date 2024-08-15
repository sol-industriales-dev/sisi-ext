using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PreNomina_AutMapping : EntityTypeConfiguration<tblC_Nom_PreNomina_Aut>
    {
        public tblC_Nom_PreNomina_AutMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.prenominaID).HasColumnName("nominaID");
            Property(x => x.aprobadorClave).HasColumnName("aprobadorClave");
            Property(x => x.aprobadorNombre).HasColumnName("aprobadorNombre");
            Property(x => x.aprobadorPuesto).HasColumnName("aprobadorPuesto");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.autorizando).HasColumnName("autorizando");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.esObra).HasColumnName("esObra");
            //HasRequired(x => x.aprobador).WithMany().HasForeignKey(y => y.aprobadorClave);
            //HasRequired(x => x.prenomina).WithMany().HasForeignKey(y => y.prenominaID);

            ToTable("tblC_Nom_PreNomina_Aut");
        }
    }
}
