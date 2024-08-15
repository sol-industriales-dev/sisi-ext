using System;
using System.Collections.Generic;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class ProyectoDTO
    {
        #region SQL
        public int id { get; set; }
        public string nombreProyecto { get; set; }
        public int FK_Cliente { get; set; }
        public int FK_Prioridad { get; set; }
        public int FK_Division { get; set; }
        public int FK_Municipio { get; set; }
        public decimal importeCotizadoAprox { get; set; }
        public DateTime fechaInicio { get; set; }
        public int FK_Estatus { get; set; }
        public int FK_Escenario { get; set; }
        public int FK_UsuarioResponsable { get; set; }
        public int FK_Riesgo { get; set; }
        public string descripcionObra { get; set; }
        public bool esProspecto { get; set; }
        public int FK_Canal { get; set; }
        public int FK_EstatusHistorial { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public bool esModuloClientes { get; set; }
        public string nombreCliente { get; set; }
        public string nombreContacto { get; set; }
        public string nombreCompletoResponsable { get; set; }
        public string division { get; set; }
        public string ubicacion { get; set; }
        public string estatus { get; set; }
        public string prioridad { get; set; }
        public string riesgo { get; set; }
        public string strImporteCotizadoAprox { get; set; }
        public string escenario { get; set; }
        public int FK_Estado { get; set; }
        public int FK_Pais { get; set; }
        public bool esCrearClienteDesdeProyectos { get; set; }
        public string tipoCliente { get; set; }
        public string htmlContactos { get; set; }
        public int FK_Contacto { get; set; }
        public int FK_Estatus_Prospeccion { get; set; }
        public int FK_Estatus_LaborVenta { get; set; }
        public int FK_Estatus_Cotizacion { get; set; }
        public int FK_Estatus_Negociacion { get; set; }
        public int FK_Estatus_Cierre { get; set; }
        public string canal { get; set; }
        public bool esSeleccionado { get; set; }
        #endregion
    }
}