using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class BackLogsDTO
    {
        public BackLogsDTO()
        {
            lstUsuarios = new List<int>();
            lstResponsables = new List<int>();
        }

        public int id { get; set; }
        public string folioBL { get; set; }
        public DateTime fechaInspeccion { get; set; }
        public string noEconomico { get; set; }
        public List<string> arrNoEconomicos { get; set; }
        public decimal horas { get; set; }
        public int idConjunto { get; set; }
        public string subconjunto { get; set; }
        public string conjunto { get; set; }
        public string conjuntoAbreviacion { get; set; }
        public int Mes { get; set; }
        public int idSubconjunto { get; set; }
        public bool parte { get; set; }
        public bool manoObra { get; set; }
        public string descripcion { get; set; }
        public string cc { get; set; }
        public string areaCuenta { get; set; }
        public int idEstatus { get; set; }
        public List<int> lstEstatus { get; set; }
        public string estatus { get; set; }
        public decimal totalMX { get; set; }
        public string numero { get; set; } //NUMERO REQUISICION DE ENKONTROL ARRENDADORA 01_9
        public int diasTotales { get; set; } //CANTIDAD DE DIAS TRANSCURRIDOS APARTIR DE LA FECHA DE CREACIÓN DEL BACKLOG (SE DEJA DE CALCULAR CUANDO HAY UNA FECHA DE CIERRE)
        public string centro_costo { get; set; } //CENTO DE COSTO DE LA TABLA si_area_cuenta DE ENKONTROL
        public DateTime fechaModificacionBL { get; set; } //SI EL BL SE ENCUENTRA INSTALADO, LA FECHA MODIFICACION ES SU FECHA DE CIERRE
        public int moneda { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal total { get; set; }
        public int tipoBL { get; set; }
        public int idSegPpto { get; set; }
        public decimal presupuestoEstimado { get; set; }
        public string folioPpto { get; set; }
        public string fechaActual { get; set; }
        public List<tblBL_Partes> lstPartes { get; set; }
        public int idCatMaquina { get; set; }
        public decimal horometro { get; set; }
        public bool esLiberado { get; set; }
        public DateTime fechaLiberado { get; set; }
        public int horasTerminacion { get; set; }
        public int idOT { get; set; }
        public int idMotivo { get; set; }
        public string motivo { get; set; }
        public DateTime fechaCreacionBL { get; set; }
        public int idResponsable { get; set; }
        public string responsable { get; set; }

        public string modelo { get; set; }
        public DateTime fechaUltimoBL { get; set; }
        public int cantidadBL { get; set; }
        public decimal horasBL { get; set; }
        public string top1Conjunto { get; set; }
        public string top2Conjunto { get; set; }
        public string top3Conjunto { get; set; }
        public int cantBL50OMenos { get; set; }
        public int cantBL70 { get; set; }
        public int cantBL100 { get; set; }
        public int cantBLTotal { get; set; }
        public int tiempoProm100 { get; set; }
        public int idUsuarioResponsable { get; set; }

        public int cantidadUsuario { get; set; }

        public int asd { get; set; }
        public decimal precio { get; set; }
        public int cantidad { get; set; }
        public decimal importe { get; set; }

        public DateTime fechaLiberadoBL { get; set; }


        public int idRe { get; set; }
        public string usu { get; set; }
        public int cantbl { get; set; }

        public int idSub { get; set; }

        public int cant0a40 { get; set; }
        public int cant41a60 { get; set; }
        public int cant61a80 { get; set; }
        public int cant81oMayor { get; set; }

        public int numeroSubConjunto { get; set; }
        public int insumo { get; set; }
        public List<int> lstUsuarios { get; set; }
        public List<int> lstMeses { get; set; }

        public int anio { get; set; }
        public DateTime fechaInstaladoBL { get; set; }
        public int tipoEquipoID { get; set; }
        public bool tienePartes { get; set; }
        public string descripcionBL { get; set; }
        public int numRequisicion { get; set; }
        public int numOC { get; set; }
        public string descripcionMaquina { get; set; }
        public string noSerie { get; set; }
        public string frente { get; set; }
        public List<int> lstResponsables { get; set; }
        public decimal costoTotalBL { get; set; }
        public string strCostoTotalBL { get; set; }
        public string strPresupuestoMes { get; set; }
        public string strPresupuestoAcumulado { get; set; }
        public decimal totalUSD { get; set; }
    }
}