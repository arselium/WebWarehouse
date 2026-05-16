using WebOptimize.Application.DTOs;
using WebOptimize.Application.Interfaces;
using WebOptimize.Application.Services;
using WebOptimize.Domain.Entities;
using WebOptimize.Domain.ValueObjects;
using Xunit;

namespace WebOptimize.WebOptimize.Tests
{
    public class OptimizationServiceTests
    {
        private readonly IOptimizationService _service;

        public OptimizationServiceTests()
        {
            _service = new OptimizationService();
        }

        [Fact]
        public async Task OptimizeAsync_ShouldReturnValidResult_WhenValidRequestProvided()
        {
            // Arrange
            var request = new OptimizationRequest
            {
                Warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = 1,
                    Name = "Склад Москва",
                    Location = new Location { Latitude = 55.7558, Longitude = 37.6173 },
                    Stock = new Dictionary<string, int> { { "Товар1", 100 } }
                }
            },
                PickupPoints = new List<PickupPoint>
            {
                new PickupPoint
                {
                    Id = 1,
                    Name = "ПВЗ1",
                    Location = new Location { Latitude = 55.7600, Longitude = 37.6200 },
                    Demand = new Dictionary<string, int> { { "Товар1", 40 } }
                }
            }
            };

            // Act
            var result = await _service.OptimizeAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Shipments);
            Assert.True(result.TotalDistance > 0);
            Assert.Contains("Оптимизация завершена", result.Message);
        }

        [Fact]
        public async Task OptimizeAsync_ShouldReturnEmpty_WhenNoDataProvided()
        {
            // Arrange
            var request = new OptimizationRequest();

            // Act
            var result = await _service.OptimizeAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Shipments);
        }
    }
}
