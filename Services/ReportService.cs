using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using OfficeOpenXml;
using YmmoApi.Dtos.Reports;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class ReportService(IReportRepository repository) : IReportService
{
    private const string SummaryReportType = "synthese";

    public async Task<byte[]> GenerateSalesExcelAsync(int? agencyId, DateOnly? from, DateOnly? to, string? reportType)
    {
        ExcelPackage.License.SetNonCommercialOrganization("Ymmo");

        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Ventes");

        if (IsSummaryReport(reportType))
        {
            var rows = await repository.GetSalesSummaryByAgencyAsync(agencyId, from, to);
            WriteHeader(sheet, "Agence", "Nb ventes", "CA total", "Prix moyen");

            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var line = i + 2;
                sheet.Cells[line, 1].Value = row.AgencyName;
                sheet.Cells[line, 2].Value = row.SalesCount;
                sheet.Cells[line, 3].Value = row.TotalRevenue;
                sheet.Cells[line, 4].Value = row.AverageSalePrice;
            }
        }
        else
        {
            var rows = await repository.GetSalesDetailAsync(agencyId, from, to);
            WriteHeader(sheet, "Id", "Bien", "Agence", "Acheteur", "Vendeur", "Prix de vente", "Date", "Commentaire");

            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var line = i + 2;
                sheet.Cells[line, 1].Value = row.Id;
                sheet.Cells[line, 2].Value = row.PropertyTitle;
                sheet.Cells[line, 3].Value = row.AgencyName;
                sheet.Cells[line, 4].Value = row.BuyerName;
                sheet.Cells[line, 5].Value = row.SellerName;
                sheet.Cells[line, 6].Value = row.SalePrice;
                sheet.Cells[line, 7].Value = row.Date.ToString("yyyy-MM-dd");
                sheet.Cells[line, 8].Value = row.Comment;
            }
        }

        sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }

    public async Task<byte[]> GenerateSalesPdfAsync(int? agencyId, DateOnly? from, DateOnly? to, string? reportType)
    {
        using var stream = new MemoryStream();
        using var writer = new PdfWriter(stream);
        using var pdfDocument = new PdfDocument(writer);
        using var document = new Document(pdfDocument);

        document.Add(new Paragraph("Rapport des ventes").SetFontSize(16));

        if (IsSummaryReport(reportType))
        {
            var rows = await repository.GetSalesSummaryByAgencyAsync(agencyId, from, to);
            var table = new Table(4, true).UseAllAvailableWidth();
            AddHeaderRow(table, "Agence", "Nb ventes", "CA total", "Prix moyen");

            foreach (var row in rows)
            {
                table.AddCell(row.AgencyName);
                table.AddCell(row.SalesCount.ToString());
                table.AddCell(row.TotalRevenue.ToString("N2"));
                table.AddCell(row.AverageSalePrice.ToString("N2"));
            }

            document.Add(table);
        }
        else
        {
            var rows = await repository.GetSalesDetailAsync(agencyId, from, to);
            var table = new Table(8, true).UseAllAvailableWidth();
            AddHeaderRow(table, "Id", "Bien", "Agence", "Acheteur", "Vendeur", "Prix de vente", "Date", "Commentaire");

            foreach (var row in rows)
            {
                table.AddCell(row.Id.ToString());
                table.AddCell(row.PropertyTitle);
                table.AddCell(row.AgencyName);
                table.AddCell(row.BuyerName);
                table.AddCell(row.SellerName);
                table.AddCell(row.SalePrice.ToString("N2"));
                table.AddCell(row.Date.ToString("yyyy-MM-dd"));
                table.AddCell(row.Comment);
            }

            document.Add(table);
        }

        document.Close();
        return stream.ToArray();
    }

    private static bool IsSummaryReport(string? reportType) =>
        string.Equals(reportType, SummaryReportType, StringComparison.OrdinalIgnoreCase);

    private static void WriteHeader(ExcelWorksheet sheet, params string[] headers)
    {
        for (var i = 0; i < headers.Length; i++)
            sheet.Cells[1, i + 1].Value = headers[i];

        sheet.Cells[1, 1, 1, headers.Length].Style.Font.Bold = true;
    }

    private static void AddHeaderRow(Table table, params string[] headers)
    {
        foreach (var header in headers)
            table.AddHeaderCell(header);
    }
}
