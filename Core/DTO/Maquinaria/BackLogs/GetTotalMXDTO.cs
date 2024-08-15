using Core.Entity.Administrativo.Almacen;
using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class GetTotalMXDTO
    {
        public string areaCuenta { get; set; }
        public int idBL { get; set; }
        public string noEconomico { get; set; }
    }
}