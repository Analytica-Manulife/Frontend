using System.Text.Json.Serialization;

namespace Frontend.Model
{
    public class YahooStock
    {
        public string? Symbol { get; set; }
        public string? AssetType { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CIK { get; set; }
        public string? Exchange { get; set; }
        public string? Currency { get; set; }
        public string? Country { get; set; }
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public string? Address { get; set; }
        public string? OfficialSite { get; set; }
        public string? FiscalYearEnd { get; set; }
        public string? LatestQuarter { get; set; }
        
        [JsonConverter(typeof(NullableLongConverter))]
        public long? MarketCapitalization { get; set; }

        [JsonConverter(typeof(NullableLongConverter))]
        public long? EBITDA { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? PERatio { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? PEGRatio { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? BookValue { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? DividendPerShare { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? DividendYield { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? EPS { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? RevenuePerShareTTM { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? ProfitMargin { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? OperatingMarginTTM { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? ReturnOnAssetsTTM { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? ReturnOnEquityTTM { get; set; }

        [JsonConverter(typeof(NullableLongConverter))]
        public long? RevenueTTM { get; set; }

        [JsonConverter(typeof(NullableLongConverter))]
        public long? GrossProfitTTM { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? DilutedEPSTTM { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? QuarterlyEarningsGrowthYOY { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? QuarterlyRevenueGrowthYOY { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]
        public double? AnalystTargetPrice { get; set; }
        
        [JsonConverter(typeof(NullableIntConverter))]

        public int? AnalystRatingStrongBuy { get; set; }
        [JsonConverter(typeof(NullableIntConverter))]

        public int? AnalystRatingBuy { get; set; }
        [JsonConverter(typeof(NullableIntConverter))]

        public int? AnalystRatingHold { get; set; }
        [JsonConverter(typeof(NullableIntConverter))]

        public int? AnalystRatingSell { get; set; }
        [JsonConverter(typeof(NullableIntConverter))]

        public int? AnalystRatingStrongSell { get; set; }

        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? TrailingPE { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? ForwardPE { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? PriceToSalesRatioTTM { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? PriceToBookRatio { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? EVToRevenue { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? EVToEBITDA { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? Beta { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? _52WeekHigh { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? _52WeekLow { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? _50DayMovingAverage { get; set; }
        [JsonConverter(typeof(NullableDoubleConverter))]

        public double? _200DayMovingAverage { get; set; }

        [JsonConverter(typeof(NullableLongConverter))]
        public long? SharesOutstanding { get; set; }

        public string? DividendDate { get; set; }
        public string? ExDividendDate { get; set; }
    }
}
