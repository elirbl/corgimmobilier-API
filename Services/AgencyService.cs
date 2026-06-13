using YmmoApi.Common;
using YmmoApi.Dtos.Agencies;
using YmmoApi.Models;
using YmmoApi.Services.Interfaces;

namespace YmmoApi.Services;

public class AgencyService(IAgencyRepository repository) : IAgencyService
{
    public async Task<PagedResult<AgencyResponseDto>> GetPagedAsync(string? city, int page, int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var (items, totalCount) = await repository.GetPagedAsync(city, page, pageSize);

        var dtos = new List<AgencyResponseDto>();
        foreach (var agency in items)
            dtos.Add(await ToResponseDtoAsync(agency));

        return new PagedResult<AgencyResponseDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<AgencyDetailDto?> GetDetailAsync(int id)
    {
        var agency = await repository.GetByIdAsync(id);
        if (agency is null)
            return null;

        var agents = await repository.GetAgentsAsync(id);
        var propertiesCount = await repository.GetPropertiesCountAsync(id);

        return new AgencyDetailDto
        {
            Id = agency.Id,
            Name = agency.Name,
            City = agency.City,
            Address = agency.Address,
            Phone = agency.Phone,
            Email = agency.Email,
            PropertiesCount = propertiesCount,
            Agents = agents.Select(a => new AgentSummaryDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                Phone = a.Phone
            }).ToList()
        };
    }

    public async Task<AgencyResponseDto> CreateAsync(AgencyCreateDto dto)
    {
        var agency = new Agency
        {
            Name = dto.Name,
            City = dto.City,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email
        };

        await repository.AddAsync(agency);
        await repository.SaveChangesAsync();

        return await ToResponseDtoAsync(agency);
    }

    public async Task<bool> UpdateAsync(int id, AgencyUpdateDto dto)
    {
        var agency = await repository.GetByIdAsync(id);
        if (agency is null)
            return false;

        agency.Name = dto.Name;
        agency.City = dto.City;
        agency.Address = dto.Address;
        agency.Phone = dto.Phone;
        agency.Email = dto.Email;

        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var agency = await repository.GetByIdAsync(id);
        if (agency is null)
            return false;

        repository.Remove(agency);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<ServiceResult> AttachAgentAsync(int agencyId, int agentId)
    {
        var agency = await repository.GetByIdAsync(agencyId);
        if (agency is null)
            return ServiceResult.Failure("Agence introuvable.");

        var agent = await repository.GetUserByIdAsync(agentId);
        if (agent is null)
            return ServiceResult.Failure("Utilisateur introuvable.");

        if (agent.Role != UserRole.Agent)
            return ServiceResult.Failure("Seul un utilisateur avec le rôle Agent peut être rattaché à une agence.");

        agent.AgencyId = agencyId;
        await repository.SaveChangesAsync();
        return ServiceResult.Success();
    }

    private async Task<AgencyResponseDto> ToResponseDtoAsync(Agency agency) => new()
    {
        Id = agency.Id,
        Name = agency.Name,
        City = agency.City,
        Address = agency.Address,
        Phone = agency.Phone,
        Email = agency.Email,
        PropertiesCount = await repository.GetPropertiesCountAsync(agency.Id)
    };
}
