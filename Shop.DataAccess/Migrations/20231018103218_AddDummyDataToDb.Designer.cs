﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shop.DataAccess.Data;


#nullable disable

namespace Shop.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231018103218_AddDummyDataToDb")]
    partial class AddDummyDataToDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebShop.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Computers"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Laptops"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "Smartphones"
                        },
                        new
                        {
                            Id = 4,
                            DisplayOrder = 4,
                            Name = "TVs"
                        },
                        new
                        {
                            Id = 5,
                            DisplayOrder = 5,
                            Name = "Monitors"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
