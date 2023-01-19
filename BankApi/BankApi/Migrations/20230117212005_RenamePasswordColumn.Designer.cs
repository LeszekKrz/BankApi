﻿// <auto-generated />
using System;
using BankAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankApi.Migrations
{
    [DbContext(typeof(OffersDbContext))]
    [Migration("20230117212005_RenamePasswordColumn")]
    partial class RenamePasswordColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("BankAPI.Database.ApplicationEntity", b =>
                {
                    b.Property<Guid>("OfferId")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreationTimestamp")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("DecisionTimestamp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("OfferId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("BankAPI.Database.OfferEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreationTimestamp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GovernmentIdName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GovernmentIdTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GovernmentIdValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Income")
                        .HasColumnType("INTEGER");

                    b.Property<int>("JobTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfInstallments")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OwnerUsername")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("BankAPI.Database.UserEntity", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankAPI.Database.ApplicationEntity", b =>
                {
                    b.HasOne("BankAPI.Database.OfferEntity", "Offer")
                        .WithOne("Application")
                        .HasForeignKey("BankAPI.Database.ApplicationEntity", "OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("BankAPI.Database.OfferEntity", b =>
                {
                    b.Navigation("Application");
                });
#pragma warning restore 612, 618
        }
    }
}
