﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class CatConjuntosDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string abreviacion{ get; set; }
        public bool esActivo { get; set; }
    }
}
