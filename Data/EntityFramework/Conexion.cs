using Core.DTO;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework
{
    public class Conexion
    {
        public Conexion()
        {
        }

        public OdbcConnection Connect()
        {
            OdbcConnection con = null;
            if (vSesiones.sesionEmpresaActual != 3 && vSesiones.sesionEmpresaActual != 11)
            {
                String strServerName = "serv_v8";
                String strUserName = "dba";
                String strPassword = "_._enkontrol_._";
                String strPort = "2639";
                String strHostName = "10.1.0.124";
                String dbName = vSesiones.sesionEmpresaActual == 1 ? "EK_ADM01_9" : vSesiones.sesionEmpresaActual == 2 ? "EK_ADM04_9" : vSesiones.sesionEmpresaActual == 4 ? "EK_ADM06_9" : vSesiones.sesionEmpresaActual == 5 ? "EK_ADM03_9" : vSesiones.sesionEmpresaActual == 6 ? "EK_ADM01_9" : vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN ? "EK_ADM05_9" : "UN TEXTO PARA QUE NO SE CONECTE";
                String strErrorMsg;

                strErrorMsg = String.Empty;
                try
                {
                    String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                    con = new OdbcConnection(conString);
                    con.Open();
                }
                catch (Exception exp)
                {
                    con = null;
                    strErrorMsg = exp.ToString();
                }
            }
            else
            {
                if (vSesiones.sesionEmpresaActual == 3)
                {
                    String strServerName = "Serv_v9";
                    String strUserName = "con123";
                    String strPassword = "con123";
                    String strPort = "2640";
                    String strHostName = "10.1.0.135";
                    String dbName = "EK_ADM20_11";
                    String strErrorMsg;

                    strErrorMsg = String.Empty;
                    try
                    {
                        String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";

                        con = new OdbcConnection(conString);
                        con.Open();
                    }
                    catch (Exception exp)
                    {
                        con = null;
                        strErrorMsg = exp.ToString();
                    }
                }
                else if (vSesiones.sesionEmpresaActual == 11) 
                {
                    String strServerName = "serv_v8_99";
                    String strUserName = "dba";
                    String strPassword = "_._enkontrol_._";
                    String strPort = "49158";
                    String strHostName = "10.1.0.124";
                    String dbName = "EK_ADM99_9";
                    String strErrorMsg;

                    strErrorMsg = String.Empty;
                    try
                    {
                        String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                        con = new OdbcConnection(conString);
                        con.Open();
                    }
                    catch (Exception exp)
                    {
                        con = null;
                        strErrorMsg = exp.ToString();
                    }
                }
            }
            return con;
        }
        public OdbcConnection Connect(int empresa)
        {
            OdbcConnection con = null;
            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = empresa == 1 ? "EK_ADM01_9" : (empresa == 2 ? "EK_ADM04_9" : (empresa == 8 ? "EK_ADM05_9" : "UN TEXTO PARA QUE NO SE CONECTE"));
            String strErrorMsg;

            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }
            return con;
        }
        public OdbcConnection ConnectConstruplan()
        {
            OdbcConnection con = null;
            if (vSesiones.sesionEmpresaActual != 3)
            {
                String strServerName = "serv_v8";
                String strUserName = "dba";
                String strPassword = "_._enkontrol_._";
                String strPort = "2639";
                String strHostName = "10.1.0.124";
                String dbName = vSesiones.sesionEmpresaActual == 1 ? "EK_ADM01_9" : vSesiones.sesionEmpresaActual == 2 ? "EK_ADM04_9" : vSesiones.sesionEmpresaActual == 4 ? "EK_ADM06_9" : vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN ? "EK_ADM05_9" : "UN TEXTO PARA QUE NO SE CONECTE";
                String strErrorMsg;


                strErrorMsg = String.Empty;
                try
                {
                    String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                    con = new OdbcConnection(conString);
                    con.Open();
                }
                catch (Exception exp)
                {
                    con = null;
                    strErrorMsg = exp.ToString();
                }
            }
            else
            {
                String strServerName = "Serv_v9";
                String strUserName = "con123";
                String strPassword = "con123";
                String strPort = "2640";
                String strHostName = "10.1.0.135";
                String dbName = "EK_ADM20_11";
                String strErrorMsg;

                strErrorMsg = String.Empty;
                try
                {
                    String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                    con = new OdbcConnection(conString);
                    con.Open();
                }
                catch (Exception exp)
                {
                    con = null;
                    strErrorMsg = exp.ToString();
                }
            }

            return con;
        }
        public OdbcConnection ConnectConstruplanConstruplan()
        {
            OdbcConnection con = null;

            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM01_9";
            String strErrorMsg;

            strErrorMsg = String.Empty;

            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }
        public OdbcConnection ConnectCplanProductivo()
        {
            OdbcConnection con = null;
            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM01_9";
            String strErrorMsg;

            strErrorMsg = String.Empty;
            try
            {
                //String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";

                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }
            return con;
        }
        public OdbcConnection ConnectColombiaProductivo()
        {
            OdbcConnection con = null;
            String strServerName = "Serv_v9";
            String strUserName = "con123";
            String strPassword = "con123";
            String strPort = "2640";
            String strHostName = "10.1.0.135";
            String dbName = "EK_ADM20_11";
            String strErrorMsg;

            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";

                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }
            return con;
        }

        public OdbcConnection ConnectRH()
        {
            String strServerName = "serv_nomv8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49160";
            String strHostName = "10.1.0.124";
            String dbName = vSesiones.sesionEmpresaActual == 1 ? "EK_NOM11_9" : vSesiones.sesionEmpresaActual == 2 ? "EK_NOM51_9" : vSesiones.sesionEmpresaActual == 5 ? "EK_NOM50_9" : vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN ? "EK_ADM05_9" : "UN TEXTO PARA QUE NO SE CONECTE";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //conString = null;
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectEKMerge(int db)
        {
            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = db == 1 ? "EK_ADM01_9" : "EK_ADM04_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + " ;port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectRHMERGE(int bd)
        {
            String strServerName = "serv_nomv8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49160";
            String strHostName = "10.1.0.124";
            String dbName = bd == 1 ? "EK_NOM11_9" : bd == 2 ? "EK_NOM51_9" : bd == 5 ? "EK_NOM50_9" : "UN TEXTO PARA QUE NO SE CONECTE";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //conString = null;
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectRHConstruplan()
        {
            String strServerName = "serv_nomv8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49160";
            String strHostName = "10.1.0.124";
            String dbName = "EK_NOM11_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //conString = null;
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectRHArrendadora()
        {
            String strServerName = "serv_nomv8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49160";
            String strHostName = "10.1.0.124";
            String dbName = "EK_NOM51_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //conString = null;
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectPrueba()
        {
            String strServerName = "serv_v8_99";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49156";
            String strHostName = "10.1.0.124";
            String dbName = vSesiones.sesionEmpresaActual == 1 ? "EK_ADM98_9" : vSesiones.sesionEmpresaActual == 2 ? "EK_ADM99_9" : vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN ? "EK_ADM05_9" : "EK_ADM98_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }
        public OdbcConnection ConnectPruebaCplanProd()
        {
            String strServerName = "serv_v8_99";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49157";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM98_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }
        public OdbcConnection ConnectPruebaEici()
        {
            String strServerName = "serv_v8_99";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49159";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM99_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ":" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }
        public OdbcConnection ConnectPruebaRH()
        {
            String strServerName = "serv_nomv8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49160";
            String strHostName = "10.1.0.124";
            //String dbName = "EK_NOM91_9";
            String dbName = "EK_NOM97_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //conString = null;
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectCPPruebaRH()
        {
            String strServerName = "serv_nomv8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49160";
            String strHostName = "10.1.0.124";
            String dbName = "EK_NOM97_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //conString = null;
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectCplanEici()
        {
            OdbcConnection con = null;

            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM06_9";
            String strErrorMsg;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }
            return con;
        }

        public OdbcConnection ConnectArrendarora()
        {
            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM04_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ":" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectArrendaroraPrueba()
        {
            String strServerName = "serv_v8_99";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "49157";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM99_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ":" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public void Close(OdbcConnection con)
        {
            con.Close();
        }

        public OdbcConnection ConnectComprasConstruplan()
        {
            OdbcConnection con = null;

            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM01_9";
            String strErrorMsg;


            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectComprasArrendadora()
        {
            OdbcConnection con = null;
            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM04_9";
            String strErrorMsg;


            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectComprasConstruplanPrueba()
        {
            String strServerName = "serv_v8_99";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._"; ;
            String strPort = "49159";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM98_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectComprasArrendadoraPrueba()
        {
            String strServerName = "serv_v8_99";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._"; ;
            String strPort = "49159";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM99_9";
            String strErrorMsg;

            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                //String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        public OdbcConnection ConnectCplanVirtual()
        {
            OdbcConnection con = null;

            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM10_9";
            String strErrorMsg;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }
            return con;
        }

        public OdbcConnection ConnectArrendadoraVirtual()
        {
            OdbcConnection con = null;

            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM12_9";
            String strErrorMsg;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }
            return con;
        }

        public OdbcConnection ConnectCplanIntegradora()
        {
            OdbcConnection con = null;
            String strServerName = "serv_v8";
            String strUserName = "dba";
            String strPassword = "_._enkontrol_._";
            String strPort = "2639";
            String strHostName = "10.1.0.124";
            String dbName = "EK_ADM03_9";
            String strErrorMsg;

            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={SQL Anywhere 11};DatabaseName=" + dbName + ";EngineName=" + strServerName + ";uid=" + strUserName + ";pwd=" + strPassword + ";LINKs=tcpip(host=" + strHostName + ";port=" + strPort + ")";

                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }
            return con;
        }

        public OdbcConnection ConexionEKAdm()
        {
            var conexion = new OdbcConnection();

            switch (vSesiones.sesionAmbienteEnkontrolAdm)
            {
                case Core.Enum.Multiempresa.EnkontrolAmbienteEnum.Prod:
                    conexion = new Conexion().Connect();
                    break;
                case Core.Enum.Multiempresa.EnkontrolAmbienteEnum.Prueba:
                    conexion = new Conexion().ConnectPrueba();
                    break;
            }

            return conexion;
        }

        public OdbcConnection ConexionEKRh()
        {
            var conexion = new OdbcConnection();

            switch (vSesiones.sesionAmbienteEnkontrolRh)
            {
                case Core.Enum.Multiempresa.EnkontrolAmbienteEnum.PruebaRh:
                    conexion = new Conexion().ConnectCPPruebaRH();
                    break;
                case Core.Enum.Multiempresa.EnkontrolAmbienteEnum.Rh:
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        conexion = new Conexion().ConnectRHConstruplan();
                    }
                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                    {
                        conexion = new Conexion().ConnectRHArrendadora();
                    }
                    break;
            }

            return conexion;
        }
    }
}
