using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Data
{
    public class DbContextSchema : IDbContextSchema
    {
        public string Schema { get; }

        public DbContextSchema(string schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }
    }
}
