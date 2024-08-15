namespace Core.DTO.Maquinaria.BackLogs
{
    public class RelAreaCuentaAlmacenDetDTO
    {
        public int id { get; set; }
        public int idRelacion { get; set; }
        public int Almacen { get; set; }
        public int Prioridad { get; set; }
        public int TipoAlmacen { get; set; }
    }
}