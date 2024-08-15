using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class RPTNotasCreditoDTO
    {
        public string Generador { get; set; }
        public string OC { get; set; }
        public string Equipo { get; set; }
        public string Modelo { get; set; }
        public string SerieEquipo { get; set; }


        public string SerieComponente { get; set; }
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public string CausaRemosion { get; set; }
        public string HorometroEquipo { get; set; }
        public string HorometroComponente { get; set; }
        public string MontoPesos { get; set; }
        public string MontoDLL { get; set; }
        public string AbonoDLL { get; set; }
        public string NoCredito { get; set; }

        public string Comentario { get; set; }
        public string GrupoMes { get; set; }
        public string DescripcionMes { get; set; }
        public string Anio { get; set; }
        public string fechaCierre { get; set; }
        public string Comentario2 { get; set; }
        public string TipoNC { get; set; }

        public string cc { get; set; }
        public string nombreDelUsuario { get; set; }
        public decimal montoTotalOC { get; set; }
    }
}
