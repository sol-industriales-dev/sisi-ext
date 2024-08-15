using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Usuarios
{
    public class UsuarioMapping : EntityTypeConfiguration<tblP_Usuario>
    {
        public UsuarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.apellidoPaterno).HasColumnName("apellidoPaterno");
            Property(x => x.apellidoMaterno).HasColumnName("apellidoMaterno");
            Property(x => x.correo).HasColumnName("correo");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.nombreUsuario).HasColumnName("nombreUsuario");
            Property(x => x.contrasena).HasColumnName("contrasena");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.perfilID).HasColumnName("perfilID");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.cveEmpleado).HasColumnName("cveEmpleado");
            HasRequired(x => x.perfil).WithMany().HasForeignKey(y => y.perfilID);
            HasMany(x => x.permisos).WithMany(x => x.usuarios).Map(m =>
            {
                m.ToTable("tblP_MenutblP_Usuario");
                m.MapLeftKey("tblP_Menu_id");
                m.MapRightKey("tblP_Usuario_id");

            });
            HasRequired(x => x.puesto).WithMany().HasForeignKey(y => y.puestoID);
            HasMany(x => x.permisosPorVista).WithMany(x => x.usuarios).Map(m =>
            {
                m.ToTable("tblP_AccionesVistatblP_Usuario");
                m.MapLeftKey("tblP_AccionesVista_id");
                m.MapRightKey("tblP_Usuario_id");

            });
            Property(x => x.enviar).HasColumnName("enviar");
            Property(x => x.cliente).HasColumnName("cliente");
            Property(x => x.tipoSGC).HasColumnName("tipoSGC");
            Property(x => x.usuarioSGC).HasColumnName("usuarioSGC");
            Property(x => x.tipoSeguridad).HasColumnName("tipoSeguridad");
            Property(x => x.usuarioSeguridad).HasColumnName("usuarioSeguridad");
            Property(x => x.usuarioMAZDA).HasColumnName("usuarioMAZDA");
            Property(x => x.dashboardMaquinariaPermiso).HasColumnName("dashboardMaquinariaPermiso");
            Property(x => x.dashboardMaquinariaPermiso).HasColumnName("dashboardMaquinariaAdmin");
            Property(x => x.esAuditor).HasColumnName("esAuditor");
            Property(x => x.externoSeguridad).HasColumnName("externoSeguridad");
            Property(x => x.sistemasGenerales).HasColumnName("sistemasGenerales");
            Property(x => x.gestionRH).HasColumnName("gestionRH");
            Property(x => x.usuarioGeneral).HasColumnName("usuarioGeneral");
            Property(x => x.externoGestor).HasColumnName("externoGestor");
            Property(x => x.esColombia).HasColumnName("esColombia");
            Property(x => x.isBajio).HasColumnName("isBajio");
            Property(x => x.externoPatoos).HasColumnName("externoPatoos");
            Property(x => x.externoPatoosNombre).HasColumnName("externoPatoosNombre");
            Property(x => x.tipoPatoos).HasColumnName("tipoPatoos");
            ToTable("tblP_Usuario");
        }
    }
}
