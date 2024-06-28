using HomeAssignment.Models.DTO;
using Microsoft.CodeAnalysis.Elfie.Model;

namespace HomeAssignment.Repositories;

public interface IWarehouseRepository
{
    Task<bool> DoesWarehouseExist(int id);
    Task<bool> DoesProductExist(int id);
    Task<int> GetOrderIdWithProduct(int productId, int amount, DateTime dateTime);
    Task<bool> IsOrderCompleted(int orderId);
    Task<int> UpdateFulfilledAtOrder(int id);
    Task<int> AddNewProductWarehouse(FulfillOrderDto fulfillOrderDto, int orderId, double price);
    Task<double> GetProductPrice(int productId);
    Task<int> AddNewProductWarehouseProcedure(FulfillOrderDto fulfillOrderDto);

}