using LimonikOne.Modules.Reception.Domain.Receptions;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Reception.Application.Receptions.Get;

internal sealed class GetReceptionByIdHandler : IQueryHandler<GetReceptionByIdQuery, ReceptionDto>
{
    private readonly IReceptionRepository _receptionRepository;

    public GetReceptionByIdHandler(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public async Task<Result<ReceptionDto>> HandleAsync(GetReceptionByIdQuery query, CancellationToken cancellationToken = default)
    {
        var reception = await _receptionRepository.GetByIdAsync(
            ReceptionId.From(query.Id), cancellationToken);

        if (reception is null)
        {
            return Result.Failure<ReceptionDto>(
                Error.NotFound("Reception.NotFound", $"Reception with ID '{query.Id}' was not found."));
        }

        var dto = new ReceptionDto(
            reception.Id.Value,
            reception.DisplayId,
            reception.GuestName.FirstName,
            reception.GuestName.LastName,
            reception.Status.ToString(),
            reception.Notes,
            reception.CreatedAt,
            reception.CheckedInAt);

        return Result.Success(dto);
    }
}
