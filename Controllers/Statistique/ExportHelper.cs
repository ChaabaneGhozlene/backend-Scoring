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
        public static byte[] ToPdf(object data, string reportType, StatFilterDto filter)
        {
            var rows = ToList(data);
            var sb   = new StringBuilder();

            sb.AppendLine("<html><body>");
            sb.AppendLine($"<h2>{reportType}</h2>");
            // ✅ DateFrom / DateTo (pas DateDebut / DateFin)
            sb.AppendLine($"<p>Du {filter.DateFrom:dd/MM/yyyy} au {filter.DateTo:dd/MM/yyyy}</p>");
            sb.AppendLine("<table border='1' cellpadding='4' cellspacing='0'>");

            if (rows.Any())
            {
                var props = rows[0].GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                sb.AppendLine("<tr>" +
                    string.Concat(props.Select(p => $"<th>{p.Name}</th>")) +
                    "</tr>");

                foreach (var row in rows)
                    sb.AppendLine("<tr>" +
                        string.Concat(props.Select(p =>
                            $"<td>{p.GetValue(row) ?? ""}</td>")) +
                        "</tr>");
            }

            sb.AppendLine("</table></body></html>");
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        // ─── RTF ──────────────────────────────────────────────────────────────
        public static byte[] ToRtf(object data, string reportType, StatFilterDto filter)
        {
            var rows = ToList(data);
            var sb   = new StringBuilder();

            sb.AppendLine(@"{\rtf1\ansi");
            sb.AppendLine(@"{\b " + reportType + @"\b0}\par");
            // ✅ DateFrom / DateTo (pas DateDebut / DateFin)
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