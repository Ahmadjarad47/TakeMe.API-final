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
    public class PostesRepositries : GenericRepositrie<Postes>, IPostes
    {
        public PostesRepositries(ApplicationDbContext context) : base(context)
        {
        }
    }
}
