﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
   public class tblCom_Retenciones_Interna
    {
        public int id { get; set; }
        public string retencion { get; set; }

        public virtual List<tblCom_OCRetenciones_Interna> retencionesOC { get; set; }
    }
}
