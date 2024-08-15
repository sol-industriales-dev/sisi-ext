using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_SolicitudEquipoDet
    {
        public int id { get; set; }
        public int solicitudEquipoID { get; set; }
        public string folio { get; set; }
        public int tipoMaquinariaID { get; set; }
        public int grupoMaquinariaID { get; set; }
        public int modeloEquipoID { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int horas { get; set; }
        public int prioridad { get; set; }
        public bool estatus { get; set; }
        public string Comentario { get; set; }
        public virtual tblM_SolicitudEquipo SolicitudEquipo { get; set; }
        public virtual tblM_CatGrupoMaquinaria GrupoMaquinaria { get; set; }
        public virtual tblM_CatModeloEquipo ModeloEquipo { get; set; }
        public virtual tblM_CatTipoMaquinaria TipoMaquinaria { get; set; }

        public int tipoUtilizacion { get; set; }

    }
}
