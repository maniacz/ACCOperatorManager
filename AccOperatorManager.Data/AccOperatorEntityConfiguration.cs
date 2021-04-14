using AccOperatorManager.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Data
{
    public class AccOperatorEntityConfiguration : IEntityTypeConfiguration<AccOperator>
    {
        private readonly string _schema;

        public AccOperatorEntityConfiguration(string schema)
        {
            _schema = schema;
        }

        public void Configure(EntityTypeBuilder<AccOperator> builder)
        {
            if (!String.IsNullOrEmpty(_schema))
            {
                builder.ToTable(nameof(AccDbContext.AccOperators), _schema);
            }

            builder.HasKey(o => o.Operatorid);
        }
    }
}
