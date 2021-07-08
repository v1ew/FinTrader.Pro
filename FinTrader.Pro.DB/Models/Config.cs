namespace FinTrader.Pro.DB.Models
{
    /// <summary>
    /// Параметры программы
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Количество облигаций в портфеле
        /// </summary>
        public int BondsCount { get; set; }
        
        /// <summary>
        /// Максимальная доходность к погашению при отборе облигаций
        /// </summary>
        public int MaxYield { get; set; }
    }
}