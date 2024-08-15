using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.ActoCondicion
{
    public class CondicionMapping : EntityTypeConfiguration<tblSAC_Condicion>
    {
        public CondicionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.rutaImagenAntes).HasColumnName("rutaImagenAntes");
            Property(x => x.rutaImagenDespues).HasColumnName("rutaImagenDespues");
            Property(x => x.fechaResolucion).HasColumnName("fechaResolucion");

            Property(x => x.claveInformo).HasColumnName("claveInformo");
            Property(x => x.nombreInformo).HasColumnName("nombreInformo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.clasificacionID).HasColumnName("clasificacionID");
            HasRequired(x => x.clasificacion).WithMany().HasForeignKey(x => x.clasificacionID);
            Property(x => x.procedimientoID).HasColumnName("procedimientoID");
            HasRequired(x => x.procedimientoViolado).WithMany().HasForeignKey(x => x.procedimientoID);
            Property(x => x.fechaSuceso).HasColumnName("fechaSuceso");
            Property(x => x.claveSupervisor).HasColumnName("claveSupervisor");
            Property(x => x.nombreSupervisor).HasColumnName("nombreSupervisor");
            Property(x => x.departamentoID).HasColumnName("departamentoID");
            HasRequired(x => x.departamento).WithMany().HasForeignKey(x => x.departamentoID);
            Property(x => x.subclasificacionDepID).HasColumnName("subclasificacionDepID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.nivelPrioridad).HasColumnName("nivelPrioridad");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            HasRequired(x => x.usuarioCreador).WithMany().HasForeignKey(x => x.usuarioCreadorID);
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");
            HasOptional(x => x.agrupacion).WithMany().HasForeignKey(x => x.idAgrupacion);
            Property(x => x.accionCorrectiva).HasColumnName("accionCorrectiva");
            HasOptional(x => x.prioridad).WithMany().HasForeignKey(x => x.nivelPrioridad);
            Property(x => x.firmaSupervisor).HasColumnName("firmaSupervisor");
            Property(x => x.firmaSST).HasColumnName("firmaSST");
            Property(x => x.claveSST).HasColumnName("claveSST");
            Property(x => x.nombreSST).HasColumnName("nombreSST");
            Property(x => x.fechaFirmado).HasColumnName("fechaFirmado");
            Property(x => x.clasificacionGeneralID).HasColumnName("clasificacionGeneralID");
            HasOptional(x => x.clasificacionGeneral).WithMany().HasForeignKey(x => x.clasificacionGeneralID);

            ToTable("tblSAC_Condicion");
        }
    }
}
