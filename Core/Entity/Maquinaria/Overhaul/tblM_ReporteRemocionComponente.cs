using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Newtonsoft.Json;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_ReporteRemocionComponente
    {
        public int id { get; set; }
        public DateTime fechaRemocion { get; set; }
        public int componenteRemovidoID { get; set; }
        public int componenteInstaladoID { get; set; }
        public int maquinaID { get; set; }
        public string areaCuenta { get; set; }
        public int motivoRemocionID { get; set; }
        public int destinoID { get; set; }
        public string comentario { get; set; }
        public bool garantia { get; set; }
        public int ? empresaResponsable { get; set; }
        public string personal { get; set; }
        public string imgComponenteRemovido { get; set; }
        public string imgComponenteInstalado { get; set; }
        public int estatus { get; set; }
        public int ? empresaInstala { get; set; }
        public DateTime ? fechaInstalacionCRemovido { get; set; }
        public DateTime ?  fechaInstalacionCInstalado { get; set; }
        public DateTime ?  fechaUltimaReparacion { get; set; }
        public DateTime ? fechaVoBo { get; set; }
        public DateTime ? fechaEnvio { get; set; }
        public DateTime ? fechaAutorizacion { get; set; }
        public int realiza { get; set; }
        public decimal horasComponente { get; set; }
        public decimal horasMaquina { get; set; }
        public string JsonEvidencia { get; set; }
        public int trackID { get; set; }

        public virtual tblM_CatComponente componenteRemovido { get; set; }
        public virtual tblM_CatComponente componenteInstalado { get; set; }
        public virtual tblM_CatMaquina maquina { get; set; }
        public virtual tblM_CatLocacionesComponentes destino { get; set; }
    }
}
