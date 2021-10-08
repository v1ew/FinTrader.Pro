using System;

namespace FinTrader.Pro.DB.Models
{
    public class Bond
    {
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
        
        /// <summary>
        /// Доходность по оценке пред. дня
        /// </summary>
        public double? YieldAtPrevWaPrice { get; set; }
        
        /// <summary>
        /// Дата окончания купона
        /// </summary>
        public DateTime? NextCoupon { get; set; }
        
        /// <summary>
        /// Сумма купона, в валюте номинала
        /// </summary>
        public double? CouponValue { get; set; }
        
        /// <summary>
        /// НКД
        /// </summary>
        public double? AccruedInt { get; set; }

        public int? LotSize { get; set; }
        
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
        
        /// <summary>
        /// Наименование бумаги
        /// </summary>
        public string SecName { get; set; }
        
        /// <summary>
        /// Валюта номинала
        /// </summary>
        public string FaceUnit { get; set; }
        
        public string Isin { get; set; }
        
        /// <summary>
        /// Сопр. валюта инструмента
        /// </summary>
        public string CurrencyId { get; set; }
        
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
        /// Код эмитента
        /// </summary>
        public int? EmitterId { get; set; }
        
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
        public double? Yield { get; set; }

        /// <summary>
        /// Объем заключенных сделок в среднем за 5 дней
        /// </summary>
        public double? ValueAvg { get; set; }
        
        /// <summary>
        /// Комментарий, причина дисквалификации облигации
        /// </summary>
        public string Comment { get; set; }
        
        /// <summary>
        /// Время последнего изменения записи
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
