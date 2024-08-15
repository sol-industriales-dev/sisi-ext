using Core.DTO;
using Core.Entity.ControlObra;
using Core.Entity.ControlObra.GestionDeCambio;
using Core.Entity.ControlObra.MatrizDeRiesgo;
using Core.Entity.Maquinaria.Reporte;
using Core.Entity.Principal.Alertas;
using Core.Entity.SubContratistas.Usuarios;
using Core.Entity.SubContratistas;
using Core.Entity.SubContratistas.Menus;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.ControlObra.Evaluacion;
using Core.Entity.Encuestas;

namespace Data.EntityFramework.Context
{
    public class contextSigoplan : DbContext
    {

        #region SubContratistas
        public DbSet<tblX_DocumentacionFija> tblX_DocumentacionFija { get; set; }
        public DbSet<tblX_SubContratista> tblX_SubContratista { get; set; }
        public DbSet<tblX_RelacionSubContratistaDocumentacion> tblX_RelacionSubContratistaDocumentacion { get; set; }
        public DbSet<tblX_Contrato> tblX_Contrato { get; set; }
        public DbSet<tblX_ContratotblCOES_Especialidad> tblX_ContratotblCOES_Especialidad { get; set; }
        public DbSet<tblX_Proyecto> tblX_Proyecto { get; set; }
        public DbSet<tblX_Cliente> tblX_Cliente { get; set; }
        public DbSet<tblCO_foliador> tblCO_foliador { get; set; }
        public DbSet<tblX_AnexoContrato> tblX_AnexoContrato { get; set; }
        public DbSet<tblX_ContratoPeriodo> tblX_ContratoPeriodo { get; set; }
        public DbSet<tblX_CapturaMensual> tblX_CapturaMensual { get; set; }
        public DbSet<tblX_CapturaMensualArchivo> tblX_CapturaMensualArchivo { get; set; }
        public DbSet<tblX_PeriodoCaptura> tblX_PeriodoCaptura { get; set; }
        public DbSet<tblX_ExcepcionPeriodoCaptura> tblX_ExcepcionPeriodoCaptura { get; set; }
        public DbSet<tblX_ArchivoMensual> tblX_ArchivoMensual { get; set; }
        public DbSet<tblX_Cliente_AnexoContrato> tblX_Cliente_AnexoContrato { get; set; }
        public DbSet<tblX_Cliente_ContratoPeriodo> tblX_Cliente_ContratoPeriodo { get; set; }
        public DbSet<tblX_Cliente_CapturaMensual> tblX_Cliente_CapturaMensual { get; set; }
        public DbSet<tblX_Cliente_CapturaMensualArchivo> tblX_Cliente_CapturaMensualArchivo { get; set; }
        public DbSet<tblX_EstatusContrato> tblX_EstatusContrato { get; set; }
        public DbSet<tblX_TipoBloqueo> tblX_TipoBloqueo { get; set; }
        public DbSet<tblX_Firmante> tblX_Firmante { get; set; }
        public DbSet<tblX_FirmaEvaluacion> tblX_FirmaEvaluacion { get; set; }
        public DbSet<tblX_FirmaEvaluacionDetalle> tblX_FirmaEvaluacionDetalle { get; set; }
        public DbSet<tblP_Usuarios> tblP_Usuario { get; set; }
        public DbSet<tblP_Encabezado> tblP_Encabezado { get; set; }
        #endregion
        public DbSet<tblP_Permisos> tblP_Permisos { get; set; }

        public DbSet<tblCO_ADP_EvalSubConAsignacion> tblCO_ADP_EvalSubConAsignacion { get; set; }
        public DbSet<tblCO_OC_GestionFirmas> tblCO_OC_GestionFirmas { get; set; }
        public DbSet<tblCO_OC_RelClavesEmpleadosEntreBD> tblCO_OC_RelClavesEmpleadosEntreBD { get; set; }
        public DbSet<tblCO_OC_Notificantes> tblCO_OC_Notificantes { get; set; }
        public DbSet<tblCO_OrdenDeCambio> tblCO_OrdenDeCambio { get; set; }
        public DbSet<tblCO_OC_Firmas> tblCO_OC_Firmas { get; set; }
        public DbSet<tblCO_OC_ArchivosFirmas> tblCO_OC_ArchivosFirmas { get; set; }
        public DbSet<tblCO_OC_SoportesEvidencia> tblCO_OC_SoportesEvidencia { get; set; }
        public DbSet<tblCO_OC_Montos> tblCO_OC_Montos { get; set; }
        public DbSet<tblP_Alerta> tblP_Alerta { get; set; }
        public DbSet<tblCO_ADP_EvaluadorXcc> tblCO_ADP_EvaluadorXcc { get; set; }
        public DbSet<tblCO_ADP_EvaluacionDiv> tblCO_ADP_EvaluacionDiv { get; set; }
        public DbSet<tblCO_ADP_EvalSubContratista> tblCO_ADP_EvalSubContratista { get; set; }
        public DbSet<tblCO_ADP_EvalSubContratistaDet> tblCO_ADP_EvalSubContratistaDet { get; set; }
        public DbSet<tblCO_ADP_EvaluacionPlantilla> tblCO_ADP_EvaluacionPlantilla { get; set; }
        public DbSet<tblCO_ADP_EvaluacionReq> tblCO_ADP_EvaluacionReq { get; set; }
        public DbSet<tblCO_ADP_EvaluacionRel> tblCO_ADP_EvaluacionRel { get; set; }
        public DbSet<tblCO_ADP_Notificante> tblCO_ADP_Notificante { get; set; }
        public DbSet<tblCO_ADP_Facultamientos> tblCO_ADP_Facultamientos { get; set; }
        public DbSet<tblEN_Estrellas> tblEN_Estrellas { set; get; }
        
