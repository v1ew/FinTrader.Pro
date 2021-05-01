using System;

namespace FinTrader.Pro.DB.Models
{
    /// <summary>
    /// В таблице сохраняются обновления данных по облигациям
    /// </summary>
    public class Updating
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// В какой таблице произошли изменения - Bonds или Coupons
        /// </summary>
        public string Table { get; set; }
        
        /// <summary>
        /// ISIN бумаги
        /// </summary>
        public string Isin { get; set; }
        
        /// <summary>
        /// Имя поля, которое изменилось
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Старое значение поля, приведенное к строке
        /// </summary>
        public string OldValue { get; set; }
        
        /// <summary>
        /// Новое значение поля, в виде строки
        /// </summary>
        public string NewValue { get; set; }
        
        /// <summary>
        /// Дата и время изменения
        /// </summary>
        public DateTime ChangedTime { get; set; }
    }
}