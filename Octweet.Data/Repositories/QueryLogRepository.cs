using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Octweet.Data.Abstractions;
using Octweet.Data.Abstractions.Repositories;

namespace Octweet.Data.Repositories
{
    public class QueryLogRepository : IQueryLogRepository
    {
        private readonly OctweetDbContext _context;
        public QueryLogRepository(OctweetDbContext context)
        {
            _context = context;
        }

        public async Task<QueryLog> GetLatestExecution(string query)
        {
            return await _context.QueryLog.FirstOrDefaultAsync(x => x.Query == query);
        }

        public async Task InsertOrUpdateQueryLog(QueryLog queryLog)
        {
            if (queryLog == null)
            {
                throw new ArgumentNullException(nameof(queryLog));
            }

            _context.Update(queryLog);
            await _context.SaveChangesAsync();
        }
    }
}
