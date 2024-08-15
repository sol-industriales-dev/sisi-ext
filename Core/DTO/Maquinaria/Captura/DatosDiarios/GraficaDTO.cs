using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.DatosDiarios
{
    public class GraficaDTO
    {
        public List<string> categorias { get; set; }
        public string serie1Descripcion { get; set; }
        public List<decimal> serie1 { get; set; }
        public string serie2Descripcion { get; set; }
        public List<decimal> serie2 { get; set; }
        public string serie3Descripcion { get; set; }
        public List<decimal> serie3 { get; set; }

        #region Dashboard Estatus Diario
        public List<string> listaCausa { get; set; }
        public List<string> listaFechaInicial { get; set; }
        public List<string> listaFechaProyectada { get; set; }
        public List<string> listaAcciones { get; set; }
        #endregion

        public GraficaDTO()
        {
            categorias = new List<string>();
            serie1 = new List<decimal>();
            serie2 = new List<decimal>();
            serie3 = new List<decimal>();

            #region Dashboard Estatus Diario
            listaCausa = new List<string>();
            listaFechaInicial = new List<string>();
            listaFechaProyectada = new List<string>();
            listaAcciones = new List<string>();
            #endregion
        }
    }
}
