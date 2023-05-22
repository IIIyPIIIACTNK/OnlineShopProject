using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopProject.Models;

public partial class OsClientsContext : DbContext
{
    public OsClientsContext()
    {
        Clients.Load();
        Items.Load();
        SoldItems.Load();
    }

    public OsClientsContext(DbContextOptions<OsClientsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }
    public ObservableCollection<Client> ClientsToView { get { return Clients.Local.ToObservableCollection(); } }
    public virtual DbSet<SoldItem> SoldItems { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public ObservableCollection<Item> ItemsToView { get { return Items.Local.ToObservableCollection(); } }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress; Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OSClients;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {

            entity.Property(e => e.EMail)
                .HasMaxLength(50)
                .HasColumnName("EMail");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("LastName");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("MiddleName");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("PhoneNumber");
        });

        modelBuilder.Entity<SoldItem>(entity =>
        {
            //entity.HasNoKey();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(50)
                .HasColumnName("CustomerEmail");
            entity.Property(e => e.ItemId)
                .HasMaxLength(4)
                .HasColumnName("ItemId");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(4)
                .HasColumnName("ItemCode");
        });
        modelBuilder.Entity<Item>(entity =>
        {
            //entity.HasNoKey();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            entity.Property(e => e.Name)
                 .HasMaxLength(50)
                 .HasColumnName("Name");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
