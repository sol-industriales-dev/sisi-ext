using Core.Enum.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.FileManager
{
    public class tblFM_Archivo_Base
    {
        #region SQL
        public long id { get; set; }
        public long padreID { get; set; }
        public int nivel { get; set; }
        public int año { get; set; }
        public int divisionID { get; set; }
        public int subdivisionID { get; set; }
        public int ccID { get; set; }
        public bool esCarpeta { get; set; }
        public int orden { get; set; }
        public bool perteneceSeguridad { get; set; }
        public TipoCarpetaEnum tipoCarpeta { get; set; }
        #endregion
    }
}
