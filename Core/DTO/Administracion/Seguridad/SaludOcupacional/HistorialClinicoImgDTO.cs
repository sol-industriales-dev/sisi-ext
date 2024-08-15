using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class HistorialClinicoImgDTO
    {

        public byte[] perfil { get; set; }
        public byte[] graficaEspirometria { get; set; }
        public byte[] graficaAudiometria { get; set; }
        public byte[] graficaElectrocardiograma1 { get; set; }
        public byte[] graficaElectrocardiograma2 { get; set; }
        public byte[] radiografia1 { get; set; }
        public byte[] radiografia2 { get; set; }
        public byte[] radiografia3 { get; set; }
        public byte[] tableLaboratorio { get; set; }
        public byte[] firma { get; set; }
        public byte[] documentoAdjunto1 { get; set; }
        public byte[] documentoAdjunto2 { get; set; }
        public byte[] documentoAdjunto3 { get; set; }

    }
}
