using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebHelper.ClientIp
{
    public class ClientIP
    {
        public static string GetIp(HttpRequest Request)
        {
            try
            {
                string result = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (null == result || result == String.Empty)
                {
                    result = Request.ServerVariables["REMOTE_ADDR"];
                }

                if (null == result || result == String.Empty)
                {
                    result = Request.UserHostAddress;
                }
                return result;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

    }
}
