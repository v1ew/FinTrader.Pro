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
            var boardIds = new[]
            {
                "TQCB",
                "TQOB",
            };
            var request = new MarketSecuritiesListRequest(issClient);
            var bonds = await request.FetchAsync("stock", "bonds", new Dictionary<string, string> {
                { "iss.only", "securities" },
                { "iss.meta", "off" },
                { "iss.df", "%d-%m-%Y" },
                { "iss.tf", "%H:%M:%S" },
                { "security_collection", "stock_bonds_all" },
                { "securities.columns", "SECID,BOARDID,SHORTNAME,ISIN,FACEUNIT,CURRENCYID,COUPONVALUE,COUPONPERCENT,ACCRUEDINT,LOTSIZE,LOTVALUE,FACEVALUE,MATDATE,COUPONPERIOD,ISSUESIZE,OFFERDATE,STATUS,SECTYPE,SECNAME" }
            });

            // Здесь фильтруем по режиму торгов
            var data = bonds.Securities.Data.Where(b => boardIds.Contains(b[BondsColumnNames.BoardId]));

            return data;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> LoadBondsMarketDataAsync()
        {
            var request = new MarketSecuritiesListRequest(issClient);
            var bonds = await request.FetchAsync("stock", "bonds", new Dictionary<string, string> {
                { "iss.only", "marketdata" },
                { "iss.meta", "off" },
                { "iss.df", "%d-%m-%Y" },
                { "iss.tf", "%H:%M:%S" },
                { "marketdata.columns", "SECID,BOARDID,NUMTRADES,VOLTODAY,DURATION,MARKETPRICETODAY,YIELDTOOFFER,YIELDLASTCOUPON,VALTODAY_RUR,LCURRENTPRICE" }
            });

            var data = bonds.Securities.Data;

            return data;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> LoadCouponsAsync(string secId)
        {
            var request = new BondCouponsRequest(issClient);
            var bondization = await request.FetchAsync(secId, new Dictionary<string, string>
            {
                { "iss.meta", "off" },
                { "iss.only", "coupons" },
                { "limit", "200" }
            });
            return bondization.Coupons.Data;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> LoadAmortizationsAsync(string secId)
        {
            var request = new BondCouponsRequest(issClient);
            var bondization = await request.FetchAsync(secId, new Dictionary<string, string>
            {
                { "iss.meta", "off" },
                { "iss.only", "amortizations" },
                { "limit", "200" }
            });
            return bondization.Amortizations.Data;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> LoadOffersAsync(string secId)
        {
            var request = new BondCouponsRequest(issClient);
            var bondization = await request.FetchAsync(secId, new Dictionary<string, string>
            {
                { "iss.meta", "off" },
                { "iss.only", "offers" },
                { "limit", "200" }
            });
            return bondization.Offers.Data;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> LoadBondsInfoAsync(string secId)
        {
            var request = new SecurityDefinitionRequest(issClient);
            var bondsInfo = await request.FetchAsync(secId, new Dictionary<string, string>
            {
                { "iss.meta", "off" },
                { "iss.only", "description" }
            });

            return bondsInfo.Description.Data;
        }
    }
}
