using AccOperatorManager.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Data
{
    public class AccOperatorManagerDbContext : DbContext
    {
        //todo: Tu może trzeba stworzyć pusty konstruktor przyjmujący parametr DbContextOptions<OperatorDbContext>
        public AccOperatorManagerDbContext(DbContextOptions<AccOperatorManagerDbContext> options)
            : base(options)
        {
        }

        public DbSet<AccOperator> Operators { get; set; }
    }
}
