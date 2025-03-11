using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeOrLoans.Migrations
{
    /// <inheritdoc />
    public partial class newColumnsToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name:"Description",
                table:"Product",
                type: "text",
                nullable: false
            );
            migrationBuilder.AddColumn<string>(
                name:"Name",
                table:"Product",
                type: "varchar(50)",
                nullable: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
