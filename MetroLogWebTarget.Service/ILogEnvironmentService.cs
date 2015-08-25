using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Service
{
    public interface ILogEnvironmentService
    {
        IEnumerable<LogEnvironment> GetTopNByPubId(string pubId, int num);

        void Create(LogEnvironment logEnv);

        LogEnvironment GetById(int id);
    }
}
