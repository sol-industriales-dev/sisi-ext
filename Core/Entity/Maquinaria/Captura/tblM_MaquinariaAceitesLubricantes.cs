using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_MaquinariaAceitesLubricantes
    {
        public int id { get; set; }
        public string Economico { get; set; }
        public string CC { get; set; }
        public DateTime Fecha { get; set; }
        public int Turno { get; set; }
        public decimal Horometro { get; set; }
        public bool Rotacion { get; set; }
        public bool Sopleteo { get; set; }
        public bool AK { get; set; }
        public bool Lubricacion { get; set; }
        public decimal Antifreeze { get; set; }
        public int MotorId { get; set; }
        public decimal MotorVal { get; set; }
        public int TransmisionID { get; set; }
        public decimal TransmisionVal { get; set; }
        public int HidraulicoID { get; set; }
        public decimal HidraulicoVal { get; set; }
        public int DiferencialId { get; set; }
        public decimal DiferencialVal { get; set; }
        public int MFTIzqId { get; set; }
        public int MFTDerId { get; set; }
        public decimal MFTIzqVal { get; set; }
        public decimal MFTDerVal { get; set; }
        public int MDIzqID { get; set; }
        public int MDDerID { get; set; }
        public decimal MDIzqVal { get; set; }
        public decimal MDDerVal { get; set; }
        public int DirId { get; set; }
        public decimal DirVal { get; set; }
        public decimal Grasa { get; set; }
        public int GrasaId{ get; set; }
        public decimal GrasaVal { get; set; }
        public string Firma { get; set; }

        public int otroId1 { get; set; }
        public decimal otros1 { get; set; }
        public int otroId2 { get; set; }
        public decimal otros2 { get; set; }
        public int otroId3 { get; set; }
        public decimal otros3 { get; set; }
        public int otroId4 { get; set; }
        public decimal otros4 { get; set; }
    }
}
