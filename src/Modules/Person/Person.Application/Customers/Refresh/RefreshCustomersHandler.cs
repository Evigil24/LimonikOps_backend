using LimonikOne.Modules.Person.Domain.Customers;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Dynamics;

namespace LimonikOne.Modules.Person.Application.Customers.Refresh;

internal sealed class RefreshCustomersHandler(
    IDynamicsHttpClient dynamicsClient,
    ICustomerRepository customerRepository,
    IPersonUnitOfWork unitOfWork
) : ICommandHandler<RefreshCustomersCommand>
{
    private const string EntitySet = "CustomersV3";
    private const string SelectFields =
        "CustomerAccount,ItemCustomerGroupId,PartyNumber,NameAlias,RFCNumber,OrganizationName";

    private readonly IDynamicsHttpClient _dynamicsClient = dynamicsClient;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IPersonUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(
        RefreshCustomersCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var dynamicsCustomers = await _dynamicsClient.GetAsync<DynamicsCustomerDto>(
            EntitySet,
            select: SelectFields,
            cancellationToken: cancellationToken
        );

        var existingCustomers = await _customerRepository.GetAllAsync(cancellationToken);
        var existingByAccountNumber = existingCustomers.ToDictionary(c => c.AccountNumber);

        var newCustomers = new List<CustomerEntity>();

        foreach (var dc in dynamicsCustomers)
        {
            if (existingByAccountNumber.TryGetValue(dc.CustomerAccount, out var existing))
            {
                existing.UpdateFromDynamics(
                    dc.ItemCustomerGroupId,
                    dc.OrganizationName,
                    dc.NameAlias,
                    dc.PartyNumber,
                    dc.RFCNumber
                );
            }
            else
            {
                newCustomers.Add(
                    CustomerEntity.CreateFromDynamics(
                        dc.CustomerAccount,
                        dc.ItemCustomerGroupId,
                        dc.OrganizationName,
                        dc.NameAlias,
                        dc.PartyNumber,
                        dc.RFCNumber
                    )
                );
            }
        }

        if (newCustomers.Count > 0)
        {
            await _customerRepository.AddRangeAsync(newCustomers, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
