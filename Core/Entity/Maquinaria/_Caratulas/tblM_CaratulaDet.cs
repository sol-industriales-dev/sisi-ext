using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Maquinaria.Catalogo;



namespace Core.Entity.Maquinaria._Caratulas
{
    public class tblM_CaratulaDet
    {
        public int id { get; set; }
        public int caratula { get; set; }
        public int idGrupo { get; set; }
        [ForeignKey("idGrupo")]
        public virtual tblM_CatGrupoMaquinaria lstCatGrupo { get; set; }
        public int idModelo { get; set; }
        [ForeignKey("idModelo")]
        public virtual tblM_CatModeloEquipo lstCatModelo { get; set; }
        public decimal depreciacionDLLS { get; set; }
        public decimal depreciacionMXN { get; set; }
        public decimal inversionDLLS { get; set; }
        public decimal inversionMXN { get; set; }
        public decimal seguroDLLS { get; set; }
        public decimal seguroMXN { get; set; }
        public decimal filtroDLLS { get; set; }
        public decimal filtroMXN { get; set; }
        public decimal mantenimientoDLLS { get; set; }
        public decimal mantenimientoMXN { get; set; }
        public decimal manoObraDLLS { get; set; }
        public decimal manoObraMXN { get; set; }
        public decimal auxiliarDLLS { get; set; }
        public decimal auxiliarMXN { get; set; }
        public decimal indirectosDLLS { get; set; }
        public decimal indirectosMXN { get; set; }
        public decimal depreciacionOHDLLS { get; set; }
        public decimal depreciacionOHMXN { get; set; }
        public decimal aceiteDLLS { get; set; }
        public decimal aceiteMXN { get; set; }
        public decimal carilleriaDLLS { get; set; }
        public decimal carilleriaMXN { get; set; }
        public decimal ansulDLLS { get; set; }
        public decimal ansulMXN { get; set; }
        public decimal utilidadDLLS { get; set; }
        public decimal utilidadMXN { get; set; }
        public decimal costoDLLS { get; set; }
        public decimal costoMXN { get; set; }
        public decimal tipoCambio { get; set; }
        public int idUsuarioTecnico { get; set; }
        [ForeignKey("idUsuarioTecnico")]
        public virtual tblP_Usuario lstUsuariosTecnico { get; set; }
        public int idUsuarioServicio { get; set; }
        [ForeignKey("idUsuarioServicio")]
        public virtual tblP_Usuario lstUsuariosServicio { get; set; }
        public int idUsuarioConstruccion { get; set; }
        [ForeignKey("idUsuarioConstruccion")]
        public virtual tblP_Usuario lstUsuariosConstruccion { get; set; }
        public int estatus { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public int tipoHoraDia { get; set; }
       

    }
}
