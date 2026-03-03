using Microsoft.EntityFrameworkCore;
using CapStone.Domain.Entities;

namespace CapStone.Infrastructure.Data
{
    public class AccidentDbContext : DbContext
    {
        public AccidentDbContext(DbContextOptions<AccidentDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<PolicyType> PolicyTypes => Set<PolicyType>();
        public DbSet<PolicyCoverage> PolicyCoverages => Set<PolicyCoverage>();
        public DbSet<PolicyRequest> PolicyRequests => Set<PolicyRequest>();
        public DbSet<Policy> Policies => Set<Policy>();
        public DbSet<InsuranceClaim> Claims => Set<InsuranceClaim>();
        public DbSet<ClaimReview> ClaimReviews => Set<ClaimReview>();
        public DbSet<Settlement> Settlements => Set<Settlement>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Nominee> Nominees => Set<Nominee>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
             
            modelBuilder.Entity<PolicyType>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);


 
            modelBuilder.Entity<PolicyCoverage>()
                .HasOne<PolicyType>()
                .WithMany()
                .HasForeignKey(pc => pc.PolicyTypeId)
                .OnDelete(DeleteBehavior.Cascade);


    
            modelBuilder.Entity<PolicyRequest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(pr => pr.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
             
            modelBuilder.Entity<PolicyRequest>()
                .HasOne(pr => pr.AssignedAgent)
                .WithMany()
                .HasForeignKey(pr => pr.AssignedAgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PolicyRequest>()
                .HasOne(pr => pr.PolicyType)
                .WithMany()
                .HasForeignKey(pr => pr.PolicyTypeId)
                .OnDelete(DeleteBehavior.Restrict);
 
            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.AssignedAgent)
                .WithMany()
                .HasForeignKey(p => p.AssignedAgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.PolicyType)
                .WithMany()
                .HasForeignKey(p => p.PolicyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.PolicyRequest)
                .WithMany()
                .HasForeignKey(p => p.RequestId)
                .OnDelete(DeleteBehavior.Restrict);
 
            modelBuilder.Entity<Nominee>()
                .HasOne<Policy>()
                .WithMany()
                .HasForeignKey(n => n.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);

 
            modelBuilder.Entity<Payment>()
                .HasOne<Policy>()
                .WithMany()
                .HasForeignKey(p => p.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);

 
            modelBuilder.Entity<InsuranceClaim>()
                .HasOne(c => c.Policy)
                .WithMany()
                .HasForeignKey(c => c.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsuranceClaim>()
                .HasOne(c => c.Customer)
                .WithMany()
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsuranceClaim>()
                .HasOne(c => c.Officer)
                .WithMany()
                .HasForeignKey(c => c.OfficerId)
                .OnDelete(DeleteBehavior.Restrict);

 
            modelBuilder.Entity<ClaimReview>()
                .HasOne<InsuranceClaim>()
                .WithMany()
                .HasForeignKey(cr => cr.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClaimReview>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(cr => cr.OfficerId)
                .OnDelete(DeleteBehavior.Restrict);

 
            modelBuilder.Entity<Settlement>()
                .HasOne<InsuranceClaim>()
                .WithMany()
                .HasForeignKey(s => s.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            //AccidentDbSeedData.Apply(modelBuilder);
        }
    }
}