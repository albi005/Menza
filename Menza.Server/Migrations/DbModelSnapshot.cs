﻿// <auto-generated />
using System;
using Menza.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Menza.Server.Migrations
{
    [DbContext(typeof(Db))]
    partial class DbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Menza.Server.Menu", b =>
                {
                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Date");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Menza.Server.Vote", b =>
                {
                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<byte>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Date", "Email");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("Menza.Server.Vote", b =>
                {
                    b.HasOne("Menza.Server.Menu", "Menu")
                        .WithMany("Votes")
                        .HasForeignKey("Date")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("Menza.Server.Menu", b =>
                {
                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
