using HomeAssignment.Models.DTO;
using HomeAssignment.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace HomeAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController(IWarehouseRepository warehouseRepository) : ControllerBase
    {
        [HttpPost("manual")]
        public async Task<IActionResult> FulfillOrder([FromBody] FulfillOrderDto fulfillOrderDto)
        {
            if (!await warehouseRepository.DoesProductExist(fulfillOrderDto.IdProduct))
            {
                return NotFound("Product does not exist.");
            }

            if (!await warehouseRepository.DoesWarehouseExist(fulfillOrderDto.IdWarehouse))
            {
                return NotFound("Warehouse does not exist.");
            }

            if (fulfillOrderDto.Amount <= 0)
            {
                return BadRequest("Amount should be greater than 0.");
            }
            
            var orderId = await warehouseRepository.GetOrderIdWithProduct(fulfillOrderDto.IdProduct, fulfillOrderDto.Amount, fulfillOrderDto.CreatedAt);

            if (orderId == -1 
                || await warehouseRepository.IsOrderCompleted(orderId))
            {
                return NotFound("Order does not exist or has already been completed.");
            }

            await warehouseRepository.UpdateFulfilledAtOrder(orderId);

            var price = await warehouseRepository.GetProductPrice(fulfillOrderDto.IdProduct);

            var id = await warehouseRepository.AddNewProductWarehouse(fulfillOrderDto, orderId, price);

            return Ok(id);
        }
        
        [HttpPost("procedure")]
        public async Task<IActionResult> FulfillOrderProcedure([FromBody] FulfillOrderDto fulfillOrderDto)
        {
            if (fulfillOrderDto.Amount <= 0)
            {
                return BadRequest("Amount should be greater than 0.");
            }
            
            try
            {
                var id = await warehouseRepository.AddNewProductWarehouseProcedure(fulfillOrderDto);
                return Ok(id);

            } catch (SqlException e)
            {
                return NotFound(e.Message);
            }

        }
    }
}
