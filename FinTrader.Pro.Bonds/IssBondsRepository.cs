using FinTrader.Pro.DB.Models;
using FinTrader.Pro.Iss;
using FinTrader.Pro.Iss.Columns;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using FinTrader.Pro.Iss.Requests;

namespace FinTrader.Pro.Bonds
{
    public class IssBondsRepository : IIssBondsRepository
    {
        private IIssClient issClient;

        public IssBondsRepository(IIssClient client)
        {
            issClient = client;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> LoadBondsAsync()
        {
            var request = new MarketSecuritiesListRequest(issClient);
            var bonds = await request.FetchAsync("stock", "bonds", new Dictionary<string, string> { 
                { "iss.only", "securities" },
                { "iss.meta", "off" },
                { "iss.df", "%d-%m-%Y" },
                { "iss.tf", "%H:%M:%S" },
                { "securities.columns", "SECID,BOARDID,SHORTNAME,FACEUNIT,CURRENCYID,COUPONVALUE,NEXTCOUPON,COUPONPERCENT,LOTSIZE,LOTVALUE,FACEVALUE,MATDATE,COUPONPERIOD,ISSUESIZE,REGNUMBER,OFFERDATE,STATUS,SECTYPE,SECNAME,LATNAME" }
            });

            var data = bonds.Securities.Data.Where(b => b[BondsColumnNames.FaceUnit] == "SUR" && b[BondsColumnNames.CurrencyId] == "SUR").OrderByDescending(b => b[BondsColumnNames.CouponPercent]);
            //data = data.Where();

            return data;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> LoadCouponsAsync(string secId)
        {
            var request = new BondCouponsRequest(issClient);
            var coupons = await request.FetchAsync(secId, new Dictionary<string, string>
            {
                { "iss.meta", "off" }
            });
            // TODO: return offers
            return coupons.Coupons.Data;
        }
    }
}
