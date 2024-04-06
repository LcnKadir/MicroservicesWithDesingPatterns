using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Commands
{
    public class ChangeProductNameCommand: IRequest
    {
        public ChangeProductNameDto  ChangeProductNameDto { get; set; }
    }
}
