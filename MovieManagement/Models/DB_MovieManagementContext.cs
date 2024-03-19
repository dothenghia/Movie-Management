using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MovieManagement.Models
{
    public partial class DB_MovieManagementContext : DbContext
    {
        public DB_MovieManagementContext()
        {
        }

        public DB_MovieManagementContext(DbContextOptions<DB_MovieManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AgeCertificate> AgeCertificates { get; set; } = null!;
        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<BillVoucher> BillVouchers { get; set; } = null!;
        public virtual DbSet<Contributor> Contributors { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ShowTime> ShowTimes { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=DB_MovieManagement; Trusted_Connection=True;");
                // optionsBuilder.UseSqlServer("Server=BIXCUIT;Database=DB_MovieManagement; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("DOB");

                entity.Property(e => e.Fullname).HasMaxLength(30);

                entity.Property(e => e.Gender).HasMaxLength(8);

                entity.Property(e => e.Password).HasMaxLength(30);

                entity.Property(e => e.Username).HasMaxLength(30);
            });

            modelBuilder.Entity<AgeCertificate>(entity =>
            {
                entity.ToTable("AgeCertificate");

                entity.Property(e => e.BackgroundColor).HasMaxLength(30);

                entity.Property(e => e.DisplayContent).HasMaxLength(30);

                entity.Property(e => e.ForegroundColor).HasMaxLength(30);
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.BookingTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Bill_Account");
            });

            modelBuilder.Entity<BillVoucher>(entity =>
            {
                entity.HasKey(e => new { e.BillId, e.VoucherId })
                    .HasName("PK__BillVouc__125C1BF80A4CAD13");

                entity.ToTable("BillVoucher");

                entity.Property(e => e.AppliedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillVouchers)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillVoucher_Bill");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.BillVouchers)
                    .HasForeignKey(d => d.VoucherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillVoucher_Voucher");
            });

            modelBuilder.Entity<Contributor>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.PersonId, e.RoleId })
                    .HasName("PK__Contribu__1AFA916A6ACE0519");

                entity.ToTable("Contributor");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Contributors)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contributor_Movie");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Contributors)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contributor_Person");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Contributors)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contributor_Role");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.GenreName).HasMaxLength(30);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movie");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.ImdbScore).HasColumnName("IMDbScore");

                entity.Property(e => e.PosterUrl).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(30);

                entity.Property(e => e.TrailerUrl).HasMaxLength(100);

                entity.HasOne(d => d.AgeCertificate)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.AgeCertificateId)
                    .HasConstraintName("FK_AgeCertificate");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_Movie_Genre");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.AvatarUrl).HasMaxLength(100);

                entity.Property(e => e.Biography).HasMaxLength(1000);

                entity.Property(e => e.Fullname).HasMaxLength(30);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleName).HasMaxLength(30);
            });

            modelBuilder.Entity<ShowTime>(entity =>
            {
                entity.ToTable("ShowTime");

                entity.Property(e => e.ShowDate).HasColumnType("datetime");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.ShowTimes)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK_ShowTime_Movie");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(e => e.Row).HasMaxLength(5);

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK_Ticket_Bill");

                entity.HasOne(d => d.ShowTime)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ShowTimeId)
                    .HasConstraintName("FK_Ticket_ShowTime");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");

                entity.Property(e => e.VoucherCode).HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
