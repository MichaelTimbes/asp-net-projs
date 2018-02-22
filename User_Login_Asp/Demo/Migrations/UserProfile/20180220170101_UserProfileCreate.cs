using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Demo.Migrations.UserProfile
{
    public partial class UserProfileCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserProfileStatusUpdate = table.Column<string>(nullable: true),
                    UserProfileSummary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserProfileID = table.Column<int>(nullable: false),
                    User_Name = table.Column<string>(nullable: true),
                    User_Password = table.Column<string>(nullable: true),
                    User_SignIn_Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserModel_UserProfile_UserProfileID",
                        column: x => x.UserProfileID,
                        principalTable: "UserProfile",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserModel_UserProfileID",
                table: "UserModel",
                column: "UserProfileID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserModel");

            migrationBuilder.DropTable(
                name: "UserProfile");
        }
    }
}
