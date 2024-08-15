using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class RemocionDTO
    {
        public string noEconomico { get; set; }
        public string modelo { get; set; }
        public decimal horas { get; set; }
        public string serieMaquina { get; set; }
        public string descripcionComponente { get; set; }
        public string numParteComponente { get; set; }
        public string serieComponenteRemovido { get; set; }
        public decimal horasComponenteRemovido { get; set; }
        public int modeloID { get; set; }
        public int subconjuntoID { get; set; }
        public bool tipoLocacion { get; set; }
        public string cc { get; set; }
        public string nombreCC { get; set; }
        public int idModelo { get; set; }
        public string fecha { get; set; }

        public DateTime fechaNum { get; set; }
        public int componenteRemovidoID { get; set; }
        public int maquinaID { get; set; }
        public string ccID { get; set; }
        public bool garantia { get; set; }

        public int motivoID { get; set; }
        public int destinoID { get; set; }
        public string comentario { get; set; }
        public int componenteInstaladoID { get; set; }
        public int ? empresaResponsable { get; set; }
        public string personal { get; set; }
        public string imgRemovido { get; set; }
        public string imgInstalado { get; set; }
        public int folioReporte { get; set; }
        public int estatus { get; set; }
        public DateTime fechaInstalacionRemovidoRaw { get; set; }
        public string fechaInstalacionRemovido { get; set; }
        public DateTime fechaInstalacionInstaladoRaw { get; set; }
        public string fechaInstalacionInstalado { get; set; }
        public string ultimaReparacion { get; set; }
        public int? empresaInstala { get; set; }
        public string componenteInstalado { get; set; }

        //public string serieComponenteInstalado { get; set; }
    }
}