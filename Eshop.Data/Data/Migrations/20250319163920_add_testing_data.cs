using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Data.Migrations
{
    public partial class add_testing_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Code", "Description", "Hidden", "ImagesCount", "OldPrice", "Price", "ShortDescription", "Stock", "Title", "Url" },
                values: new object[] { 17, "something", "<p>Lorem ipsum dolor sit amet. Qui cumque harum a iure sapiente hic debitis blanditiis At vero voluptatem sit molestiae dolores et quas dolorum. Qui facilis impedit et ipsam sint sed numquam necessitatibus ex autem omnis et saepe odio? Et cumque autem aut quas fugiat et animi saepe et nobis tempore et magni facere. </p><p>Non quisquam corrupti et dolorem provident ut cumque porro et error laudantium et labore autem. A quod nobis aut labore modi ut nemo modi in fugit dolorum non dolores quia non libero laudantium? Aut nemo maxime sed suscipit aspernatur aut dolores amet in iusto nostrum. </p>", false, 2, null, 500m, "Lorem ipsum dolor sit amet. Qui cumque harum a iure sapiente hic debitis blanditiis At vero voluptatem sit molestiae dolores et quas dolorum. Qui facilis impedit et ipsam sint sed numquam necessitatibus.", 11, "Závěs Sense, zelený", "zaves-sense-zeleny" });

            migrationBuilder.InsertData(
                table: "CategoryProduct",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[] { 1, 17 });

            migrationBuilder.InsertData(
                table: "CategoryProduct",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[] { 3, 17 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryProduct",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 17 });

            migrationBuilder.DeleteData(
                table: "CategoryProduct",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 17 });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17);
        }
    }
}
