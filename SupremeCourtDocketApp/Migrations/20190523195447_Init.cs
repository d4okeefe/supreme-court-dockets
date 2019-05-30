using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SupremeCourtDocketApp.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupremeCourtDocket",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocketNumber = table.Column<string>(nullable: true),
                    WebAddress = table.Column<string>(nullable: true),
                    WebPage = table.Column<string>(nullable: true),
                    DateRetrieved = table.Column<DateTime>(nullable: false),
                    DocketYear = table.Column<int>(nullable: true),
                    DocketNoYear = table.Column<int>(nullable: true),
                    CaseTitle = table.Column<string>(nullable: true),
                    LowerCourt = table.Column<string>(nullable: true),
                    LowerCourtCaseNumbers = table.Column<string>(nullable: true),
                    DateDocketed = table.Column<DateTime>(nullable: true),
                    DateOfDecision = table.Column<DateTime>(nullable: true),
                    DateOfRehearingDenied = table.Column<DateTime>(nullable: true),
                    DateOfDiscretionaryCourtDecision = table.Column<DateTime>(nullable: true),
                    Analyst = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupremeCourtDocket", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocketContacts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SupremeCourtDocketID = table.Column<int>(nullable: false),
                    PartyHeader = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PartyFooter = table.Column<string>(nullable: true),
                    PartyDescription = table.Column<string>(nullable: true),
                    PartyName = table.Column<string>(nullable: true),
                    NameBlock = table.Column<string>(nullable: true),
                    AddressBlock = table.Column<string>(nullable: true),
                    IsCounselOfRecord = table.Column<bool>(nullable: false),
                    AttorneyFullName = table.Column<string>(nullable: true),
                    AttorneySurname = table.Column<string>(nullable: true),
                    AttorneyEmail = table.Column<string>(nullable: true),
                    AttorneyCityStateZip = table.Column<string>(nullable: true),
                    IsCityStateZipValid = table.Column<bool>(nullable: false),
                    AttorneyCity = table.Column<string>(nullable: true),
                    AttorneyState = table.Column<string>(nullable: true),
                    AttorneyZip = table.Column<string>(nullable: true),
                    AttorneyOffice = table.Column<string>(nullable: true),
                    AttorneyStreetAddress = table.Column<string>(nullable: true),
                    PhoneNumberTenDigit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocketContacts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DocketContacts_SupremeCourtDocket_SupremeCourtDocketID",
                        column: x => x.SupremeCourtDocketID,
                        principalTable: "SupremeCourtDocket",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocketInfoLink",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    SupremeCourtDocketID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocketInfoLink", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DocketInfoLink_SupremeCourtDocket_SupremeCourtDocketID",
                        column: x => x.SupremeCourtDocketID,
                        principalTable: "SupremeCourtDocket",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocketProceedings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SupremeCourtDocketID = table.Column<int>(nullable: false),
                    ProceedingDate = table.Column<DateTime>(nullable: false),
                    SecondaryDate = table.Column<DateTime>(nullable: false),
                    ProceedingDescription = table.Column<string>(nullable: true),
                    TypeOfProceeding = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocketProceedings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DocketProceedings_SupremeCourtDocket_SupremeCourtDocketID",
                        column: x => x.SupremeCourtDocketID,
                        principalTable: "SupremeCourtDocket",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProceedingLink",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocketProceedingsID = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    LinkDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProceedingLink", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProceedingLink_DocketProceedings_DocketProceedingsID",
                        column: x => x.DocketProceedingsID,
                        principalTable: "DocketProceedings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocketContacts_SupremeCourtDocketID",
                table: "DocketContacts",
                column: "SupremeCourtDocketID");

            migrationBuilder.CreateIndex(
                name: "IX_DocketInfoLink_SupremeCourtDocketID",
                table: "DocketInfoLink",
                column: "SupremeCourtDocketID");

            migrationBuilder.CreateIndex(
                name: "IX_DocketProceedings_SupremeCourtDocketID",
                table: "DocketProceedings",
                column: "SupremeCourtDocketID");

            migrationBuilder.CreateIndex(
                name: "IX_ProceedingLink_DocketProceedingsID",
                table: "ProceedingLink",
                column: "DocketProceedingsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocketContacts");

            migrationBuilder.DropTable(
                name: "DocketInfoLink");

            migrationBuilder.DropTable(
                name: "ProceedingLink");

            migrationBuilder.DropTable(
                name: "DocketProceedings");

            migrationBuilder.DropTable(
                name: "SupremeCourtDocket");
        }
    }
}
