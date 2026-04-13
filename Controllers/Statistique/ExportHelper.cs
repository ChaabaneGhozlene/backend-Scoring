using System.Text;
using System.Reflection;
using ClosedXML.Excel;
using scoring_Backend.DTO.Statistique;

namespace scoring_Backend.Helpers
{
    public static class ExportHelper
    {
        // ─── CSV ──────────────────────────────────────────────────────────────
        public static byte[] ToCsv(object data)
        {
            var rows = ToList(data);
            var sb   = new StringBuilder();
            if (!rows.Any()) return Encoding.UTF8.GetBytes("");

            var props = rows[0].GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            sb.AppendLine(string.Join(";", props.Select(p => p.Name)));

            foreach (var row in rows)
                sb.AppendLine(string.Join(";", props.Select(p =>
                    p.GetValue(row)?.ToString()?.Replace(";", ",") ?? "")));

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        // ─── XLS ──────────────────────────────────────────────────────────────
        public static byte[] ToXls(object data)
        {
            var rows = ToList(data);
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Export");

            if (!rows.Any())
            {
                using var emptyStream = new MemoryStream();
                wb.SaveAs(emptyStream);
                return emptyStream.ToArray();
            }

            var props = rows[0].GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // En-têtes
            for (int c = 0; c < props.Length; c++)
                ws.Cell(1, c + 1).Value = props[c].Name;

            // Lignes
            for (int r = 0; r < rows.Count; r++)
                for (int c = 0; c < props.Length; c++)
                {
                    var val = props[c].GetValue(rows[r]);
                    ws.Cell(r + 2, c + 1).Value = val?.ToString() ?? "";
                }

            ws.Row(1).Style.Font.Bold = true;
            ws.ColumnsUsed().AdjustToContents();

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return stream.ToArray();
        }

        // ─── PDF ──────────────────────────────────────────────────────────────
        // chartImageBase64 : image PNG encodée en base64 capturée côté frontend
        //                    null/vide → PDF sans graphique (comportement précédent)
        public static byte[] ToPdf(
            object data,
            string reportType,
            StatFilterDto filter,
            string? chartImageBase64 = null)
        {
            var rows = ToList(data);
            var sb   = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html><html><head><meta charset='utf-8'>");
            sb.AppendLine(@"<style>
                body  { font-family: Arial, sans-serif; font-size: 12px; margin: 20px; color: #1f2937; }
                h2    { color: #312e81; margin-bottom: 4px; }
                .meta { color: #6b7280; font-size: 11px; margin-bottom: 16px; }
                table { border-collapse: collapse; width: 100%; margin-bottom: 24px; }
                th    { background: #eef2ff; color: #4338ca; font-size: 11px;
                        text-transform: uppercase; letter-spacing: .04em;
                        padding: 8px 10px; text-align: left; border-bottom: 2px solid #c7d2fe; }
                td    { padding: 7px 10px; border-bottom: 1px solid #f3f4f6; }
                tr:nth-child(even) td { background: #f9fafb; }
                .chart-section h3 { color: #374151; font-size: 13px; margin-bottom: 8px; }
                .chart-section img { max-width: 100%; border: 1px solid #e5e7eb;
                                     border-radius: 8px; display: block; }
            </style>");
            sb.AppendLine("</head><body>");

            // ── Titre & méta ────────────────────────────────────────────────
            sb.AppendLine($"<h2>{System.Web.HttpUtility.HtmlEncode(reportType)}</h2>");
            sb.AppendLine($"<p class='meta'>Période : {filter.DateFrom:dd/MM/yyyy} → {filter.DateTo:dd/MM/yyyy}</p>");

            // ── Tableau de données ──────────────────────────────────────────
            if (rows.Any())
            {
                var props = rows[0].GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                sb.AppendLine("<table>");
                sb.AppendLine("<thead><tr>" +
                    string.Concat(props.Select(p => $"<th>{p.Name}</th>")) +
                    "</tr></thead><tbody>");

                foreach (var row in rows)
                    sb.AppendLine("<tr>" +
                        string.Concat(props.Select(p =>
                            $"<td>{System.Web.HttpUtility.HtmlEncode(p.GetValue(row)?.ToString() ?? "")}</td>")) +
                        "</tr>");

                sb.AppendLine("</tbody></table>");
            }
            else
            {
                sb.AppendLine("<p style='color:#9ca3af'>Aucune donnée.</p>");
            }

            // ── Graphique (uniquement si l'image est fournie) ────────────────
            if (!string.IsNullOrWhiteSpace(chartImageBase64))
            {
                // Supprimer le préfixe data:image/png;base64, si présent
                var base64Only = chartImageBase64.Contains(',')
                    ? chartImageBase64.Split(',')[1]
                    : chartImageBase64;

                sb.AppendLine("<div class='chart-section'>");
                sb.AppendLine("<h3>Graphique</h3>");
                sb.AppendLine($"<img src='data:image/png;base64,{base64Only}' alt='Graphique' />");
                sb.AppendLine("</div>");
            }

            sb.AppendLine("</body></html>");
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        // ─── RTF ──────────────────────────────────────────────────────────────
        public static byte[] ToRtf(object data, string reportType, StatFilterDto filter)
        {
            var rows = ToList(data);
            var sb   = new StringBuilder();

            sb.AppendLine(@"{\rtf1\ansi");
            sb.AppendLine(@"{\b " + reportType + @"\b0}\par");
            sb.AppendLine($"Du {filter.DateFrom:dd/MM/yyyy} au {filter.DateTo:dd/MM/yyyy}\\par");

            if (rows.Any())
            {
                var props = rows[0].GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var row in rows)
                {
                    var line = string.Join(" | ", props.Select(p =>
                        $"{p.Name}: {p.GetValue(row) ?? ""}"));
                    sb.AppendLine(line + @"\par");
                }
            }

            sb.AppendLine("}");
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        // ─── Helper interne ───────────────────────────────────────────────────
        private static List<object> ToList(object data)
        {
            if (data is IEnumerable<object> enumerable)
                return enumerable.ToList();
            return new List<object> { data };
        }
    }
}