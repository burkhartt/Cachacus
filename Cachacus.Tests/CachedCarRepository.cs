using System.Collections.Generic;
using Cachacus.Repositories;

namespace Cachacus.Tests {
    public class CachedCarRepository : AbstractCacheRepository<Car>, ICarRepository {
        private readonly CarRepository carRepository;

        public CachedCarRepository(CarRepository carRepository) {
            this.carRepository = carRepository;
        }

        public IEnumerable<Car> GetAllCars() {
            return Cache.GetAll(() => carRepository.GetAllCars());
        }

        protected override IEnumerable<Car> WarmUp() {
            return new[] {
                new Car("Red"),
                new Car("Blue")
            };
        }
    }
}