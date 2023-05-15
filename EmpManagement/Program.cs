using EmpManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext> ( options =>
{
    options.UseSqlServer ( builder.Configuration.GetConnectionString ( "DefaultConnection" ) );
} );

builder.Services.AddIdentity<ApplicationUser ,  IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext> ();

builder.Services.Configure<IdentityOptions> ( options => {
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;
} );

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc(builder =>
{
    var policy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                        .Build();
    builder.Filters.Add(new AuthorizeFilter(policy));
} );
builder.Services.AddScoped<IEmployeeRepository , SQLEmployeeRepository> ( );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication ( );

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
