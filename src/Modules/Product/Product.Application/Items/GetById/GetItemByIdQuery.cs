using LimonikOne.Modules.Product.Application.Items;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.GetById;

public sealed record GetItemByIdQuery(Guid Id) : IQuery<ItemDto>;
