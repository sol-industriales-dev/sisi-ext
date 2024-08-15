using Core.Entity.Maquinaria._Caratulas;
using Core.Entity.Maquinaria.Caratulas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria._Caratulas
{
    public class tblM_CaratulaDetMapping : EntityTypeConfiguration<tblM_CaratulaDet>
    {
        public tblM_CaratulaDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idGrupo).HasColumnName("idGrupo");
            Property(x => x.idModelo).HasColumnName("idModelo");
            Property(x => x.depreciacionDLLS).HasColumnName("depreciacionDLLS");
            Property(x => x.depreciacionMXN).HasColumnName("depreciacionMXN");
            Property(x => x.inversionDLLS).HasColumnName("inversionDLLS");
            Property(x => x.inversionMXN).HasColumnName("inversionMXN");
            Property(x => x.seguroDLLS).HasColumnName("SeguroDLLS");
            Property(x => x.seguroMXN).HasColumnName("SeguroMXN");
            Property(x => x.filtroDLLS).HasColumnName("filtroDLLS");
            Property(x => x.filtroMXN).HasColumnName("filtroMXN");
            Property(x => x.mantenimientoDLLS).HasColumnName("mantenimientoDLLS");
            Property(x => x.mantenimientoMXN).HasColumnName("mantenimientoMXN");
            Property(x => x.manoObraDLLS).HasColumnName("manoObraDLLS");
            Property(x => x.manoObraMXN).HasColumnName("manoObraMXN");
            Property(x => x.auxiliarDLLS).HasColumnName("auxiliarDLLS");
            Property(x => x.auxiliarMXN).HasColumnName("auxiliarMXN");
            Property(x => x.indirectosDLLS).HasColumnName("indirectosDLLS");
            Property(x => x.indirectosMXN).HasColumnName("indirectosMXN");
            Property(x => x.depreciacionOHDLLS).HasColumnName("depreciacionOHDLLS");
            Property(x => x.depreciacionOHMXN).HasColumnName("depreciacionOHMXN");
            Property(x => x.aceiteDLLS).HasColumnName("aceiteDLLS");
            Property(x => x.aceiteMXN).HasColumnName("aceiteMXN");
            Property(x => x.carilleriaDLLS).HasColumnName("carilleriaDLLS");
            Property(x => x.carilleriaMXN).HasColumnName("carilleriaMXN");
            Property(x => x.ansulDLLS).HasColumnName("ansulDLLS");
            Property(x => x.ansulMXN).HasColumnName("ansulMXN");
            Property(x => x.utilidadDLLS).HasColumnName("utilidadDLLS");
            Property(x => x.utilidadMXN).HasColumnName("utilidadMXN");
            Property(x => x.costoDLLS).HasColumnName("costoDLLS");
            Property(x => x.costoMXN).HasColumnName("costoMXN");
            Property(x => x.idUsuarioTecnico).HasColumnName("idUsuarioTecnico");
            Property(x => x.idUsuarioServicio).HasColumnName("idUsuarioServicio");
            Property(x => x.idUsuarioConstruccion).HasColumnName("idUsuarioConstruccion");
            Property(x => x.tipoCambio).HasColumnName("tipoCambio");            
            Property(x => x.caratula).HasColumnName("caratula");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.tipoHoraDia).HasColumnName("tipoHoraDia");            

            HasRequired(x => x.lstUsuariosTecnico).WithMany().HasForeignKey(y => y.idUsuarioTecnico);
            HasRequired(x => x.lstUsuariosServicio).WithMany().HasForeignKey(y => y.idUsuarioServicio);
            HasRequired(x => x.lstUsuariosConstruccion).WithMany().HasForeignKey(y => y.idUsuarioConstruccion);

            HasRequired(x => x.lstCatGrupo).WithMany().HasForeignKey(y => y.idGrupo);
            HasRequired(x => x.lstCatModelo).WithMany().HasForeignKey(y => y.idModelo);
            ToTable("tblM_GuardarCaratula");

        }
    }
}
