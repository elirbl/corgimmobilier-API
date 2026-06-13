using YmmoApi.Dtos.Visits;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class VisitService(IVisitRepository repository, IEmailService emailService) : IVisitService
{
    private static readonly TimeOnly BusinessDayStart = new(9, 0);
    private static readonly TimeOnly BusinessDayEnd = new(18, 0);
    private static readonly TimeSpan SlotDuration = TimeSpan.FromHours(1);

    public async Task<ServiceResult<VisitResponseDto>> CreateAsync(VisitCreateDto dto, int clientId)
    {
        var property = await repository.GetPropertyAsync(dto.PropertyId);
        if (property is null)
            return ServiceResult<VisitResponseDto>.Failure("Bien introuvable.");

        if (dto.AgentId.HasValue)
        {
            var agent = await repository.GetUserAsync(dto.AgentId.Value);
            if (agent is null || agent.Role != UserRole.Agent)
                return ServiceResult<VisitResponseDto>.Failure("Agent introuvable.");

            var slotEnd = dto.ScheduledAt.Add(SlotDuration);
            var activeVisits = await repository.GetActiveByAgentAndDateAsync(dto.AgentId.Value, DateOnly.FromDateTime(dto.ScheduledAt));
            var hasConflict = activeVisits.Any(v => dto.ScheduledAt < v.ScheduledAt.Add(SlotDuration) && v.ScheduledAt < slotEnd);
            if (hasConflict)
                return ServiceResult<VisitResponseDto>.Failure("Ce créneau n'est pas disponible pour cet agent.");
        }

        var visit = new Visit
        {
            PropertyId = dto.PropertyId,
            ClientId = clientId,
            AgentId = dto.AgentId,
            ScheduledAt = dto.ScheduledAt,
            Status = VisitStatus.Scheduled,
            Notes = dto.Notes ?? string.Empty
        };

        await repository.AddAsync(visit);
        await repository.SaveChangesAsync();

        var created = await repository.GetByIdWithDetailsAsync(visit.Id);

        if (created!.Client is not null)
        {
            await emailService.SendAsync(
                created.Client.Email,
                "Confirmation de votre visite",
                $"Bonjour {created.Client.FirstName},\n\nVotre visite du bien \"{property.Title}\" est confirmée pour le " +
                $"{created.ScheduledAt:dd/MM/yyyy à HH:mm}.\n\nL'équipe Ymmo.");
        }

        return ServiceResult<VisitResponseDto>.Success(ToResponseDto(created));
    }

    public async Task<ServiceResult<List<VisitResponseDto>>> GetAgentCalendarAsync(int agentId, DateOnly? date)
    {
        var agent = await repository.GetUserAsync(agentId);
        if (agent is null || agent.Role != UserRole.Agent)
            return ServiceResult<List<VisitResponseDto>>.Failure("Agent introuvable.");

        var visits = await repository.GetByAgentAsync(agentId, date);
        return ServiceResult<List<VisitResponseDto>>.Success(visits.Select(ToResponseDto).ToList());
    }

    public async Task<ServiceResult<VisitResponseDto>> UpdateStatusAsync(int id, VisitStatusUpdateDto dto)
    {
        var visit = await repository.GetByIdAsync(id);
        if (visit is null)
            return ServiceResult<VisitResponseDto>.Failure("Visite introuvable.");

        if (visit.Status != VisitStatus.Scheduled)
            return ServiceResult<VisitResponseDto>.Failure("Seule une visite planifiée peut changer de statut.");

        visit.Status = dto.Status;
        await repository.SaveChangesAsync();

        var updated = await repository.GetByIdWithDetailsAsync(id);
        return ServiceResult<VisitResponseDto>.Success(ToResponseDto(updated!));
    }

    public async Task<ServiceResult<List<TimeSlotDto>>> GetAvailabilityAsync(int agentId, DateOnly date)
    {
        var agent = await repository.GetUserAsync(agentId);
        if (agent is null || agent.Role != UserRole.Agent)
            return ServiceResult<List<TimeSlotDto>>.Failure("Agent introuvable.");

        var activeVisits = await repository.GetActiveByAgentAndDateAsync(agentId, date);

        var slots = new List<TimeSlotDto>();
        var current = date.ToDateTime(BusinessDayStart);
        var dayEnd = date.ToDateTime(BusinessDayEnd);

        while (current.Add(SlotDuration) <= dayEnd)
        {
            var slotEnd = current.Add(SlotDuration);
            var isFree = activeVisits.All(v => current >= v.ScheduledAt.Add(SlotDuration) || slotEnd <= v.ScheduledAt);

            if (isFree)
                slots.Add(new TimeSlotDto { Start = current, End = slotEnd });

            current = current.Add(SlotDuration);
        }

        return ServiceResult<List<TimeSlotDto>>.Success(slots);
    }

    private static VisitResponseDto ToResponseDto(Visit v) => new()
    {
        Id = v.Id,
        PropertyId = v.PropertyId,
        PropertyTitle = v.Property?.Title ?? string.Empty,
        ClientId = v.ClientId,
        ClientName = v.Client is null ? string.Empty : $"{v.Client.FirstName} {v.Client.LastName}",
        AgentId = v.AgentId,
        AgentName = v.Agent is null ? null : $"{v.Agent.FirstName} {v.Agent.LastName}",
        ScheduledAt = v.ScheduledAt,
        Status = v.Status,
        Notes = v.Notes
    };
}
