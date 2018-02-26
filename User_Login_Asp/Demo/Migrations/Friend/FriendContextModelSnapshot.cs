﻿// <auto-generated />
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Demo.Migrations.Friend
{
    [DbContext(typeof(FriendContext))]
    partial class FriendContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Demo.Models.FriendModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("UserAcceptA");

                    b.Property<bool>("UserAcceptB");

                    b.Property<int>("UserProfileIDA");

                    b.Property<int>("UserProfileIDB");

                    b.Property<string>("User_NameA");

                    b.Property<string>("User_NameB");

                    b.HasKey("ID");

                    b.ToTable("FriendModel");
                });
#pragma warning restore 612, 618
        }
    }
}
