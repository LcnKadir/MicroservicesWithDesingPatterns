using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Commands
{
    public class ChangePorductPriceCommand: IRequest
    {
        public ChangeProductPriceDto  ChangeProductPriceDto { get; set; }
    }
}
