using Google.OrTools.LinearSolver;
using WebOptimize.Application.DTOs;
using WebOptimize.Application.Interfaces;
using WebOptimize.Domain.Entities;

namespace WebOptimize.Application.Services
{
    public class OptimizationService : IOptimizationService
    {
        public Task<OptimizationResponse> OptimizeAsync(OptimizationRequest request)
        {
            if (!request.Warehouses.Any() || !request.PickupPoints.Any())
                return Task.FromResult(new OptimizationResponse { Message = "Нет данных для оптимизации" });

            var solver = Solver.CreateSolver("SCIP");
            if (solver == null)
                throw new Exception("Не удалось создать solver");

            // Подготовка данных
            var warehouses = request.Warehouses;
            var pickupPoints = request.PickupPoints;
            var products = warehouses.SelectMany(w => w.Stock.Keys)
                                    .Union(pickupPoints.SelectMany(p => p.Demand.Keys))
                                    .Distinct()
                                    .ToList();

            var shipments = new List<Shipment>();
            double totalDistance = 0;
            double totalCost = 0;

            foreach (var product in products)
            {
                var supply = warehouses.Select((w, i) => (Index: i, Supply: w.Stock.GetValueOrDefault(product, 0))).ToList();
                var demand = pickupPoints.Select((p, j) => (Index: j, Demand: p.Demand.GetValueOrDefault(product, 0))).ToList();

                if (supply.All(s => s.Supply == 0) || demand.All(d => d.Demand == 0))
                    continue;

                // x[i,j] — сколько товара product отправить со склада i в ПВЗ j
                var x = new Variable[warehouses.Count, pickupPoints.Count];
                for (int i = 0; i < warehouses.Count; i++)
                    for (int j = 0; j < pickupPoints.Count; j++)
                    {
                        x[i, j] = solver.MakeIntVar(0, Math.Max(supply[i].Supply, demand[j].Demand), $"x_{i}_{j}_{product}");
                    }

                // ограничение по предложению
                for (int i = 0; i < warehouses.Count; i++)
                {
                    var constraint = solver.MakeConstraint(0, supply[i].Supply);
                    for (int j = 0; j < pickupPoints.Count; j++)
                        constraint.SetCoefficient(x[i, j], 1);
                }

                // ограничение по спросу
                for (int j = 0; j < pickupPoints.Count; j++)
                {
                    var constraint = solver.MakeConstraint(demand[j].Demand, demand[j].Demand);
                    for (int i = 0; i < warehouses.Count; i++)
                        constraint.SetCoefficient(x[i, j], 1);
                }

                // целевая функция = минимизация расстояния * количество
                var objective = solver.Objective();
                objective.SetMinimization();

                for (int i = 0; i < warehouses.Count; i++)
                    for (int j = 0; j < pickupPoints.Count; j++)
                    {
                        double dist = warehouses[i].Location.DistanceTo(pickupPoints[j].Location);
                        objective.SetCoefficient(x[i, j], dist);
                    }

                //
                var resultStatus = solver.Solve();

                if (resultStatus == Solver.ResultStatus.OPTIMAL || resultStatus == Solver.ResultStatus.FEASIBLE)
                {
                    for (int i = 0; i < warehouses.Count; i++)
                        for (int j = 0; j < pickupPoints.Count; j++)
                        {
                            double quantity = x[i, j].SolutionValue();
                            if (quantity > 0.5) // округление
                            {
                                double dist = warehouses[i].Location.DistanceTo(pickupPoints[j].Location);
                                var shipment = new Shipment
                                {
                                    WarehouseId = warehouses[i].Id,
                                    PickupPointId = pickupPoints[j].Id,
                                    ProductId = product,
                                    Quantity = (int)quantity,
                                    Distance = dist,
                                    Cost = dist * 10
                                };

                                shipments.Add(shipment);
                                totalDistance += dist * quantity;
                                totalCost += shipment.Cost * quantity;
                            }
                        }
                }
            }

            return Task.FromResult(new OptimizationResponse
            {
                Shipments = shipments,
                TotalDistance = totalDistance,
                TotalCost = totalCost,
                Message = $"Оптимизация завершена. Статус: Optimal"
            });
        }
    }
}
