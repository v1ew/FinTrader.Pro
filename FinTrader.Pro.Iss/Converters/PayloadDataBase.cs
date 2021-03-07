using Newtonsoft.Json.Linq;

namespace FinTrader.Pro.Iss.Converters
{
    /// <summary>
    /// Базовый класс для чтения блока data ответов iss
    /// </summary>
    public abstract class PayloadDataBase
    {
        /// <summary>
        /// Инициализация из JArray, нужна для десереализации из json
        /// </summary>
        /// <param name="array"></param>
        public abstract void ReadFromJAray(JArray array);
    }
}
