using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProtfolio.API.Migrations
{
    /// <inheritdoc />
    public partial class ProjectSkillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSkill_Projects_ProjectId",
                table: "ProjectSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSkill",
                table: "ProjectSkill");

            migrationBuilder.RenameTable(
                name: "ProjectSkill",
                newName: "ProjectSkills");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectSkill_ProjectId",
                table: "ProjectSkills",
                newName: "IX_ProjectSkills_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSkills",
                table: "ProjectSkills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSkills_Projects_ProjectId",
                table: "ProjectSkills",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSkills_Projects_ProjectId",
                table: "ProjectSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSkills",
                table: "ProjectSkills");

            migrationBuilder.RenameTable(
                name: "ProjectSkills",
                newName: "ProjectSkill");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectSkills_ProjectId",
                table: "ProjectSkill",
                newName: "IX_ProjectSkill_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSkill",
                table: "ProjectSkill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSkill_Projects_ProjectId",
                table: "ProjectSkill",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
