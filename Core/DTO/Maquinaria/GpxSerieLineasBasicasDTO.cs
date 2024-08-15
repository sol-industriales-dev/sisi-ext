using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria
{
    public class GpxSerieLineasBasicasDTO
    {
        public string name { get; set; }
        public List<decimal> data { get; set; }

        public GpxSerieLineasBasicasDTO()
        {
            this.data = new List<decimal>();
        }
    }
}