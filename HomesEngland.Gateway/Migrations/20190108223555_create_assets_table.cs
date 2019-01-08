using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HomesEngland.Gateway.Migrations
{
    public partial class create_assets_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
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
                    deposit = table.Column<decimal>(nullable: true),
                    agencyequityloan = table.Column<decimal>(nullable: true),
                    developerequityloan = table.Column<decimal>(nullable: true),
                    shareofrestrictedequity = table.Column<decimal>(nullable: true),
                    developerdiscount = table.Column<decimal>(nullable: true),
                    mortgage = table.Column<decimal>(nullable: true),
                    purchaseprice = table.Column<decimal>(nullable: true),
                    fees = table.Column<decimal>(nullable: true),
                    historicunallocatedfees = table.Column<decimal>(nullable: true),
                    actualagencyequitycostincludinghomebuyagentfee = table.Column<decimal>(nullable: true),
                    fulldisposaldate = table.Column<DateTime>(nullable: true),
                    originalagencypercentage = table.Column<decimal>(nullable: true),
                    staircasingpercentage = table.Column<decimal>(nullable: true),
                    newagencypercentage = table.Column<decimal>(nullable: true),
                    invested = table.Column<int>(nullable: true),
                    month = table.Column<int>(nullable: true),
                    calendaryear = table.Column<int>(nullable: true),
                    mmyyyy = table.Column<string>(nullable: true),
                    row = table.Column<int>(nullable: true),
                    col = table.Column<int>(nullable: true),
                    hpistart = table.Column<decimal>(nullable: true),
                    hpiend = table.Column<decimal>(nullable: true),
                    hpiplusminus = table.Column<decimal>(nullable: true),
                    agencypercentage = table.Column<decimal>(nullable: true),
                    mortgageeffect = table.Column<decimal>(nullable: true),
                    remainingagencycost = table.Column<decimal>(nullable: true),
                    waestimatedpropertyvalue = table.Column<decimal>(nullable: true),
                    agencyfairvaluedifference = table.Column<decimal>(nullable: true),
                    impairmentprovision = table.Column<decimal>(nullable: true),
                    fairvaluereserve = table.Column<decimal>(nullable: true),
                    agencyfairvalue = table.Column<decimal>(nullable: true),
                    disposalscost = table.Column<decimal>(nullable: true),
                    durationinmonths = table.Column<decimal>(nullable: true),
                    monthofcompletionsinceschemestart = table.Column<decimal>(nullable: true),
                    disposalmonthsincecompletion = table.Column<decimal>(nullable: true),
                    imspaymentdate = table.Column<DateTime>(nullable: true),
                    ispaid = table.Column<bool>(nullable: true),
                    isasset = table.Column<bool>(nullable: true),
                    propertytype = table.Column<string>(nullable: true),
                    tenure = table.Column<string>(nullable: true),
                    expectedstaircasingrate = table.Column<decimal>(nullable: true),
                    estimatedsaleprice = table.Column<decimal>(nullable: true),
                    estimatedvaluation = table.Column<decimal>(nullable: true),
                    regionalsaleadjust = table.Column<decimal>(nullable: true),
                    regionalstairadjust = table.Column<decimal>(nullable: true),
                    notlimitedbyfirstcharge = table.Column<bool>(nullable: true),
                    earlymortgageifneverrepay = table.Column<decimal>(nullable: true),
                    arrearseffectappliedorlimited = table.Column<string>(nullable: true),
                    relativesalepropertytypeandtenureadjustment = table.Column<decimal>(nullable: true),
                    relativestairpropertytypeandtenureadjustment = table.Column<decimal>(nullable: true),
                    islondon = table.Column<bool>(nullable: true),
                    quarterspend = table.Column<decimal>(nullable: true),
                    mortgageprovider = table.Column<string>(nullable: true),
                    housetype = table.Column<string>(nullable: true),
                    purchasepriceband = table.Column<decimal>(nullable: true),
                    householdincome = table.Column<decimal>(nullable: true),
                    householdfivekincomeband = table.Column<decimal>(nullable: true),
                    householdfiftykincomeband = table.Column<decimal>(nullable: true),
                    firsttimebuyer = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assets");
        }
    }
}
