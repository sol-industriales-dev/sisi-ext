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
    public class tblM_CaratulaAutMapping : EntityTypeConfiguration<tblM_CaratulaAut>
    {
        public tblM_CaratulaAutMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCaratula).HasColumnName("idCaratula");
            Property(x => x.esAutorizado).HasColumnName("esAutorizado");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.claveAutorizante).HasColumnName("claveAutorizante");            
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.nombreAutorizante).HasColumnName("nombreAutorizante");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.idAlerta).HasColumnName("idAlerta");
            HasRequired(x => x.lstCaratula).WithMany().HasForeignKey(y => y.idCaratula);
            HasRequired(x => x.lstUsuariosTecnico).WithMany().HasForeignKey(y => y.idUsuarioTecnico);
            HasRequired(x => x.lstUsuariosServicio).WithMany().HasForeignKey(y => y.idUsuarioServicio);
            HasRequired(x => x.lstUsuariosConstruccion).WithMany().HasForeignKey(y => y.idUsuarioConstruccion);
            
            ToTable("tblM_AutorizarCaratula");
        }
    }
}