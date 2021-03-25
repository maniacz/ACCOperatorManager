using System;
using AccOperatorManager.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AccOperatorManager.Data
{
    public partial class AccDbContext : DbContext
    {
        public AccDbContext()
        {
        }

        public AccDbContext(DbContextOptions<AccDbContext> options)
            : base(options)
        {
        }

        public DbSet<AccOperator> AccOperators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ACC");

            modelBuilder.Entity<AccOperator>(entity =>
            {
                entity.HasKey(e => new { e.Operatorid, e.Line });

                entity.ToTable("ACC_OPERATOR");

                entity.Property(e => e.Operatorid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OPERATORID");

                entity.Property(e => e.Line)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LINE");

                entity.Property(e => e.GroupList)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("GROUP_LIST");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Op)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("OP");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");
            });

            modelBuilder.HasSequence("ACCDATADETAILSEQID").IsCyclic();

            modelBuilder.HasSequence("ACCDATAHEADERSEQID").IsCyclic();

            modelBuilder.HasSequence("ACCDATASPECSEQID");

            modelBuilder.HasSequence("ACCSPECDATADETAILSEQID").IsCyclic();

            modelBuilder.HasSequence("ACCSPECDATAHEADERSEQID").IsCyclic();

            modelBuilder.HasSequence("AUDITRESULTS_SEQUENCE");

            modelBuilder.HasSequence("PLCFAULTRECID");

            modelBuilder.HasSequence("PROCDATA_IGNORED_SEQID");

            modelBuilder.HasSequence("PVOUTPUTSEQID");

            modelBuilder.HasSequence("SEQ_ACC_RPT1").IsCyclic();

            modelBuilder.HasSequence("TRG_RECID");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
