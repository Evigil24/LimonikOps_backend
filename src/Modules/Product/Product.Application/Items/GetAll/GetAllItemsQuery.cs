using LimonikOne.Modules.Product.Application.Items;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.GetAll;

public sealed record GetAllItemsQuery : IQuery<IReadOnlyList<ItemDto>>;
