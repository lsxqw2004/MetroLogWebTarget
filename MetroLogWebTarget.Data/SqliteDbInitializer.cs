using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.CodeFirst;

namespace MetroLogWebTarget.Data
{
    public class SqliteDbInitializer : SqliteDropCreateDatabaseAlways<MetroLogDbContext>
    {
        public SqliteDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        { }

        protected override void Seed(MetroLogDbContext context)
        {

            base.Seed(context);
        }
    }
}
