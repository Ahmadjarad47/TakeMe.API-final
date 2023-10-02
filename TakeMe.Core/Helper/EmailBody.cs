using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.Helper
{
    public class EmailBody
    {
        public static string EmailStringBody(string code)
        {
            return $@"<html>
<head>

</head>
<body style=""margin:0;padding: 0;font-family: Arial, Helvetica, sans-serif;"">
  <div>
    <div>
      <h1>Reset your password</h1>
      <hr>
      <p>You are receiving this e-mail because you requested a password reset for your TakeMe App.</p>
          <p style="" box-shadow: 0px 0px 10px #888888; padding: 10px;background-color: #f2f2f2;font-size: 20px;  color: blue;   text-align: center;  border-radius: 10px;  letter-spacing: 8px;"">{code} </p>  
  </div>
  </div>
</body>
</html>
";
        }
    }
}
