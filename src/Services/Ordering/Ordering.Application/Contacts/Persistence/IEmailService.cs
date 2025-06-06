using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Application.Models;

namespace Ordering.Application.Contacts.Persistence
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(Email email);
    }
}

