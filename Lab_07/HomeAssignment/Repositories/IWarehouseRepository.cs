using Microsoft.CodeAnalysis.Elfie.Model;

namespace HomeAssignment.Repositories;

public interface IWarehouseRepository
{
    Task<bool> DoesWarehouseExist(int id);
    Task<bool> DoesProductExist(int id);
    Task<bool> DoesOrderWithProductExist(int id, int amount, DateTime dateTime);
    Task<bool> IsOrderCompleted(int id);
    

}