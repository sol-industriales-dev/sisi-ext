using System;
using System.Collections.Generic;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class ClienteDTO
    {
        #region INIT
        public ClienteDTO()
        {
            lstID_Clientes = new List<int>();
        }
        #endregion

        #region SQL
        public int id { get; set; }
        public string nombreCliente { get; set; }
        public int FK_Division { get; set; }
        public int FK_Municipio { get; set; }
        public string paginaWeb { get; set; }
        public int FK_TipoCliente { get; set; }
        public int FK_EstatusHistorial { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string division { get; set; }
        public string ubicacion { get; set; }
        public bool esCrearClienteDesdeProyectos { get; set; }
        public int FK_Pais { get; set; }
        public int FK_Estado { get; set; }
        public List<int> lstID_Clientes { get; set; }
        #endregion
    }
}