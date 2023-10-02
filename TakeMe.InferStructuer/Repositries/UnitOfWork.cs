using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.Entities;
using TakeMe.Core.Interfaces;
using TakeMe.InferStructuer.Data;

namespace TakeMe.InferStructuer.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public ICompanies Companies { get; }

        public IPostes Postes { get; }

        public IRegisterDaily RegisterDaily { get; }

        public IQrCodeReader IQrCodeReader { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            Postes = new PostesRepositries(context);
            Companies = new CompaniesRepositories(context);
            RegisterDaily=new RegisterDailyRepositry(context);
            IQrCodeReader = new QrCodeReaderRepositry(context);
        }
    }
}
