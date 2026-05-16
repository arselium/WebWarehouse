using WebOptimize.Application.DTOs;

namespace WebOptimize.Application.Interfaces
{
    public interface IOptimizationService
    {
        Task<OptimizationResponse> OptimizeAsync(OptimizationRequest request);
    }
}
