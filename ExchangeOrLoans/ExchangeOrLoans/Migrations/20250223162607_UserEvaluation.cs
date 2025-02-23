using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExchangeOrLoans.Migrations
{
    /// <inheritdoc />
    public partial class UserEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEvaluation",
                columns: table => new
                {
                    IdEvaluation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUserReviewer = table.Column<int>(type: "integer", nullable: false),
                    IdUserRated = table.Column<int>(type: "integer", nullable: false),
                    EvaluatedScore = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvaluation", x => x.IdEvaluation);
                    
                    table.ForeignKey(
                        name: "FK_UserEvaluation_User_Reviewer",
                        column: x => x.IdUserReviewer,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    
                    
                    table.ForeignKey(
                        name: "FK_UserEvaluation_User_Rated",
                        column: x => x.IdUserRated,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    
                    
                });
            
            migrationBuilder.CreateIndex(
                name: "IX_UserEvaluation_IdUserReviewer",
                table: "UserEvaluation",
                column: "IdUserReviewer");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvaluation_IdUserRated",
                table: "UserEvaluation",
                column: "IdUserRated");
        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEvaluation");
        }
    }
}
