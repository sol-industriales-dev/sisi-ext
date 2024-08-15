using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion
{
    public class FiltroActoCondicionDTO
    {
        public string cc { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public int claveSupervisor { get; set; }
        public int departamentoID { get; set; }
        public int subclasificacionDepID { get; set; }
        public int estatus { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
        public int clasificacionID { get; set; }
    }
}
