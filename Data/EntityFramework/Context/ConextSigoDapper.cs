using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.Odbc;
using Core.DTO.Utils.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Data.EntityFramework.Context
{
    public class ConextSigoDapper
    {

        public static string conexionSIGOPLAN()
        {
            string conexion = "";
            string User = "sa";
            string Password = "/*Construplan*/";
            string bd = "SIGOPLAN";
            string server = "10.1.0.110";
            conexion = "Data Source=" + server + ";Initial Catalog=" + bd + ";User ID=" + User + ";Password=" + Password + ";";
            return conexion;
        }
        public static string conexionSIGOPLANARRENDADORA()
        {
            string conexion = "";
            string User = "sa"; 
            string Password = "/*Construplan*/";
            string bd = "SIGOPLANARRENDADORA";
            string server = "10.1.0.110";
            conexion = "Data Source=" + server + ";Initial Catalog=" + bd + ";User ID=" + User + ";Password=" + Password + ";";
            return conexion;
        }
        public static string conexionSUBCONTRATISTAS()
        {
            string conexion = "";
            string User = "sa";
            string Password = "/*Construplan*/";
            string bd = "SUBCONTRATISTAS_GESTOR";
            string server = "10.1.0.110";
            conexion = "Data Source=" + server + ";Initial Catalog=" + bd + ";User ID=" + User + ";Password=" + Password + ";";
            return conexion;
        }
        public static string conexionSUBCONTRATISTASPRUEBA()
        {
            string conexion = "";
            string User = "sa";
            string Password = "/*Construplan*/";
            string bd = "SUBCONTRATISTAS_GESTORPRUEBAS";
            string server = "10.1.0.110";
            conexion = "Data Source=" + server + ";Initial Catalog=" + bd + ";User ID=" + User + ";Password=" + Password + ";";
            return conexion;
        }
        public static string conexionEnkontrol()
        {
            string strServerName = "serv_v8";
            string strUserName = "dba";
            string strPassword = "_._enkontrol_._";
            string strPort = "2639";
            string strHostName = "10.1.0.124";
            string dbName = "EK_ADM01_9";

            string con = "Driver={Adaptive Server Anywhere 9.0};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
            return con;
        }
        public static string conexionEnkontrolArrendadora()
        {
            string strServerName = "serv_v8";
            string strUserName = "dba";
            string strPassword = "_._enkontrol_._";
            string strPort = "2639";
            string strHostName = "10.1.0.124";
            string dbName = "EK_ADM04_9";

            string con = "Driver={Adaptive Server Anywhere 9.0};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
            return con;
        }
        public static string conexionEnkontrolArrendadoraVirtual()
        {
            string strServerName = "serv_v8";
            string strUserName = "dba";
            string strPassword = "_._enkontrol_._";
            string strPort = "2639";
            string strHostName = "10.1.0.124";
            string dbName = "EK_ADM12_9";

            string con = "Driver={Adaptive Server Anywhere 9.0};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
            return con;
        }
        public static string conexionEnkontrolRHNOm()
        {
            string strServerName = "serv_nomv8";
            string strUserName = "dba";
            string strPassword = "_._enkontrol_._";
            string strPort = "49160";
            string strHostName = "10.1.0.124";
            string dbName = "EK_NOM11_9";

            string con = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
            return con;
        }
        public static string conexionStarsoftBancos()
        {
            string conexion = "";
            string User = "sa";
            string Password = "/*Construplan*/";
            string bd = "003BDCBT2022";
            string server = "10.1.0.136";
            conexion = "Data Source=" + server + ";Initial Catalog=" + bd + ";User ID=" + User + ";Password=" + Password + ";";
            return conexion;
        }
        public static List<T> Select<T>(DapperDTO consulta)
        {
            string stringConection = VerificarConectionStringName(consulta);
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[stringConection].ConnectionString))
            {
                try
                {
                    return connection.Query<T>(consulta.consulta, consulta.parametros).ToList();
                }
                catch (Exception o_O)
                {
                    return new List<T>();
                }
            }
        }

        public static string VerificarConectionStringName(DapperDTO consulta)
        {
            string connectionString = "";
            switch (consulta.baseDatos)
            {
                case Core.Enum.Principal.MainContextEnum.Construplan:
                    connectionString = conexionSIGOPLAN();
                    break;
                case Core.Enum.Principal.MainContextEnum.Arrendadora:
                    connectionString = conexionSIGOPLANARRENDADORA();
                    break;
                case Core.Enum.Principal.MainContextEnum.Colombia:
                    connectionString = conexionEnkontrol();
                    break;
                case Core.Enum.Principal.MainContextEnum.RHConstruplan:
                    connectionString = conexionEnkontrol();
                    break;
            }
            return connectionString;
        }
    }
}
