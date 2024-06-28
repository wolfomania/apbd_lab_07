using HomeAssignment.Models.DTO;
using Microsoft.Data.SqlClient;

namespace HomeAssignment.Repositories;

public class WarehouseRepository : IWarehouseRepository
{

    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesWarehouseExist(int id)
    {
        var query = "SELECT 1 FROM Warehouse WHERE IdWarehouse = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<bool> DoesProductExist(int id)
    {
        var query = "SELECT 1 FROM Product WHERE IdProduct = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<int> GetOrderIdWithProduct(int productId, int amount, DateTime dateTime)
    {
        var query = "SELECT IdOrder " +
                    "FROM 'Order' " +
                    "WHERE IdProduct = @ID AND Amount = @Amount AND CreatedAt < @CreatedAt";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", productId);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@CreatedAt", dateTime);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();
        
        if (!reader.HasRows)
        {
            return -1;
        }

        return reader.GetInt32(reader.GetOrdinal("IdOrder"));
    }

    public async Task<bool> IsOrderCompleted(int orderId)
    {
        var query = "SELECT 1 " +
                    "FROM Product_Warehouse " +
                    "WHERE IdOrder = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", orderId);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<int> UpdateFulfilledAtOrder(int id)
    {
        var query = "UPDATE 'Order' " +
                    "SET FulfilledAt = @FulfilledAt " +
                    "WHERE IdOrder = @ID ";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        command.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);

        await connection.OpenAsync();

        var res = await command.ExecuteNonQueryAsync();

        return res;
    }

    public async Task<double> GetProductPrice(int productId)
    {
        var query = "SELECT Price " +
                    "FROM Product " +
                    "WHERE IdProduct = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", productId);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        return reader.GetInt32(reader.GetOrdinal("Price"));
    }

    public async Task<int> AddNewProductWarehouse(FulfillOrderDto fulfillOrderDto, int orderId, double price)
    {

        var query = "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                    "VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt) " +
                    "SELECT SCOPE_IDENTITY()";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdWarehouse", fulfillOrderDto.IdWarehouse);
        command.Parameters.AddWithValue("@IdProduct", fulfillOrderDto.IdProduct);
        command.Parameters.AddWithValue("@IdOrder", orderId);
        command.Parameters.AddWithValue("@Amount", fulfillOrderDto.Amount);
        command.Parameters.AddWithValue("@Price", fulfillOrderDto.Amount * price);
        command.Parameters.AddWithValue("@CreatedAt", fulfillOrderDto.CreatedAt);
        

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        return reader.GetInt32(reader.GetOrdinal("IdProductWarehouse"));
    }

    public async Task<int> AddNewProductWarehouseProcedure(FulfillOrderDto fulfillOrderDto)
    {
        var query = "EXEC AddNewProductWarehouse @IdWarehouse, @IdProduct, @Amount, @CreatedAt";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdWarehouse", fulfillOrderDto.IdWarehouse);
        command.Parameters.AddWithValue("@IdProduct", fulfillOrderDto.IdProduct);
        command.Parameters.AddWithValue("@Amount", fulfillOrderDto.Amount);
        command.Parameters.AddWithValue("@CreatedAt", fulfillOrderDto.CreatedAt);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        return reader.GetInt32(reader.GetOrdinal("NewId"));
    }
}