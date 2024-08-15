using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class QueryHomologadosDTO
    {
        public tblM_KPI_Homologado homolo { get; set; }
        public tblM_KPI_CodigosParo codigo { get; set; }
        public tblM_CatMaquina maquina { get; set; }
        public tblM_CatModeloEquipo modelo { get; set; }
        public tblM_CatGrupoMaquinaria grupo { get; set; }
    }
}
