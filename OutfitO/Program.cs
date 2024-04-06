using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutfitO.Models;
using OutfitO.Repository;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(Options =>
{
    Options.IdleTimeout = TimeSpan.FromMinutes(35);
});

builder.Services.AddDbContext<OutfitoContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db"));
});

builder.Services.AddIdentity<User, IdentityRole>(
                    options =>
                    {
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireDigit = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequiredLength = 8;
                    }).AddEntityFrameworkStores<OutfitoContext>();

#region to redirect the visitor to login page 
// Configure authentication to use cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    // Set the login path
    options.LoginPath = "/User/Login";
});
builder.Services.AddAuthorization();
#endregion

builder.Services.AddScoped<ICartRepository,CartRepository>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPromoCodeRepository, PromoCodeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseSession();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
