using System;

namespace Core.Entity.Administrativo.Comercializacion.CRM
{
    public class tblAF_CRM_Cotizaciones
    {
        #region SQL
        public int id { get; set; }
        public int FK_Proyecto { get; set; }
        public int FK_ResponsableCotizacion { get; set; }
        public decimal importeFinal { get; set; }
        public DateTime fechaFinal { get; set; }
        public int importeRevN { get; set; }
        public DateTime fechaRevN { get; set; }
        public decimal importeOriginal { get; set; }
        public DateTime fechaOriginal { get; set; }
        public string comentario { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}