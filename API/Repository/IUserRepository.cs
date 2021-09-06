using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repository
{
    public interface IUsersRepository
    {
        GtlTitle GetTitle(string ISBN);
    }
}
