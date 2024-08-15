using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    /// <summary>
    /// Relacion de Encuestas de encuestas con usuarios que dan seguimiento a los clientes externos.
    /// </summary>
    public class tblEN_EncuestaAsignaUsuario
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public int usuarioID { get; set; }
    }
}
