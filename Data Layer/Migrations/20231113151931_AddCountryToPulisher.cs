using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Layer.Migrations
{
	/// <inheritdoc />
	public partial class AddCountryToPulisher : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(table: "publisher", name: "country", type: "varchar(255)",
				nullable: false, defaultValueSql: "'Not Specified'", collation: "utf8mb4_0900_ai_ci");

		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(table: "publisher", name: "country");
		}
	}
}
