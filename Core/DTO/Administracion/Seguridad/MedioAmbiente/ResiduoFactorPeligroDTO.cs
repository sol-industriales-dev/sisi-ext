using Core.Enum.Administracion.Seguridad.MedioAmbiente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class ResiduoFactorPeligroDTO
    {
        public int id { get; set; }
        public FactorPeligroEnum factorPeligro { get; set; }
        public string factorPeligroDesc { get; set; }
        public int residuoID { get; set; }
    }
}
