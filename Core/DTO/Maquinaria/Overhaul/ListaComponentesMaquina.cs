using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ListaComponentesMaquina
    {
        public int idLocacion;
        public string descripcionLocacion;
        public int modeloEquipoID;
        public string cc;
        public List<tblM_CatComponente> componente;
        public bool tipoLocacion;
        //public int locacion;
        public DateTime ? fecha;
        //public List<DateTime ?> fechasInstalacion = new List<DateTime ?>();
    }
}