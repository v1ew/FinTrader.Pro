using System;

namespace FinTrader.Pro.DB.Models
{
    public class MarketRecord
    {
        /// <summary>
        /// Код ценной бумаги
        /// </summary>
        public string SecId { get; set; }
        
        /// <summary>
        /// Дата торгов
        /// </summary>
        public DateTime TradeDate { get; set; }
        
        /// <summary>
        /// Дюрация в днях
        /// </summary>
        public double? Duration { get; set; }
        
        /// <summary>
        /// Вычисляется по формуле:
        /// Дюрация / ((1 + Доходность / 100) * 365) 
        /// </summary>
        public double? ModifiedDuration { get; set; }
        
        /// <summary>
        /// Доходность
        /// </summary>
        public int? Yield { get; set; }

        /// <summary>
        /// Объем заключенных сделок в единицах ценных бумаг, штук
        /// </summary>
        public int? Value { get; set; }
        
        /// <summary>
        /// Объем: для ОФЗ - в рублях. Для облигаций в Т+ (кроме ОФЗ) - в долларах США.
        /// Для номинированных в евро – в евро, для остальных облигаций в Т0 - в рублях
        /// </summary>
        public int? Volume { get; set; }
        
        /// <summary>
        /// Время последнего изменения записи
        /// </summary>
        public DateTime ChangedTime { get; set; }
    }
}