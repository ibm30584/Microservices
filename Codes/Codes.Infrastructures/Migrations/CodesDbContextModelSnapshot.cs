﻿// <auto-generated />
using System;
using Codes.Infrastructures.Services.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Codes.Infrastructures.Migrations
{
    [DbContext(typeof(CodesDbContext))]
    partial class CodesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Codes.Domain.Entities.Code", b =>
                {
                    b.Property<int>("CodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeId"));

                    b.Property<int>("CodeTypeId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Text2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CodeId");

                    b.HasIndex("CodeTypeId");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("Codes");
                });

            modelBuilder.Entity("Codes.Domain.Entities.CodeType", b =>
                {
                    b.Property<int>("CodeTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeTypeId"));

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Text2")
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CodeTypeId");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("CodeTypes");
                });

            modelBuilder.Entity("Codes.Domain.Entities.Code", b =>
                {
                    b.HasOne("Codes.Domain.Entities.CodeType", "CodeType")
                        .WithMany("Codes")
                        .HasForeignKey("CodeTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsMany("Codes.Domain.Entities.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("CodeId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<string>("Key")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CodeId", "Id");

                            b1.ToTable("Codes");

                            b1.ToJson("Metadata");

                            b1.WithOwner()
                                .HasForeignKey("CodeId");
                        });

                    b.Navigation("CodeType");

                    b.Navigation("Metadata");
                });

            modelBuilder.Entity("Codes.Domain.Entities.CodeType", b =>
                {
                    b.Navigation("Codes");
                });
#pragma warning restore 612, 618
        }
    }
}
