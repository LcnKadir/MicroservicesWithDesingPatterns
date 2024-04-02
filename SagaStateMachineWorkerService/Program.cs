using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachineWorkerService;
using SagaStateMachineWorkerService.Models;
using Shared;
using System.Reflection;

namespace SagaStateMachineWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(cfg =>
                    {


                        cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(opt =>
                        {
                            opt.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
                            {
                                builder.UseSqlServer(hostContext.Configuration.GetConnectionString("SqlCon"), m =>
                                {
                                    m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                });
                            });
                        });


                        cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configure =>
                        {
                            configure.Host(hostContext.Configuration.GetConnectionString("RabbitMQ"));

                            configure.ReceiveEndpoint(RabbitMQSettingsConst.OrderSaga, e =>
                            {
                                e.ConfigureSaga<OrderStateInstance>(provider);//OrderSaga queue will be listened to and object instance will be created for each message coming to the queue and written as a row in the database. //OrderSaga kuyruðu dinlenerek, kuyruða gelen her mesajda nesne örneði oluþacak ve veri tabanýnda satýr olarak yazýlacak.
                            });

                        }));

                    });

                    services.AddHostedService<Worker>();
                });
    }
}
