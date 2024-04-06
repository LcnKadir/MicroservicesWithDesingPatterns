using MediatR;

namespace EventSourcing.API.Commands
{
    public class DeleteProductCommand
    {
        public Guid Id { get; set; }
    }
}
