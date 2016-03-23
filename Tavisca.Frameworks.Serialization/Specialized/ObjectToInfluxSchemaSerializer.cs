using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Exceptions;

namespace Tavisca.Frameworks.Serialization.Specialized
{
    public sealed class ObjectToInfluxSchemaSerializer
    {
        public ICollection<string> Serialize(IInfluxDbData obj)
        {
            if (obj == null)
                return null;

            var tables = obj.GetTables();

            return Serialize(tables);
        }

        public ICollection<string> Serialize(ICollection<InfluxDbTable> tables)
        {
            if (tables == null || tables.Count == 0)
                return null;

            ValidateTables(tables);

            var retVal = new List<string>(tables.Count);

            foreach (var influxDbTable in tables)
            {
                retVal.Add(ConvertToInfluxString(influxDbTable));
            }

            return retVal;
        }

        private const string InfluxTableTemplate = "[{\"name\":\"{name}\",\"columns\":[{columns}],\"points\":[{rows}]}]";

        private string ConvertToInfluxString(InfluxDbTable table)
        {
            var stringedColumns = table.Columns.Select(x => "\"" + x + "\"");

            var columns = string.Join(",", stringedColumns);

            var rows = GetRowsString(table.Rows);

            return InfluxTableTemplate
                .Replace("{name}", table.Name)
                .Replace("{columns}", columns)
                .Replace("{rows}", rows);
        }

        private string GetRowsString(IEnumerable<InfluxDbTableRow> rows)
        {
            var builder = new StringBuilder();

            foreach (var influxDbTableRow in rows)
            {
                builder.Append("[");

                foreach (var influxDbTableRowItem in influxDbTableRow.Items)
                {
                    builder.Append(GetItemString(influxDbTableRowItem))
                        .Append(",");
                }
                builder.Remove(builder.Length - 1, 1);

                builder.Append("],");
            }

            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        private string GetItemString(InfluxDbTableRowItem item)
        {
            if (item == null)
                return "null";

            var obj = item.Value ?? string.Empty;

            switch (item.DataType)
            {
                case InfluxDbTableRowItemType.Double:
                case InfluxDbTableRowItemType.Float:
                case InfluxDbTableRowItemType.Decimal:
                case InfluxDbTableRowItemType.Int:
                case InfluxDbTableRowItemType.Long:
                    return obj.ToString();
                case InfluxDbTableRowItemType.DateTime:
                    return "\"" + GetEpoch(item.Value) + "\"";
                case InfluxDbTableRowItemType.Bool:
                    return (bool)item.Value ? "1" : "0";
                case InfluxDbTableRowItemType.Guid:
                    return "\"" + obj.ToString() + "\"";
                case InfluxDbTableRowItemType.String:
                default:
                return "\"" + EscapeJsonCharacters(obj.ToString()) + "\"";
            }
        }

        private static string EscapeJsonCharacters(string value)
        {
            return value.Replace("\"", "\\\"").Replace("\"", "\\\"");
        }

        private readonly static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static string GetEpoch(object value)
        {
            var time = value is DateTime ? ((DateTime)value) : Epoch;

            if (time < Epoch)
                time = DateTime.UtcNow;

            if (time.Kind == DateTimeKind.Unspecified)
            {
                time = DateTime.SpecifyKind(time, DateTimeKind.Utc);
            }

            if (time.Kind != DateTimeKind.Utc)
                time = time.ToUniversalTime();

            return time.ToString("yyyyMMdd HH:mm:ss.fff"); //20080215 16:05:06.789
            //return Convert.ToInt64(Math.Floor((time - Epoch).TotalMilliseconds));
        }

        private static void ValidateTables(ICollection<InfluxDbTable> tables)
        {
            var messageBuilder = new StringBuilder().Append(Resources.SerializationResources.IInfluxDbData_Tables_Invalid).Append(Environment.NewLine);
            var throwEx = false;

            if (tables == null)
            {
                messageBuilder.Append("the tables collection returned was null").Append(Environment.NewLine);
                throw new SerializationException(messageBuilder.ToString());
            }

            if (tables.Any(x => x == null))
            {
                messageBuilder.Append("the tables collection contained a null value").Append(Environment.NewLine);
                throwEx = true;
            }

            if (tables.Any(x => string.IsNullOrWhiteSpace(x.Name)))
            {
                messageBuilder.Append("the tables collection had a table with a name which was null or empty.").Append(Environment.NewLine);
                throwEx = true;
            }

            var columns = tables.SelectMany(x => x.Columns).ToList();

            if (columns.Any(string.IsNullOrWhiteSpace))
            {
                messageBuilder.Append("the tables collection had a column with which was null or empty.").Append(Environment.NewLine);
                throwEx = true;
            }

            var rows = tables.SelectMany(x => x.Rows).ToList();

            if (rows.Any(x => x == null || x.Items == null))
            {
                messageBuilder.Append("the tables collection had a row which was either null itself or its items was null.").Append(Environment.NewLine);
                throwEx = true;
            }

            if (throwEx)
                throw new SerializationException(messageBuilder.ToString());
        }
    }
}
