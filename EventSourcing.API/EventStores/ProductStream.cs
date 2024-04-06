using EventSourcing.API.DTOs;
using EventSourcing.Shared.Events;
using EventStore.ClientAPI;

namespace EventSourcing.API.EventStores
{
    public class ProductStream : AbstrackStream
    {
        public static string StreamName => "ProductStream";

        //public static string GroupName => "agroup";
        public static string GroupName => "replay";

        public ProductStream(IEventStoreConnection eventStoreConnection) : base(StreamName, eventStoreConnection)
        {
        }

        public void Created(CreatProductDto creatProductDto)
        {
            Events.AddLast(new ProductCreatedEvent { Id = Guid.NewGuid(), Name = creatProductDto.Name, Price = creatProductDto.Price, Stock = creatProductDto.Stock, UserId = creatProductDto.UserId });
        }

        public void NameChanged(ChangeProductNameDto changeProductNameDto) 
        {
            Events.AddLast(new ProductNameChangeEvent { ChangeName = changeProductNameDto.Name, Id = changeProductNameDto.Id });
        
        }

        public void PriceChanged(ChangeProductPriceDto changeProductPriceDto)
        {
            Events.AddLast(new ProductPriceChangeEvent() { ChangedPrice = changeProductPriceDto.Price, Id = changeProductPriceDto.Id });
        }

        public void Delete(Guid id)
        {
            Events.AddLast(new ProductDeleteEvent { Id = id });
        }
    }
}
