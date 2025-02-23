using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeOrLoans.Migrations
{
    /// <inheritdoc />
    public partial class Trigger_MediaReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //create trigger function
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_user_average_score()
                RETURNS TRIGGER AS $$
                BEGIN
                    UPDATE ""User""
                    SET ""Score"" = (
                        SELECT AVG(""EvaluatedScore"")
                        FROM ""UserEvaluation""
                        WHERE ""IdUserRated"" = NEW.""IdUserRated""
                    )
                    WHERE ""Id"" = NEW.""IdUserRated"";
                    
                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                CREATE TRIGGER trigger_update_user_average_score
                AFTER INSERT OR UPDATE ON ""UserEvaluation""
                FOR EACH ROW EXECUTE FUNCTION update_user_average_score();
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trigger_update_user_average_score ON UserEvaluation;");
            
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_user_average_score;");
        }
    }
}
