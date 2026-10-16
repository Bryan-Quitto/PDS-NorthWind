using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Migrations.NorthWindSales
{
    /// <inheritdoc />
    public partial class RefactorCustomerModel1FNAndSRI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Identification",
                table: "Customers",
                type: "varchar(13)",
                unicode: false,
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Customers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerType",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondLastName",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TradeName",
                table: "Customers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                computedColumnSql: "CASE WHEN [CustomerType] = 2 THEN ISNULL([BusinessName], '') ELSE CONCAT_WS(' ', [LastName], [SecondLastName], [FirstName], [SecondName]) END",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            // Eliminar clientes residuales que no tienen órdenes y no cumplen las nuevas restricciones
            migrationBuilder.Sql("DELETE FROM [Customers] WHERE [Id] NOT IN ('ALFKI', 'ANATR', 'ANTON', 'AROUT', 'BERGS');");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "ALFKI",
                columns: new[] { "BusinessName", "CustomerType", "FirstName", "Identification", "LastName", "SecondLastName", "SecondName", "TradeName" },
                values: new object[] { "ALFREDS FUTTERKISTE CIA. LTDA.", 2, null, "1790011674001", null, null, null, "Alfreds Futterkiste" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "ANATR",
                columns: new[] { "BusinessName", "CustomerType", "FirstName", "Identification", "LastName", "SecondLastName", "SecondName", "TradeName" },
                values: new object[] { null, 1, "Ana", "0102030409", "Trujillo", "Pérez", "María", "Emparedados y helados" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "ANTON",
                columns: new[] { "BusinessName", "CustomerType", "FirstName", "Identification", "LastName", "SecondLastName", "SecondName", "TradeName" },
                values: new object[] { null, 1, "Antonio", "1803456782", "Moreno", "Taquería", "José", "Taquería El Paso" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "AROUT",
                columns: new[] { "BusinessName", "CustomerType", "FirstName", "Identification", "LastName", "SecondLastName", "SecondName", "TradeName" },
                values: new object[] { null, 1, "Thomas", "1712345676001", "Hardy", null, null, "Around the Horn" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "BERGS",
                columns: new[] { "BusinessName", "CustomerType", "FirstName", "Identification", "LastName", "SecondLastName", "SecondName", "TradeName" },
                values: new object[] { "BERGLUNDS SNABBKOP S.A.", 2, null, "1790087657001", null, null, null, "Berglunds Market" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BusinessName",
                table: "Customers",
                column: "BusinessName");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FirstName",
                table: "Customers",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Identification",
                table: "Customers",
                column: "Identification",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LastName_FirstName",
                table: "Customers",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TradeName",
                table: "Customers",
                column: "TradeName");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Customers_CustomerType_Integrity",
                table: "Customers",
                sql: "([CustomerType] = 1 AND [FirstName] IS NOT NULL AND [LastName] IS NOT NULL AND [BusinessName] IS NULL) OR ([CustomerType] = 2 AND [BusinessName] IS NOT NULL AND [FirstName] IS NULL AND [LastName] IS NULL AND [SecondName] IS NULL AND [SecondLastName] IS NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Customers_Identification_Format",
                table: "Customers",
                sql: "([CustomerType] = 1 AND (LEN([Identification]) = 10 OR (LEN([Identification]) = 13 AND RIGHT([Identification], 3) <> '000'))) OR ([CustomerType] = 2 AND LEN([Identification]) = 13 AND RIGHT([Identification], 3) <> '000')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_BusinessName",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_FirstName",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Identification",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_LastName_FirstName",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TradeName",
                table: "Customers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Customers_CustomerType_Integrity",
                table: "Customers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Customers_Identification_Format",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SecondLastName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TradeName",
                table: "Customers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComputedColumnSql: "CASE WHEN [CustomerType] = 2 THEN ISNULL([BusinessName], '') ELSE CONCAT_WS(' ', [LastName], [SecondLastName], [FirstName], [SecondName]) END");

            migrationBuilder.AlterColumn<string>(
                name: "Identification",
                table: "Customers",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldUnicode: false,
                oldMaxLength: 13);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "ALFKI",
                columns: new[] { "Identification", "Name" },
                values: new object[] { "0102030405", "Alfreds Futterkiste" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "ANATR",
                columns: new[] { "Identification", "Name" },
                values: new object[] { "0203040506", "Ana Trujillo Emparedados y helados" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "ANTON",
                columns: new[] { "Identification", "Name" },
                values: new object[] { "0304050607", "Antonio Moreno Taqueria" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "AROUT",
                columns: new[] { "Identification", "Name" },
                values: new object[] { "0405060708", "Around the Horn" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "BERGS",
                columns: new[] { "Identification", "Name" },
                values: new object[] { "0506070809", "Berglunds snabbkop" });
        }
    }
}
