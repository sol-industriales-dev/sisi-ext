using System;

namespace Core.Entity.Principal.Bitacoras
{
    public class tblP_LogError
    {
        public long id { get; set; }
        /// <summary>
        /// Indica en que empresa sucedió el error. 1 para Construplan y 2 para Arrendadora.
        /// </summary>
        public int empresa { get; set; }
        /// <summary>
        /// Ejemplo: [Administracion, Seguimiento de Acuerdos, Procesos, Encuestas, etc.]
        /// El id del sistema se puede ver en la url al seleccionar alguno.
        /// </summary>
        public int sistema { get; set; }
        /// <summary>
        /// Identificador del menú.
        /// Ejemplo Al dar click sobre RH > Reporte Incidencias y darle inspeccionar (clic derecho)
        /// se podrá ver un atributo llamado data-menuid con el id del módulo.
        /// </summary>
        public int modulo { get; set; }
        public string controlador { get; set; }
        public string accion { get; set; }
        /// <summary>
        /// Mensaje de la excepción como tal.
        /// Ejemplo: e.Message
        /// </summary>
        public string mensaje { get; set; }
        /// <summary>
        /// Tipo de acción que se estaba intentando realizar al momento del error.
        /// El identificador de cada acción se encuentra en el archivo AccionEnum.
        /// </summary>
        public int tipo { get; set; }
        /// <summary>
        /// Identificador del usuario que estaba logueado al momento del error.
        /// </summary>
        public int usuarioID { get; set; }
        public DateTime fecha { get; set; }
        /// <summary>
        /// Identificador del objeto sí se estaba intentando actualizar o eliminar
        /// en la base de datos.
        /// </summary>
        public long registroID { get; set; }
        /// <summary>
        /// Objeto en formato JSON.
        /// </summary>
        public string objeto { get; set; }
        public string publicIP { get; set; }
        public string localIP { get; set; }
    }
}
