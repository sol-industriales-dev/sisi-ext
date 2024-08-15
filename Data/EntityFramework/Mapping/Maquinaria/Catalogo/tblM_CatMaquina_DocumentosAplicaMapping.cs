using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{

    class tblM_CatMaquina_DocumentosAplicaMapping : EntityTypeConfiguration<tblM_CatMaquina_DocumentosAplica>
    {
        public tblM_CatMaquina_DocumentosAplicaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.grupoDesc).HasColumnName("grupoDesc");
            Property(x => x.grupoID).HasColumnName("grupoID");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.pedimento).HasColumnName("pedimento");
            Property(x => x.polizaSeguro).HasColumnName("polizaSeguro");
            Property(x => x.tarjetaCirculacion).HasColumnName("tarjetaCirculacion");
            Property(x => x.permisoCarga).HasColumnName("permisoCarga");
            Property(x => x.certificacion).HasColumnName("certificacion");
            Property(x => x.cuadroComparativo).HasColumnName("cuadroComparativo");
            Property(x => x.contratos).HasColumnName("contratos");

            HasRequired(x => x.grupo).WithMany().HasForeignKey(y => y.grupoID);

            ToTable("tblM_CatMaquina_DocumentosAplica");

        }

    }
}
