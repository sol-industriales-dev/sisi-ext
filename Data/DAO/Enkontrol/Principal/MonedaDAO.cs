using Core.DAO.Enkontrol.Principal;
using Core.DTO;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Enkontrol.Principal
{
    public class MonedaDAO : IMonedaDAO
    {
        UsuarioFactoryServices ufs = new UsuarioFactoryServices();
        /// <summary>
        /// Guarda Tipo de cambio de hoy
        /// </summary>
        /// <param name="moneda">ID</param>
        /// <param name="tc">Tipo de cambio</param>
        public void guardarTC(int moneda, decimal tc)
        {
            var ahora = DateTime.Now;
            var exist = (List<dynamic>)_contextEnkontrol.Where(string.Format("SELECT * FROM tipo_cambio WHERE moneda = '{0}' and fecha = {1}", moneda, ahora.ToString("yyyy-MM-dd"))).ToObject<List<dynamic>>();
            var isUpdate = exist.Count > 0;
            if(isUpdate)
                return;
            var usuario = vSesiones.sesionUsuarioDTO;
            var ekUsuario = ufs.getUsuarioService().getUserEk(usuario.id);
            var c = new Conexion();
            var con = c.Connect();
            if(con.State.Equals("Open"))
                con.Open();
            try
            {
                var consulta = string.Empty;
                var id = 0;
                consulta = @"INSERT INTO tipo_cambio
                                (moneda
                                ,fecha
                                ,tipo_cambio
                                ,empleado_modifica
                                ,fecha_modifica
                                ,hora_modifica)
                                VALUES (?,?,?,?,?,?,)";
                OdbcCommand command = new OdbcCommand(consulta);
                OdbcParameterCollection parameters = command.Parameters;
                parameters.Add("@moneda", OdbcType.Char).Value = moneda;
                parameters.Add("@fecha", OdbcType.Date).Value = ahora;
                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = tc;
                parameters.Add("@empleado_modifica", OdbcType.Numeric).Value = ekUsuario.empleado;
                parameters.Add("@fecha_modifica", OdbcType.Date).Value = ahora;
                parameters.Add("@hora_modifica", OdbcType.Date).Value = ahora;
                command.Connection = con;
                id = command.ExecuteNonQuery();
            }
            catch(Exception) { }
            finally
            {
                c.Close(con);
            }
        }
        /// <summary>
        /// Valida si el usuario tiene permiso de acceso a la ventana de tipo de cambio del sistema de enkontrol Orden de compra
        /// </summary>
        /// <returns>permiso de la vista</returns>
        public bool isUsuarioCambiarTC()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var ekUsuario = ufs.getUsuarioService().getUserEk(usuario.id);
                var res = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where(
                    "SELECT e.num, e.nom, m.nivel FROM ek010ab e INNER JOIN ek031ab_soc m ON m.nombre LIKE 'Tipo de cambio%' AND m.nivel = e.nivel WHERE e.num = " + ekUsuario.empleado
                    ).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                return res != null;
            }
            catch(Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Consulta el tipo de cambio de hoy
        /// </summary>
        /// <param name="moneda">ID</param>
        /// <returns>Tipo de cambio</returns>
        public decimal getTcHoy(int moneda)
        {
            if(moneda.Equals(1))
                return 1;
            try
            {
                var ahora = DateTime.Now;
                var tc = (List<dynamic>)_contextEnkontrol.Where(string.Format("SELECT tipo_cambio FROM tipo_cambio WHERE moneda = '{0}' and fecha = {1}", moneda, ahora.ToString("yyyy-MM-dd"))).ToObject<List<dynamic>>();
                return (decimal)tc[0].tipo_cambio;
            }
            catch(Exception)
            {
                return 1;
            }
        }
        #region combobx
        /// <summary>
        /// Lista de monedas
        /// </summary>
        /// <returns>Combobox de monedas</returns>
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboMonedaHoy()
        {
            try
            {
                var ahora = DateTime.Now.ToString("yyyy-MM-dd");
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where(
                    "SELECT m.clave AS Value, m.moneda AS Text, (SELECT t.tipo_cambio FROM tipo_cambio t WHERE t.moneda = m.clave and t.fecha = '" + ahora + "') AS Prefijo FROM moneda m"
                    ).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                return lst.Select(m => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = m.Value,
                    Text = m.Text,
                    Prefijo = m.Value.Equals("1") ? "1" : m.Prefijo ?? "0"
                }).ToList();
            }
            catch(Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        #endregion
    }
}
