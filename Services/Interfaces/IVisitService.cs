using YmmoApi.Dtos.Visits;

namespace YmmoApi.Services.Interfaces;

public interface IVisitService
{
    Task<ServiceResult<VisitResponseDto>> CreateAsync(VisitCreateDto dto, int clientId);
    Task<ServiceResult<List<VisitResponseDto>>> GetAgentCalendarAsync(int agentId, DateOnly? date);
    Task<ServiceResult<VisitResponseDto>> UpdateStatusAsync(int id, VisitStatusUpdateDto dto);
    Task<ServiceResult<List<TimeSlotDto>>> GetAvailabilityAsync(int agentId, DateOnly date);
}
