using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.DTOs;

namespace TakeMe.Core.Interfaces
{
    public interface IEmailService
    {
        void sendEmail(EmailModelDTO email);
    }
}
