using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Data
{
    public class DbSchemaAwareModelCacheFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            return new
            {
                Type = context.GetType(),
                Schema = context is IDbContextSchema schema ? schema.Schema : null
            };
        }
    }
}
