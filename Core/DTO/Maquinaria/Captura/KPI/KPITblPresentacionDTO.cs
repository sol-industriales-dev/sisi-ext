using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.KPI
{
    public class KPITblPresentacionDTO
    {
        public string valor { get; set; }
        public string row { get; set; }
        public string col { get; set; }
        public int colSpan { get; set; }
        public int rowSpan { get; set; }
        public string clase { get; set; }
        public ColorDTO color { get; set; }
        public KPITblPresentacionDTO()
        {
            valor = string.Empty;
            clase = string.Empty;
            colSpan = 1;
            rowSpan = 1;
            color = new ColorDTO();
        }
    }
    public class ColorDTO
    {
        public int aplha { get; set; }
        public int red { get; set; }
        public int blue { get; set; }
        public int green { get; set; }
        public ColorDTO Amarillo()
        {
            return new ColorDTO
            {
                red = 255,
                green = 255,
                aplha = 1
            };
        }
        public ColorDTO Verde()
        {
            return new ColorDTO
            {
                red = 169,
                green = 208,
                blue = 142,
                aplha = 1
            };
        }
        public ColorDTO AzulRey()
        {
            return new ColorDTO
            {
                red = 47,
                green = 117,
                blue = 181,
                aplha = 1
            };
        }
        public ColorDTO AzulBajito()
        {
            return new ColorDTO
            {
                red = 221,
                green = 235,
                blue = 247,
                aplha = 1
            };
        }
        public ColorDTO VerdeClarito()
        {
            return new ColorDTO
            {
                red = 146,
                green = 208,
                blue = 80,
                aplha = 1
            };
        }
        public ColorDTO VerdeOscuro()
        {
            return new ColorDTO
            {
                red = 84,
                green = 130,
                blue = 53,
                aplha = 1
            };
        }
        public ColorDTO Naranja()
        {
            return new ColorDTO
            {
                red = 255,
                green = 192,
                aplha = 1
            };
        }
        public ColorDTO Gris()
        {
            return new ColorDTO
            {
                red = 217,
                green = 217,
                blue = 217,
                aplha = 1
            };
        }
        public ColorDTO GrisClaro()
        {
            return new ColorDTO
            {
                red = 237,
                green = 237,
                blue = 237,
                aplha = 1
            };
        }
        public ColorDTO GrisAzulito()
        {
            return new ColorDTO
            {
                red = 172,
                green = 185,
                blue = 202,
                aplha = 1
            };
        }
        public ColorDTO Rosa()
        {
            return new ColorDTO
            {
                red = 252,
                green = 228,
                blue = 214,
                aplha = 1
            };
        }
    }
}
