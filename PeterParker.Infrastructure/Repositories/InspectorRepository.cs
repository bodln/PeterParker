using PeterParker.Data;
using PeterParker.Data.Models;
using PeterParker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Repositories
{
    public class InspectorRepository : Repository<Inspector>, IInspectorRepository
    {
        private readonly DataContext context;

        public InspectorRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
