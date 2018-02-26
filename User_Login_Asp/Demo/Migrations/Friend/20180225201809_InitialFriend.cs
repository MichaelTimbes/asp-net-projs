using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Demo.Migrations.Friend
{
    public partial class InitialFriend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserAcceptA = table.Column<bool>(nullable: false),
                    UserAcceptB = table.Column<bool>(nullable: false),
                    UserProfileIDA = table.Column<int>(nullable: false),
                    UserProfileIDB = table.Column<int>(nullable: false),
                    User_NameA = table.Column<string>(nullable: true),
                    User_NameB = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendModel", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendModel");
        }
    }
}
