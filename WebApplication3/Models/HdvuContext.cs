using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models;

public partial class HdvuContext : DbContext
{
    public HdvuContext()
    {
    }

    public HdvuContext(DbContextOptions<HdvuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgentCommission> AgentCommissions { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommissionRate> CommissionRates { get; set; }

    public virtual DbSet<ConsultationRequest> ConsultationRequests { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Listing> Listings { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectArea> ProjectAreas { get; set; }

    public virtual DbSet<ProjectAreaPropertyType> ProjectAreaPropertyTypes { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyType> PropertyTypes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-T2MKG56O\\SQLEXPRESS;Database=hdvu;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgentCommission>(entity =>
        {
            entity.HasKey(e => e.CommissionId).HasName("PK__AgentCom__6C2C8F0C230CE521");

            entity.HasIndex(e => e.AgentId, "IX_AgentCommissions_AgentId");

            entity.HasIndex(e => e.ContractId, "UQ_AgentCommission_Contract").IsUnique();

            entity.Property(e => e.CommissionId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.BaseAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CommissionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CommissionPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);

            entity.HasOne(d => d.Agent).WithMany(p => p.AgentCommissions)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AgentComm__Agent__0B91BA14");

            entity.HasOne(d => d.Contract).WithOne(p => p.AgentCommission)
                .HasForeignKey<AgentCommission>(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AgentComm__Contr__0C85DE4D");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFCAEED6D580");

            entity.Property(e => e.CommentId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Content).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.LikesCount).HasDefaultValue(0);

            entity.HasOne(d => d.Listing).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ListingId)
                .HasConstraintName("FK__Comments__Listin__0E6E26BF");

            entity.HasOne(d => d.News).WithMany(p => p.Comments)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Comments__NewsId__0D7A0286");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Comments__Parent__10566F31");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__UserId__0F624AF8");
        });

        modelBuilder.Entity<CommissionRate>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__Commissi__58A7CF5CBF7ABB46");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ForType).HasMaxLength(20);
            entity.Property(e => e.MaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.RatePercentage).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<ConsultationRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Consulta__33A8517AD5D6CAB4");

            entity.HasIndex(e => new { e.AssignedAgentId, e.Status }, "IX_ConsultationRequests_AssignedAgentId_Status");

            entity.Property(e => e.RequestId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Inquiry).HasMaxLength(2000);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.ServiceType).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.AssignedAgent).WithMany(p => p.ConsultationRequestAssignedAgents)
                .HasForeignKey(d => d.AssignedAgentId)
                .HasConstraintName("FK__Consultat__Assig__123EB7A3");

            entity.HasOne(d => d.User).WithMany(p => p.ConsultationRequestUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Consultat__UserI__114A936A");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D346995361F05");

            entity.HasIndex(e => new { e.ListingId, e.Status }, "IX_Contracts_ListingId_Status");

