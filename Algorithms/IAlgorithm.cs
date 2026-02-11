using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace DriverFinder1.Algorithms
{
    /// <summary>
    /// Интерфейс для алгоритмов поиска ближайших водителей
    /// </summary>
    public interface IAlgorithm
    {
        /// <summary>
        /// Название алгоритма для идентификации в отчётах
        /// </summary>
        string AlgorithmName { get; }

        /// <summary>
        /// Находит ближайших водителей к заказу
        /// </summary>
        /// <param name="order">Заказ с координатами</param>
        /// <param name="drivers">Список доступных водителей</param>
        /// <param name="count">Количество водителей для возврата (по умолчанию 5)</param>
        /// <returns>Перечисление водителей, отсортированных по возрастанию расстояния</returns>
        IEnumerable<Driver> FindNearestDrivers(Order order, IEnumerable<Driver> drivers, int count = 5);
    }
}
