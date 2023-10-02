using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.Entities;

namespace TakeMe.Core.Interfaces
{
    public interface ICompanies:IGenericRepositrie<Company>
    {
        Task<bool> CheckIshere(string name);
        Task<List<Postes>> getId(int name);
        Task<Company> getName(string name);
    }
}