            entity.Property(e => e.ContractId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.ContractType).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DepositAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DownPayment).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PdfDocumentUrl).HasMaxLength(1000);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceUnit).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Buyer).WithMany(p => p.ContractBuyers)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contracts__Buyer__151B244E");

            entity.HasOne(d => d.Listing).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ListingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contracts__Listi__1332DBDC");

            entity.HasOne(d => d.Seller).WithMany(p => p.ContractSellers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contracts__Selle__14270015");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PK__Conversa__C050D8772F528C9C");

            entity.HasIndex(e => new { e.Participant1Id, e.Participant2Id }, "UQ_Conversation_Participants").IsUnique();

            entity.Property(e => e.ConversationId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.LastMessageAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Participant1).WithMany(p => p.ConversationParticipant1s)
                .HasForeignKey(d => d.Participant1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversat__Parti__160F4887");

            entity.HasOne(d => d.Participant2).WithMany(p => p.ConversationParticipant2s)
                .HasForeignKey(d => d.Participant2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversat__Parti__17036CC0");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB56B8B5BF7");

            entity.Property(e => e.InvoiceId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("VND");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.InvoiceDate).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.InvoicePdfUrl).HasMaxLength(1000);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Contract).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK__Invoices__Contra__17F790F9");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__UserId__18EBB532");
        });

        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(e => e.ListingId).HasName("PK__Listings__BF3EBED07FC69A6B");

            entity.HasIndex(e => new { e.AgentId, e.Status }, "IX_Listings_AgentId_Status");

            entity.HasIndex(e => new { e.Price, e.IsForRent, e.Status }, "IX_Listings_Price_IsForRent_Status");

            entity.HasIndex(e => new { e.ProjectId, e.AreaId, e.ProjectAreaPropertyTypeId }, "IX_Listings_Project_Area_Type");

            entity.HasIndex(e => e.ListingId, "UQ_FTS_Listings_Key").IsUnique();

            entity.Property(e => e.ListingId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.AreaSqM).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ComputedPrice)
                .HasComputedColumnSql("([Price]-isnull([DiscountAmount],(0)))", true)
                .HasColumnType("decimal(19, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.IsFeatured).HasDefaultValue(false);
            entity.Property(e => e.IsForRent).HasDefaultValue(false);
            entity.Property(e => e.IsSoldOrRented).HasComputedColumnSql("(case when [Status]='Rented' OR [Status]='Sold' then (1) else (0) end)", true);
            entity.Property(e => e.PostedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceUnit).HasMaxLength(50);
            entity.Property(e => e.RentDurationUnit).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Views).HasDefaultValue(0);

            entity.HasOne(d => d.Agent).WithMany(p => p.Listings)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK__Listings__AgentI__1AD3FDA4");

            entity.HasOne(d => d.Area).WithMany(p => p.Listings)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__Listings__AreaId__1CBC4616");

            entity.HasOne(d => d.ProjectAreaPropertyType).WithMany(p => p.Listings)
                .HasForeignKey(d => d.ProjectAreaPropertyTypeId)
                .HasConstraintName("FK__Listings__Projec__1DB06A4F");

            entity.HasOne(d => d.Project).WithMany(p => p.Listings)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Listings__Projec__1BC821DD");

            entity.HasOne(d => d.Property).WithMany(p => p.Listings)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK__Listings__Proper__19DFD96B");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9CD7624F83");

            entity.HasIndex(e => new { e.ConversationId, e.Timestamp }, "IX_Messages_ConversationId_Timestamp").IsDescending(false, true);

            entity.Property(e => e.ContentType)
                .HasMaxLength(50)
                .HasDefaultValue("text");
            entity.Property(e => e.ConversationId).HasMaxLength(100);
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("FK__Messages__Conver__1EA48E88");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Receiv__208CD6FA");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Sender__1F98B2C1");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF32076E12E");

            entity.HasIndex(e => e.PostedAt, "IX_News_PostedAt").IsDescending();

            entity.HasIndex(e => e.NewsId, "UQ_FTS_News_Key").IsUnique();

            entity.HasIndex(e => e.Slug, "UQ__News__BC7B5FB6FCE77B5B").IsUnique();

            entity.Property(e => e.NewsId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.ContentType)
                .HasMaxLength(50)
                .HasDefaultValue("Article");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsExternal).HasDefaultValue(false);
            entity.Property(e => e.IsHighlight).HasDefaultValue(false);
            entity.Property(e => e.PostedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ShortDescription).HasMaxLength(1000);
            entity.Property(e => e.Slug).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Published");
            entity.Property(e => e.Tags).HasMaxLength(500);
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Views).HasDefaultValue(0);

            entity.HasOne(d => d.Listing).WithMany(p => p.News)
                .HasForeignKey(d => d.ListingId)
                .HasConstraintName("FK__News__ListingId__236943A5");

            entity.HasOne(d => d.Poster).WithMany(p => p.News)
                .HasForeignKey(d => d.PosterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__News__PosterId__2180FB33");

            entity.HasOne(d => d.Project).WithMany(p => p.News)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__News__ProjectId__22751F6C");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABEF0A1A37D0F");

            entity.HasIndex(e => e.ProjectName, "IX_Projects_ProjectName");

            entity.Property(e => e.ProjectId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Developer).HasMaxLength(200);
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.ProjectName).HasMaxLength(300);
        });

        modelBuilder.Entity<ProjectArea>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__ProjectA__70B82048D792F3EA");

            entity.HasIndex(e => e.ProjectId, "IX_ProjectAreas_ProjectId");

            entity.Property(e => e.AreaId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AreaName).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectAreas)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__ProjectAr__Proje__2645B050");
        });

        modelBuilder.Entity<ProjectAreaPropertyType>(entity =>
        {
            entity.HasKey(e => e.ProjectAreaPropertyTypeId).HasName("PK__ProjectA__62CF019E3603EA86");

            entity.HasIndex(e => new { e.AreaId, e.PropertyTypeId }, "UQ_AreaPropertyType").IsUnique();

            entity.Property(e => e.ProjectAreaPropertyTypeId).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.Area).WithMany(p => p.ProjectAreaPropertyTypes)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__ProjectAr__AreaI__245D67DE");

            entity.HasOne(d => d.PropertyType).WithMany(p => p.ProjectAreaPropertyTypes)
                .HasForeignKey(d => d.PropertyTypeId)
                .HasConstraintName("FK__ProjectAr__Prope__25518C17");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PK__Properti__70C9A7351BDD6601");

            entity.HasIndex(e => e.OwnerId, "IX_Properties_OwnerId");

            entity.Property(e => e.PropertyId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.BlockOrTower).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.OriginalAreaSqM).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Available");
            entity.Property(e => e.UnitNumber).HasMaxLength(100);

            entity.HasOne(d => d.Area).WithMany(p => p.Properties)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__Propertie__AreaI__282DF8C2");

            entity.HasOne(d => d.Owner).WithMany(p => p.Properties)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Propertie__Owner__29221CFB");

            entity.HasOne(d => d.Project).WithMany(p => p.Properties)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Propertie__Proje__2739D489");
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.HasKey(e => e.PropertyTypeId).HasName("PK__Property__BDE14DB40B2CE104");

            entity.HasIndex(e => e.TypeName, "UQ__Property__D4E7DFA88B99661C").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6BB687A1E4");

            entity.Property(e => e.TransactionId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("VND");
            entity.Property(e => e.IsPartial).HasDefaultValue(false);
            entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            entity.Property(e => e.Purpose).HasMaxLength(500);
            entity.Property(e => e.Reference).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Contract).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK__Transacti__Contr__2A164134");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__Transacti__Invoi__2B0A656D");

            entity.HasOne(d => d.Payer).WithMany(p => p.TransactionPayers)
                .HasForeignKey(d => d.PayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Payer__2BFE89A6");

            entity.HasOne(d => d.Receiver).WithMany(p => p.TransactionReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Recei__2CF2ADDF");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C225FAEE5");

            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.HasIndex(e => e.Role, "IX_Users_Role");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053479D43C31").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456770BF5DA").IsUnique();

            entity.Property(e => e.UserId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.AvatarUrl).HasMaxLength(1000);
            entity.Property(e => e.Bio).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IdentityDocumentUrl).HasMaxLength(1000);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);
            entity.Property(e => e.KycStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
