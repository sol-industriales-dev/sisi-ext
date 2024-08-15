using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_CorteInventarioMaq_Detalle
    {
        public int Id { get; set; }
        public int IdCorteInvMaq { get; set; }
        public string Economico { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public string Año { get; set; }
        public string Ubicacion { get; set; }
        public string Redireccion { get; set; }
        public string cc { get; set; }
        public string ccCargoObra { get; set; }
        public string CargoObra { get; set; }
        public string Resguardante { get; set; }
        public string HorometroAcumulado { get; set; }
        public string Tipo { get; set; }
        public int IdEconomico { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("IdCorteInvMaq")]
        public virtual tblM_CorteInventarioMaq Corte { get; set; }

        [ForeignKey("IdEconomico")]
        public virtual tblM_CatMaquina Maquina { get; set; }
        public string Empresa { get; set; }
        public string EstatusDatosDiarios { get; set; }
    }
}