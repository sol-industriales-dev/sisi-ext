using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CatMaquina_DocumentosAplica
    {
        public int id { get; set; }
        public string grupoDesc { get; set; }
        public int grupoID { get; set; }
        public virtual tblM_CatGrupoMaquinaria grupo { get; set; }
        public bool factura { get; set; }
        public bool pedimento { get; set; }
        public bool polizaSeguro { get; set; }
        public bool tarjetaCirculacion { get; set; }
        public bool permisoCarga { get; set; }
        public bool certificacion { get; set; }
        public bool cuadroComparativo { get; set; }
        public bool contratos { get; set; }
        public bool ansul { get; set; }
    }
}
