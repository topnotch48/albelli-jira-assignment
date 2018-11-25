using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Albelli.Core.Contracts;
using Albelli.Orders.Contracts.Models;
using Albelli.Orders.Services;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable 1570

namespace Albelli.Orders.WebApi.Controllers
{
    [Route("api/orders")]
    public class OrdersController : Controller
    {
	    private readonly IOrdersManagementService _ordersManagementService;

	    public OrdersController(IOrdersManagementService ordersManagementService)
	    {
		    this._ordersManagementService =
			    ordersManagementService ?? throw new ArgumentNullException(nameof(ordersManagementService));
	    }

        /// <summary>
        /// Retrieves a paginated list of available orders
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/orders?skip=5&take=10
        ///
        /// </remarks>
        /// <param name="paging">Pagination options model</param>
        /// <returns>A paginated list of orders</returns>
        /// <response code="200">Paginated list of orders retrieved</response>
        [HttpGet]
	    [ProducesResponseType(typeof(PagingResult<Order>), 200)]
	    public async Task<IActionResult> GetOrders([FromQuery] Paging paging)
	    {
		    var result = await this._ordersManagementService.Get(paging);
		    return Ok(result);
	    }


		/// <summary>
		/// Retrieves a particular order by order id
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET api/orders/SAL1005234
		///
		/// </remarks>
		/// <param name="query">Query containing order id</param>
		/// <returns>Order with specified id</returns>
		/// <response code="200">Order has been successfully retrieved</response>
		/// <response code="204">Requested order not found</response>
		/// <response code="400">Dictionary containing OrderIdQuery model validation errors</response>
		[HttpGet("{OrderId}")]
	    [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(Order), 204)]
		[ProducesResponseType(typeof(Dictionary<string, object>), 400)]
        public async Task<IActionResult> GetOrder([FromRoute] OrderIdQuery query)
	    {
	        if (!ModelState.IsValid)
	        {
	            return BadRequest(ModelState);
	        }

            var orderId = new OrderId(query.OrderId);

		    var result = await this._ordersManagementService.Get(orderId);

		    if (result == null)
			    return NoContent();

		    return Ok(result);
        }

	    /// <summary>
	    /// Creates an order
	    /// </summary>
	    /// <remarks>
	    /// Sample request:
	    ///
	    ///     POST api/orders
	    ///     {
        ///            "OrderId": "SAL1005234",
        ///            "CustomerName": "Jeroen Smeets",
        ///            "Price": "20.95",
        ///            "PostCode": "1363AR",
        ///            "HouseNumber": "11"
        ///     }
        ///
        /// </remarks>
        /// <param name="orderNew">New order to place in the system</param>
        /// <returns>A newly-created order</returns>
        /// <response code="201">Order has been successfully created</response>
        /// <response code="400">Dictionary containing order model validation errors</response>
        [HttpPost]
	    [ProducesResponseType(typeof(Order), 201)]
	    [ProducesResponseType(typeof(Dictionary<string, object>), 400)]
	    public async Task<IActionResult> PlaceOrder([FromBody] OrderNew orderNew)
	    {
		    if (!ModelState.IsValid)
		    {
			    return BadRequest(ModelState);
		    }

		    var createdOrder = await this._ordersManagementService.Create(orderNew);

		    var url = Url.Action(nameof(GetOrder), new OrderIdQuery { OrderId = createdOrder.OrderId });

		    return Created(url, createdOrder);
	    }

		/// <summary>
		/// Deletes an order with specified order id
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     DELETE api/orders/SAL1005234
		///
		/// </remarks>
		/// <param name="query">Query containing order id</param>
		/// <response code="200">Order has been successfully deleted</response>
		/// <response code="400">Dictionary containing order model validation errors</response>
		[HttpDelete("{OrderId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(Dictionary<string, object>), 400)]
		public async Task<IActionResult> DeleteOrder([FromRoute] OrderIdQuery query)
        {
	        if (!ModelState.IsValid)
	        {
		        return BadRequest(ModelState);
	        }

			var orderId = new OrderId(query.OrderId);

	        await this._ordersManagementService.Delete(orderId);

	        return Ok();
        }

		/// <summary>
		/// Updates an order with specified order id
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     PUT api/orders/SAL1005234
		///     {
		///            "RowVersion": "0833a589-ae23-477f-a5b6-6669d3f8015b",
		///            "CustomerName": "Jeroen Boenen",
		///            "Price": "21.95",
		///            "PostCode": "1363AR",
		///            "HouseNumber": "11"
		///     }
		/// 
		/// </remarks>
		/// <param name="query">Query containing order id</param>
		/// <param name="orderUpdate">Model containing order fields to update</param>
		/// <returns>Updated order model</returns>
		/// <response code="200">Order has been successfully updated</response>
		/// <response code="400">Dictionary containing order model validation errors</response>
		[HttpPut("{OrderId}")]
	    [ProducesResponseType(typeof(Order), 200)]
	    [ProducesResponseType(typeof(Dictionary<string, object>), 400)]
		public async Task<IActionResult> UpdateOrder([FromRoute] OrderIdQuery query, [FromBody] OrderUpdate orderUpdate)
	    {
		    if (!ModelState.IsValid)
		    {
			    return BadRequest(ModelState);
		    }

		    var orderId = new OrderId(query.OrderId);

		    var updatedOrder = await this._ordersManagementService.Update(orderId, orderUpdate);

		    return Ok(updatedOrder);
		}
	}
}
