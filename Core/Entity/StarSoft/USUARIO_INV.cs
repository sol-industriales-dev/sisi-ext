using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Usuario
{
    public class USUARIO_INV
    {
        //BD BDWENCO
        public string USU_CODIGO { get; set; }
        public string EMP_CODIGO { get; set; }
        public string USU_PASSWORD { get; set; }
        public string USU_NIVEL { get; set; }
        public string USU_NOMBRE { get; set; }
        public string TAALMA { get; set; }
        public bool? USU_NI_IMP { get; set; }
        public bool? USU_COSTO { get; set; }
        public bool? FlgPermiteAutorizarRequerimiento { get; set; }
        public string EMAIL { get; set; }
        public bool? FLGPORTAL_ALMACEN { get; set; }
        public string COD_AUDITORIA { get; set; }
        public bool? FLGPORTAL_RM { get; set; }
        public bool? USU_ESTADO { get; set; }
        public DateTime? USU_FEC_CREA { get; set; }
        public DateTime? USU_FEC_INACTIVO { get; set; }

    }
}
