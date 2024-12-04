using Kyrsova;
using Kyrsova.Contractors;
using Kyrsova.Messages;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Store.LiqPay;
using Store.Memory;
using Store.Web.App;
using Store.Web.Contractors;

namespace Store.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHttpContextAccessor(); // Додаємо доступ до контексту HTTP
            services.AddDistributedMemoryCache(); // Додаємо кешування для сесій

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20); // Тривалість сесії 20 хвилин
                options.Cookie.HttpOnly = true; // Обмеження доступу до cookie тільки через HTTP
                options.Cookie.IsEssential = true; // Зробити cookie важливим для роботи сайту
            });

            // Реєстрація залежностей
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IDeliveryService, DeliveryService>();
            services.AddSingleton<IPaymentService, CashPlaymentService>();
            services.AddSingleton<IPaymentService, LiqPayPaymentService>();
            services.AddSingleton<IWebContractorService, LiqPayPaymentService>();
            services.AddSingleton<BookService>();
            services.AddSingleton<OrderService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Додано перед авторизацією, щоб забезпечити роботу сесій
            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

