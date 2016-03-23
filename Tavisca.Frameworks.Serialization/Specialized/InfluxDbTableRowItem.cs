using System;
using System.Globalization;

namespace Tavisca.Frameworks.Serialization.Specialized
{
    public sealed class InfluxDbTableRowItem
    {
        public object Value { get; set; }
        public InfluxDbTableRowItemType DataType { get; set; }

        public static implicit operator InfluxDbTableRowItem(string val)
        {
            return new InfluxDbTableRowItem()
                {
                    DataType = InfluxDbTableRowItemType.String,
                    Value = string.IsNullOrWhiteSpace(val) ? null : (val.Length > 128 ? val.Substring(0, 127) : val) //limiting values in data warehouse to 128.
                };
        }

        public static implicit operator InfluxDbTableRowItem(Guid val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Guid,
                Value = val.ToString()
            };
        }

        public static implicit operator InfluxDbTableRowItem(Nullable<Guid> val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Guid,
                Value = val.HasValue ? val.Value.ToString() : null
            };
        }

        public static implicit operator InfluxDbTableRowItem(int val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Int,
                Value = val.ToString(CultureInfo.InvariantCulture)
            };
        }

        public static implicit operator InfluxDbTableRowItem(Nullable<int> val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Int,
                Value = val.HasValue ? val.Value.ToString(CultureInfo.InvariantCulture) : null
            };
        }

        public static implicit operator InfluxDbTableRowItem(bool val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Bool,
                Value = val
            };
        }

        public static implicit operator InfluxDbTableRowItem(Nullable<bool> val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Bool,
                Value = val.HasValue ? val : null
            };
        }

        public static implicit operator InfluxDbTableRowItem(double val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Double,
                Value = val.ToString(CultureInfo.InvariantCulture)
            };
        }

        public static implicit operator InfluxDbTableRowItem(Nullable<double> val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Double,
                Value = val.HasValue ? val.Value.ToString(CultureInfo.InvariantCulture) : null
            };
        }

        public static implicit operator InfluxDbTableRowItem(decimal val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Decimal,
                Value = val.ToString(CultureInfo.InvariantCulture)
            };
        }

        public static implicit operator InfluxDbTableRowItem(Nullable<decimal> val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Decimal,
                Value = val.HasValue ? val.Value.ToString(CultureInfo.InvariantCulture) : null
            };
        }

        public static implicit operator InfluxDbTableRowItem(float val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Float,
                Value = val.ToString(CultureInfo.InvariantCulture)
            };
        }

        public static implicit operator InfluxDbTableRowItem(Nullable<float> val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Float,
                Value = val.HasValue ? val.Value.ToString(CultureInfo.InvariantCulture) : null
            };
        }

        public static implicit operator InfluxDbTableRowItem(long val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Long,
                Value = val.ToString(CultureInfo.InvariantCulture)
            };
        }

        public static implicit operator InfluxDbTableRowItem(Nullable<long> val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.Long,
                Value = val.HasValue ? val.Value.ToString(CultureInfo.InvariantCulture) : null
            };
        }

        public static implicit operator InfluxDbTableRowItem(DateTime val)
        {
            return new InfluxDbTableRowItem()
                {
                    DataType = InfluxDbTableRowItemType.DateTime,
                    Value = val
                };
        }

        public static implicit operator InfluxDbTableRowItem(Nullable<DateTime> val)
        {
            return new InfluxDbTableRowItem()
            {
                DataType = InfluxDbTableRowItemType.DateTime,
                Value = val.HasValue ? val.Value : DateTime.MinValue
            };
        }
    }
}