using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_FormatoManteniento
    {
        public int id { get; set; }
        public int modeloEquipoID { get; set; }
        public int DocumentosMaquinariaID { get; set; }
        public int idAct { get; set; }
        public bool estado { get; set; }
        public int idPM { get; set; }
        public int idCatTipoActividad { get; set; }
        public int idDN { get; set; }
    }
}
