using Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQ_Archivo : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public DateTime fechaCarga { get; set; }
        public string ubicacionArchivo { get; set; }
        public string nombreArchivo { get; set; }
    }
}
