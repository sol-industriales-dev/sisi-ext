using System;
using System.Collections.Generic;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class ContactoDTO
    {
        #region INIT
        public ContactoDTO()
        {
            lstFK_Clientes = new List<int>();
        }
        #endregion

        #region SQL
        public int id { get; set; }
        public int FK_Cliente { get; set; }
        public string nombreContacto { get; set; }
        public string puesto { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string extension { get; set; }
        public string celular { get; set; }
        public int FK_EstatusHistorial { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string division { get; set; }
        public string nombreCliente { get; set; }
        public string ubicacion { get; set; }
        public int FK_Municipio { get; set; }
        public int FK_Estado { get; set; }
        public int FK_Pais { get; set; }
        public bool esCrearClienteDesdeProyectos { get; set; }
        public int FK_Division { get; set; }
        public List<int> lstFK_Clientes { get; set; }
        #endregion
    }
}