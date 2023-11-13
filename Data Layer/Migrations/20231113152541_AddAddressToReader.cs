using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Layer.Migrations
{
	/// <inheritdoc />
	public partial class AddAddressToReader : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(table: "reader", name: "address", type: "varchar(255)", nullable: false,
				defaultValueSql: "'Not Specified'", collation: "utf8mb4_0900_ai_ci");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(table: "reader", name: "address");
		}
	}
}
