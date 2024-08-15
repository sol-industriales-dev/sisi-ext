using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class chkAlmacenDTO
    {
        public List<Insumogrupo> grupo { get; set; }
        public int tipo { get; set; }
        public List<int> almacen { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
        public decimal? cantidad { get; set; }
    }
    public class Insumogrupo
    {
        public int compania { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
    }
    public class chkAlmacenSalidaDTO
    {
        public List<int> companias { get; set; }
        public int compania { get; set; }
        public int almacen { get; set; }
        public int periodo { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
        public decimal importe { get; set; }
    }
    public class chkValuacion
    {
        public string label { get; set; }
        public int Key { get; set; }
        public decimal total { get; set; }
    }
}
