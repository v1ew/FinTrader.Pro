using System;

namespace FinTrader.Pro.DB.Models
{
    /// <summary>
    /// Даты загруженные в БД
    /// </summary>
    public class TradeDate
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}