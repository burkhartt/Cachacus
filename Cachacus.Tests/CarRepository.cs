using System.Collections.Generic;

namespace Cachacus.Tests {
    public class CarRepository : ICarRepository {
        private Car[] cars;

        public CarRepository() {
            cars = new Car[] {
                new Car("Red"),
                new Car("Blue")
            };
        }

        public IEnumerable<Car> GetAllCars() {
            throw new System.NotImplementedException();
        }
    }
}