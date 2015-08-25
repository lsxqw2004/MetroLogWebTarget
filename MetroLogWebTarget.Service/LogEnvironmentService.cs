using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLogWebTarget.Data;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Service
{
    public class LogEnvironmentService:ILogEnvironmentService
    {
        private readonly IRepository<LogEnvironment> _logEnvironmentRepository;

        public LogEnvironmentService(IRepository<LogEnvironment> logEnvironmentRepository)
        {
            _logEnvironmentRepository = logEnvironmentRepository;
        }

        public IEnumerable<LogEnvironment> GetTopNByPubId(string pubId, int num)
        {
            var query = _logEnvironmentRepository.Table.Where(r => r.PackagePublisherId == pubId).Take(num);
            return query.ToList();
        }

        public void Create(LogEnvironment logEnv)
        {
            _logEnvironmentRepository.Insert(logEnv);
        }

        public LogEnvironment GetById(int id)
        {
            return _logEnvironmentRepository.GetById(id);
        }
    }
}
