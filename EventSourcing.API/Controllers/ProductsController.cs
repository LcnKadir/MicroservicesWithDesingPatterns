using EventSourcing.API.Commands;
using EventSourcing.API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatProductDto creatProductDto)
        {
            await _mediator.Send(new CreateProductCommand() { CreatProductDto = creatProductDto });
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeName(ChangeProductNameDto changeProductNameDto)
        {
            await _mediator.Send(new ChangeProductNameCommand {ChangeProductNameDto = changeProductNameDto });
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangePrice(ChangeProductPriceDto changeProductPriceDto)
        {
            await _mediator.Send(new ChangePorductPriceCommand { ChangeProductPriceDto = changeProductPriceDto});
            return NoContent();
        }
    }
}
