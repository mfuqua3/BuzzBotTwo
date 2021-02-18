using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuzzBotTwo.Migrations
{
    public partial class BuzzBotTwoInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    QualityValue = table.Column<int>(nullable: false),
                    Quality = table.Column<string>(nullable: true),
                    Class = table.Column<int>(nullable: false),
                    Subclass = table.Column<int>(nullable: true),
                    ItemLevel = table.Column<int>(nullable: false),
                    InventorySlot = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaidLockouts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidLockouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RaidChannel = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoftResEvents",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Instance = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Faction = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    RaidDate = table.Column<DateTime>(nullable: true),
                    ItemLimit = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftResEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Raids",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    RaidLockoutId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Raids_RaidLockouts_RaidLockoutId",
                        column: x => x.RaidLockoutId,
                        principalTable: "RaidLockouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServerBotRoles",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerId = table.Column<ulong>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerBotRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServerBotRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerBotRoles_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerUsers",
                columns: table => new
                {
                    ServerUserId = table.Column<Guid>(nullable: false),
                    ServerId = table.Column<ulong>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    ServerBotRoleId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerUsers", x => x.ServerUserId);
                    table.ForeignKey(
                        name: "FK_ServerUsers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerUsers_ServerBotRoles_ServerBotRoleId",
                        column: x => x.ServerBotRoleId,
                        principalTable: "ServerBotRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServerUsers_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaidParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ServerUserId = table.Column<Guid>(nullable: true),
                    RaidId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaidParticipants_Raids_RaidId",
                        column: x => x.RaidId,
                        principalTable: "Raids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RaidParticipants_ServerUsers_ServerUserId",
                        column: x => x.ServerUserId,
                        principalTable: "ServerUsers",
                        principalColumn: "ServerUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RaidItems",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateAwarded = table.Column<DateTime>(nullable: true),
                    ParticipantId = table.Column<Guid>(nullable: false),
                    RaidId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaidItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaidItems_RaidParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "RaidParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaidItems_Raids_RaidId",
                        column: x => x.RaidId,
                        principalTable: "Raids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoftResUsers",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    Spec = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    EventId = table.Column<string>(nullable: true),
                    ParticipantId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftResUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftResUsers_SoftResEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "SoftResEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftResUsers_RaidParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "RaidParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservedItems",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<ulong>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservedItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservedItems_SoftResUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SoftResUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[] { 1, "Owner" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[] { 2, "Admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[] { 5, "User" });

            migrationBuilder.CreateIndex(
                name: "IX_RaidItems_ItemId",
                table: "RaidItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidItems_ParticipantId",
                table: "RaidItems",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidItems_RaidId",
                table: "RaidItems",
                column: "RaidId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidParticipants_RaidId",
                table: "RaidParticipants",
                column: "RaidId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidParticipants_ServerUserId",
                table: "RaidParticipants",
                column: "ServerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Raids_RaidLockoutId",
                table: "Raids",
                column: "RaidLockoutId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedItems_ItemId",
                table: "ReservedItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedItems_UserId",
                table: "ReservedItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerBotRoles_RoleId",
                table: "ServerBotRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerBotRoles_ServerId",
                table: "ServerBotRoles",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUsers_RoleId",
                table: "ServerUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUsers_ServerBotRoleId",
                table: "ServerUsers",
                column: "ServerBotRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUsers_ServerId",
                table: "ServerUsers",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUsers_UserId",
                table: "ServerUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftResUsers_EventId",
                table: "SoftResUsers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftResUsers_ParticipantId",
                table: "SoftResUsers",
                column: "ParticipantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaidItems");

            migrationBuilder.DropTable(
                name: "ReservedItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "SoftResUsers");

            migrationBuilder.DropTable(
                name: "SoftResEvents");

            migrationBuilder.DropTable(
                name: "RaidParticipants");

            migrationBuilder.DropTable(
                name: "Raids");

            migrationBuilder.DropTable(
                name: "ServerUsers");

            migrationBuilder.DropTable(
                name: "RaidLockouts");

            migrationBuilder.DropTable(
                name: "ServerBotRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
