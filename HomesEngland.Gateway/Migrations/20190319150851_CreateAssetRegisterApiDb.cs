using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomesEngland.Gateway.Migrations
{
    public partial class CreateAssetRegisterApiDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assetregisterversions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    modifieddatetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetregisterversions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "authenticationtokens",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    referencenumber = table.Column<string>(nullable: true),
                    token = table.Column<string>(nullable: true),
                    expiry = table.Column<DateTime>(nullable: false),
                    emailaddress = table.Column<string>(nullable: true),
                    modifieddatetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authenticationtokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    modifieddatetime = table.Column<DateTime>(nullable: false),
                    programme = table.Column<string>(nullable: true),
                    equityowner = table.Column<string>(nullable: true),
                    schemeid = table.Column<int>(nullable: true),
                    locationlaregionname = table.Column<string>(nullable: true),
                    imsoldregion = table.Column<string>(nullable: true),
                    noofbeds = table.Column<int>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    propertyhousename = table.Column<string>(nullable: true),
                    propertystreetnumber = table.Column<string>(nullable: true),
                    propertystreet = table.Column<string>(nullable: true),
                    propertytown = table.Column<string>(nullable: true),
                    propertypostcode = table.Column<string>(nullable: true),
                    developingrslname = table.Column<string>(nullable: true),
                    lbha = table.Column<string>(nullable: true),
                    completiondateforhpistart = table.Column<DateTime>(nullable: true),
                    imsactualcompletiondate = table.Column<DateTime>(nullable: true),
                    imsexpectedcompletiondate = table.Column<DateTime>(nullable: true),
                    imslegalcompletiondate = table.Column<DateTime>(nullable: true),
                    hopcompletiondate = table.Column<DateTime>(nullable: true),
                    deposit = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    agencyequityloan = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    developerequityloan = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    shareofrestrictedequity = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    developerdiscount = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    mortgage = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    purchaseprice = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    fees = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    historicunallocatedfees = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    actualagencyequitycostincludinghomebuyagentfee = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    fulldisposaldate = table.Column<DateTime>(nullable: true),
                    originalagencypercentage = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    staircasingpercentage = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    newagencypercentage = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    invested = table.Column<int>(nullable: true),
                    month = table.Column<int>(nullable: true),
                    calendaryear = table.Column<int>(nullable: true),
                    mmyyyy = table.Column<string>(nullable: true),
                    row = table.Column<int>(nullable: true),
                    col = table.Column<int>(nullable: true),
                    hpistart = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    hpiend = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    hpiplusminus = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    agencypercentage = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    mortgageeffect = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    remainingagencycost = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    waestimatedpropertyvalue = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    agencyfairvaluedifference = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    impairmentprovision = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    fairvaluereserve = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    agencyfairvalue = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    disposalscost = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    durationinmonths = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    monthofcompletionsinceschemestart = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    disposalmonthsincecompletion = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    imspaymentdate = table.Column<DateTime>(nullable: true),
                    ispaid = table.Column<bool>(nullable: true),
                    isasset = table.Column<bool>(nullable: true),
                    propertytype = table.Column<string>(nullable: true),
                    tenure = table.Column<string>(nullable: true),
                    expectedstaircasingrate = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    estimatedsaleprice = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    estimatedvaluation = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    regionalsaleadjust = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    regionalstairadjust = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    notlimitedbyfirstcharge = table.Column<bool>(nullable: true),
                    earlymortgageifneverrepay = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    arrearseffectappliedorlimited = table.Column<string>(nullable: true),
                    relativesalepropertytypeandtenureadjustment = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    relativestairpropertytypeandtenureadjustment = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    islondon = table.Column<bool>(nullable: true),
                    quarterspend = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    mortgageprovider = table.Column<string>(nullable: true),
                    housetype = table.Column<string>(nullable: true),
                    purchasepriceband = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    householdincome = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    householdfivekincomeband = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    householdfiftykincomeband = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    firsttimebuyer = table.Column<bool>(nullable: true),
                    assetregisterversionid = table.Column<int>(nullable: true),
                    AssetRegisterVersionEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.id);
                    table.ForeignKey(
                        name: "FK_assets_assetregisterversions_AssetRegisterVersionEntityId",
                        column: x => x.AssetRegisterVersionEntityId,
                        principalTable: "assetregisterversions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assets_assetregisterversions_assetregisterversionid",
                        column: x => x.assetregisterversionid,
                        principalTable: "assetregisterversions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assets_AssetRegisterVersionEntityId",
                table: "assets",
                column: "AssetRegisterVersionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_assets_assetregisterversionid",
                table: "assets",
                column: "assetregisterversionid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "authenticationtokens");

            migrationBuilder.DropTable(
                name: "assetregisterversions");
        }
    }
}
