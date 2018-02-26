﻿// <auto-generated />
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Demo.Migrations.UserProfile
{
    [DbContext(typeof(UserProfileContext))]
    [Migration("20180225194127_UserFriendListv2")]
    partial class UserFriendListv2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Demo.Models.UserProfile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("UserModelID");

                    b.Property<string>("UserProfileStatusUpdate");

                    b.Property<string>("UserProfileSummary");

                    b.HasKey("ID");

                    b.ToTable("UserProfile");
                });
#pragma warning restore 612, 618
        }
    }
}
