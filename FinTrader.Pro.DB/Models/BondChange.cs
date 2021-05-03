using System;

namespace FinTrader.Pro.DB.Models
{
    public class BondChange
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Код ценной бумаги
        /// </summary>
        public string SecId { get; set; }
        
        public string BoardId { get; set; }
        
        /// <summary>
        /// Отмечаем неподходящие бумаги
        /// </summary>
        public bool Discarded { get; set; } = false;
        
        public string ShortName { get; set; }
        
        /// <summary>
        /// Средневзвешенная цена предыдущего дня, % к номиналу
        /// </summary>
        public double? PrevWaPrice { get; set; }
        
        public double? YieldAtPrevWaPrice { get; set; }
        
        /// <summary>
        /// Сумма купона, в валюте номинала
        /// </summary>
        public double? CouponValue { get; set; }
        
        /// <summary>
        /// НКД
        /// </summary>
        public double? AccruedInt { get; set; }

        /// <summary>
        /// Цена последней сделки пред. дня, % к номиналу
        /// </summary>
        public double? PrevPrice { get; set; }
        
        public int? LotSize { get; set; }
        
        /// <summary>
        /// Первоначальная номинальная стоимость
        /// </summary>
        public double? InitialFaceValue { get; set; }
        
        /// <summary>
        /// Номинальная стоимость
        /// </summary>
        public double? FaceValue { get; set; }
        
        public string Status { get; set; }
        
        /// <summary>
        /// Дата погашения
        /// </summary>
        public DateTime? MatDate { get; set; }

        public int? Decimals { get; set; }
        
        public int? CouponPeriod { get; set; }
        
        /// <summary>
        /// Объем выпуска
        /// </summary>
        public long? IssueSize { get; set; }
        
        public double? PrevLegalClosePrice { get; set; }
        
        public double? PrevAdmittedQuote { get; set; }

        /// <summary>
        /// Дата последних торгов
        /// </summary>
        public DateTime? PrevDate { get; set; }
        
        /// <summary>
        /// Наименование бумаги
        /// </summary>
        public string SecName { get; set; }
        
        /// <summary>
        /// Валюта номинала
        /// </summary>
        public string FaceUnit { get; set; }
        
        /// <summary>
        /// Цена оферты
        /// </summary>
        public double? BuyBackPrice { get; set; }
        
        /// <summary>
        /// Дата, к которой рассчитывается доходность
        /// (если данное поле не заполнено, то \"Доходность посл.сделки\" рассчитывается к Дате погашения)
        /// </summary>
        public DateTime? BuyBackDate { get; set; }
        
        public string Isin { get; set; }
        
        /// <summary>
        /// Сопр. валюта инструмента
        /// </summary>
        public string CurrencyId { get; set; }
        
        /// <summary>
        /// Количество ценных бумаг в обращении
        /// </summary>
        public long? IssueSizePlaced { get; set; }
        
        /// <summary>
        /// Уровень листинга
        /// </summary>
        public int? ListLevel { get; set; }

        public string SecType { get; set; }
        
        /// <summary>
        /// Ставка купона, %
        /// </summary>
        public double? CouponPercent { get; set; }
        
        /// <summary>
        /// Дата оферты
        /// </summary>
        public DateTime? OfferDate { get; set; }
        
        /// <summary>
        /// Номинальная стоимость лота, в валюте номинала
        /// </summary>
        public double? LotValue { get; set; }
        
        /// <summary>
        /// Бумаги для квалифицированных инвесторов
        /// </summary>
        public bool? IsQualifiedInvestors { get; set; }

        /// <summary>
        /// Код эмитента
        /// </summary>
        public int? EmitterId { get; set; }
    }
}