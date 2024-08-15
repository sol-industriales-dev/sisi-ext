using Core.Enum.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class RequerimientoDTO
    {
        public int relPE_id { get; set; } //ID de tblCOES_PlantillatblCOES_Elemento
        public int relPER_id { get; set; } //ID de tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento
        public int requerimiento_id { get; set; }
        public int elemento_id { get; set; }
        public string elementoDesc { get; set; }
        public string mensaje { get; set; }
        public bool critico { get; set; }
        public string criticoDesc { get; set; }
        public decimal ponderacion { get; set; }
        public string requerimientoDesc { get; set; }
        public TipoRequerimientoEnum tipoRequerimiento { get; set; }
        public string tipoRequerimientoDesc { get; set; }

        public int evaluacion_id { get; set; }
        public string descripcion { get; set; }
        public List<EvidenciaDTO> evidencias { get; set; }
        public EstatusEvidenciaEnum estatusUltimaEvidencia { get; set; }
        public string estatusUltimaEvidenciaDesc { get; set; }
        public EstatusEvidenciaEnum estatusUltimaEvidenciaInicial { get; set; }
        public string estatusUltimaEvidenciaInicialDesc { get; set; }
        public decimal calificacionUltimaEvidencia { get; set; }
        public int ponderacionUltimaEvidencia { get; set; }
        public string nombreUltimaEvidencia { get; set; }

        public string cc { get; set; }
        public int subcontratista_id { get; set; }
    }
}
