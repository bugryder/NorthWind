using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Infrastructure
{
    public static class Log
    {
        private static string logWritePath = ConfigurationManager.AppSettings["LogWritePath"];
        static string _fileName = "NorWind.API_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

        public static void SqlError(Exception ex, string sql, string remark)
        {
            try
            {
                string dateNowStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string text = "";
                text += "[SQL執行錯誤] " + dateNowStr + "\r\n";
                text += "[SQL語法] \r\n";
                text += sql + "\r\n";
                text += "[Exception] \r\n";
                text += ex.ToString() + "\r\n";
                if (!string.IsNullOrEmpty(remark))
                {
                    text += "[備註] ";
                    text += remark;
                    text += "\r\n";

                }
                WriteLog(_fileName, text);
            }
            catch
            { }

        }



        public static void WriteLog(string fileName, string text)
        {
            try
            {
                StreamWriter sw = new StreamWriter(logWritePath + _fileName, true);
                sw.WriteLine(DateTime.Now.ToString("yyyyMMdd HH:mm") + ":" + text);
                sw.Close();
            }
            catch
            { }
        }


    }
}
