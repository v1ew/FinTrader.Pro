using FinTrader.Pro.DB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FinTrader.Pro.DB.Converters
{
    public class JsonBondConverter : JsonConverter<Bond>
    {
        public override Bond ReadJson(JsonReader reader, Type objectType, [AllowNull] Bond existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                JArray sa = JArray.Load(reader);

                return sa.HasValues ? new Bond
                {
                    SecId = sa[0].Value<string>(),
                    BoardId = sa[1].Value<string>(),
                    ShortName = sa[2].Value<string>(),
                    PrevWaPrice = sa[3].Value<double?>(),
                    YieldAtPrevWaPrice = sa[4].Value<double?>(),
                    CouponValue = sa[5].Value<double?>(),
                    NextCoupon = sa[6].Value<DateTime?>(),
                    AccruedInt = sa[7].Value<double?>(),
                    PrevPrice = sa[8].Value<double?>(),
                    LotSize = sa[9].Value<int?>(),
                    FaceValue = sa[10].Value<double?>(),
                    BoardName = sa[11].Value<string>(),
                    Status = sa[12].Value<string>(),
                    MatDate = sa[13].Value<DateTime?>(),
                    Decimals = sa[14].Value<int?>(),
                    CouponPeriod = sa[15].Value<int?>(),
                    IssueSize = sa[16].Value<long?>(),
                    PrevLegalClosePrice = sa[17].Value<double?>(),
                    PrevAdmittedQuote = sa[18].Value<double?>(),
                    PrevDate = sa[19].Value<DateTime?>(),
                    SecName = sa[20].Value<string>(),
                    Remarks = sa[21].Value<string>(),
                    InstrId = sa[22].Value<string>(),
                    SectorId = sa[23].Value<string>(),
                    MarketCode = sa[24].Value<string>(),
                    MinStep = sa[25].Value<double?>(),
                    FaceUnit = sa[26].Value<string>(),
                    BuyBackPrice = sa[27].Value<double?>(),
                    BuyBackDate = dT(sa[28]),
                    IsIn = sa[29].Value<string>(),
                    LatName = sa[30].Value<string>(),
                    RegNumber = sa[31].Value<string>(),
                    CurrencyId = sa[32].Value<string>(),
                    IssueSizePlaced = sa[33].Value<long?>(),
                    ListLevel = sa[34].Value<int?>(),
                    SecType = sa[35].Value<string>(),
                    CouponPercent = sa[36].Value<double?>(),
                    OfferDate = sa[37].Value<DateTime?>(),
                    SettleDate = sa[38].Value<DateTime?>(),
                    LotValue = sa[39].Value<double?>(),

                } : new Bond();
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException($"Error converting value {reader.Value} to type '{objectType}'.", ex);
            }

            throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {nameof(IEnumerable<Guid>)}.");
        }

        private DateTime? dT(JToken date) => date.Value<string>() == "0000-00-00" ? null : date.Value<DateTime?>();

        public override void WriteJson(JsonWriter writer, [AllowNull] Bond value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
