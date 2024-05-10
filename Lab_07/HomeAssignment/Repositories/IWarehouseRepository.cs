using HomeAssignment.Models.DTO;
using Microsoft.CodeAnalysis.Elfie.Model;

namespace HomeAssignment.Repositories;

public interface IWarehouseRepository
{
    Task<bool> DoesWarehouseExist(int id);
    Task<bool> DoesProductExist(int id);
    Task<bool> DoesOrderWithProductExist(int id, int amount, DateTime dateTime);
    Task<bool> IsOrderCompleted(int id);
    Task<int> UpdateFulfilledAtOrder(int id);
    Task<int> AddNewProductWarehouse(FulfillOrderDto fulfillOrderDto, int orderId);
    Task<double> GetProductPrice(int id);


}