        #region MATRIZ DE RIESGO
        public DbSet<tblCO_MatrizDeRiesgo> tblCO_MatrizDeRiesgo { get; set; }
        public DbSet<tblCO_MatrizDeRiesgoDet> tblCO_MatrizDeRiesgoDet { get; set; }
        public DbSet<tblCO_MR_ImpractosSobreObjetivosDelProyecto> tblCO_MR_ImpractosSobreObjetivosDelProyecto { get; set; }
        public DbSet<tblCO_MR_TiposDeRepuestaRiesgo> tblCO_MR_TiposDeRepuestaRiesgo { get; set; }
        #endregion

        public DbSet<tblCOES_Plantilla> tblCOES_Plantilla { get; set; }
        public DbSet<tblCOES_Elemento> tblCOES_Elemento { get; set; }
        public DbSet<tblCOES_Requerimiento> tblCOES_Requerimiento { get; set; }
        public DbSet<tblCOES_PlantillatblX_Contrato> tblCOES_PlantillatblX_Contrato { get; set; }
        public DbSet<tblCOES_PlantillatblCOES_Elemento> tblCOES_PlantillatblCOES_Elemento { get; set; }
        public DbSet<tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento> tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento { get; set; }
        public DbSet<tblCOES_Evaluador> tblCOES_Evaluador { get; set; }
        public DbSet<tblCOES_Evaluador_Proyecto> tblCOES_Evaluador_Proyecto { get; set; }
        public DbSet<tblCOES_Evaluador_Elemento> tblCOES_Evaluador_Elemento { get; set; }
        public DbSet<tblCOES_Facultamiento> tblCOES_Facultamiento { get; set; }
        public DbSet<tblCOES_Facultamiento_CentroCosto> tblCOES_Facultamiento_CentroCosto { get; set; }
        public DbSet<tblCOES_FirmaSubcontratista> tblCOES_FirmaSubcontratista { get; set; }
        public DbSet<tblCOES_FirmaSubcontratistatblX_Contrato> tblCOES_FirmaSubcontratistatblX_Contrato { get; set; }
        public DbSet<tblCOES_Especialidad> tblCOES_Especialidad { get; set; }
        public DbSet<tblCOES_EspecialidadtblX_SubContratista> tblCOES_EspecialidadtblX_SubContratista { get; set; }
        public DbSet<tblCOES_Asignacion> tblCOES_Asignacion { get; set; }
        public DbSet<tblCOES_Asignacion_Evaluacion> tblCOES_Asignacion_Evaluacion { get; set; }
        public DbSet<tblCOES_Evidencia> tblCOES_Evidencia { get; set; }
        public DbSet<tblCOES_Firma> tblCOES_Firma { get; set; }
        public DbSet<tblCOES_FirmaGerente> tblCOES_FirmaGerente { get; set; }
        public DbSet<tblCOES_CambioEvaluacion> tblCOES_CambioEvaluacion { get; set; }

        public contextSigoplan()
            : base(vSesiones.sesionEmpresaActual == 1 ? "MainContextSigoplan" : "MainContextSigoplan")
        {
            //Disable initializer
            Database.SetInitializer<contextSigoplan>(null);
        }
        public contextSigoplan(int bd) : base(bd == 1 ? "MaintSubContratistas" : "MainContextSigoplan")
        {
            //Disable initializer
            Database.SetInitializer<contextSigoplan>(null);
            Database.CommandTimeout = 5000;
        }

        public contextSigoplan(EmpresaEnum bd)
        {
            var connectionString = getConectionStringName(bd);
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            Database.SetInitializer<contextSigoplan>(null);
            Database.CommandTimeout = 5000;
        }
        public contextSigoplan(string cadenaConexion)
        {
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings[cadenaConexion].ConnectionString;
            Database.SetInitializer<contextSigoplan>(null);
            Database.CommandTimeout = 5000;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => !String.IsNullOrEmpty(type.Namespace))
                    .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
                foreach (var type in typesToRegister)
                {
                    dynamic configurationInstancie = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(configurationInstancie);
                }

                //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
                base.OnModelCreating(modelBuilder);
            }
            catch (Exception e)
            {

            }
        }
        #region Auxiliares
        protected string getConectionStringName(EmpresaEnum id)
        {
            var connectionString = "MaintSubContratistas";
            return connectionString;
        }
        #endregion

    }
}
