using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class GraficaBarrasIndiceDTO
    {
        public List<string> lstCategorias { get; set; }

        #region GENERACIÓN DE RESIDUOS BIOLOGICO INFECCIOSOS
        public List<decimal> lstDataRBI { get; set; }
        #endregion

        #region GENERACIÓN DE RESIDUOS PELIGROSOS
        public List<decimal> lstDataRP { get; set; }
        #endregion

        #region GENERACIÓN DE RESIDUOS DE MANEJO ESPECIAL
        public List<decimal> lstDataRME { get; set; }
        #endregion

        #region GENERACIÓN DE RESIDUOS SOLIDOS URBANOS
        public List<decimal> lstDataRSU { get; set; }
        #endregion

        #region INDICE 2023
        public List<decimal> lstIndice2023 { get; set; }
        public List<decimal> lstAcumulado2023 { get; set; }
        #endregion

        #region INDICE 2022
        public List<decimal> lstIndice2022 { get; set; }
        public List<decimal> lstAcumulado2022 { get; set; }
        #endregion

        #region INDICE 2021
        public List<decimal> lstIndice2021 { get; set; }
        public List<decimal> lstAcumulado2021 { get; set; }
        #endregion

        #region INDICE 2020
        public List<decimal> lstIndice2020 { get; set; }
        public List<decimal> lstAcumulado2020 { get; set; }
        #endregion

        public GraficaBarrasIndiceDTO()
        {
            lstCategorias = new List<string>();
            lstDataRBI = new List<decimal>();
            lstDataRP = new List<decimal>();
            lstDataRME = new List<decimal>();
            lstDataRSU = new List<decimal>();
            lstIndice2023 = new List<decimal>();
            lstAcumulado2023 = new List<decimal>();
            lstIndice2022 = new List<decimal>();
            lstAcumulado2022 = new List<decimal>();
            lstIndice2021 = new List<decimal>();
            lstAcumulado2021 = new List<decimal>();
            lstIndice2020 = new List<decimal>();
            lstAcumulado2020 = new List<decimal>();
        }
    }
}
