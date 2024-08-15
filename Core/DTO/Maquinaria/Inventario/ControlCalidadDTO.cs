using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class ControlCalidadDTO
    {
        public ControlCalidadDTO()
        {
            objControlCalidad = new tblM_CatControlCalidad();
            objInfoMaquina = new tblM_CatMaquina();
        }
        public tblM_CatControlCalidad objControlCalidad { get; set; }
        public tblM_CatMaquina objInfoMaquina { get; set; }
        public List<tblM_CatGrupoPreguntasCalidad> lstGrupos { get; set; }
        public List<tblM_CatPreguntasCalidad> lstPreguntas { get; set; }
        public List<tblM_RelPreguntaControlCalidad> lstRespuestas { get; set; }
        public int solicitudID { get; set; }
        public string areaCuentaOrigen { get; set; }
        public string areaCuentaDestino { get; set; }

    }
}
