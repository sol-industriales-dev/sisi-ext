using Core.Entity.Principal.Usuarios;
using Core.Enum.GestorCorporativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.GestorCorporativo
{
    public class tblGC_Archivo
    {
        public long id { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public int nivel { get; set; }
        public DateTime fechaCreacion { get; set; }
        public bool esCarpeta { get; set; }
        public int orden { get; set; }
        public long padreID { get; set; }
        public int usuarioCreadorID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public bool activo { get; set; }
        public GrupoCarpetaEnum grupoCarpeta { get; set; }
        public SubGrupoCarpetaEnum subGrupoCarpeta { get; set; }

        public override string ToString()
        {
            return String.Format("Nombre: {0} Orden: {1} Nivel: {2} GrupoCarpeta: {3}", nombre, orden, nivel, grupoCarpeta);
        }
    }
}
