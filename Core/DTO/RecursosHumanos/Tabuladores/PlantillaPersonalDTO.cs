using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class PlantillaPersonalDTO
    {
        #region SQL
        public int id { get; set; }
        public string cc { get; set; }
        public int FK_LineaNegocio { get; set; }
        public EstatusGestionAutorizacionEnum plantillaAutorizada { get; set; }
        public string comentarioRechazo { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string puestoDesc { get; set; }
        public int personalNecesario { get; set; }
        public int personalExistente { get; set; }
        public int porContratar { get; set; }
        public bool esFirmar { get; set; } //USUARIO SIGUIENTE A FIRMAR
        public string codeCC { get; set; }
        public string departamentoDesc { get; set; }
        public string nominaDesc { get; set; }
        public string categoriaDesc { get; set; }
        public string sueldoBaseDesc { get; set; }
        public string complementoDesc { get; set; }
        public string totalNominalDesc { get; set; }
        public string sueldoMensualDesc { get; set; }
        public string esquemaPagoDesc { get; set; }
        public List<int> lstPuestosDT { get; set; }
        public List<int> lstPuestosNuevos { get; set; }
        #endregion
    }
}
