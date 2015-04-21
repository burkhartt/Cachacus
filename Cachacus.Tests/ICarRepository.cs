using System.Collections.Generic;

namespace Cachacus.Tests {
    public interface ICarRepository {
        IEnumerable<Car> GetAllCars();
    }
}