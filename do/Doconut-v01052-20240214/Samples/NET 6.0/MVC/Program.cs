using Doconut.Viewer.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// required for Doconut
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Required for Doconut

app.MapWhen(
         context => context.Request.Path.ToString().EndsWith("DocImage.axd"),
         appBranch =>
           {
               appBranch.UseDoconut(new DoconutOptions { UnSafeMode = false, ShowDoconutInfo = true });

           });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
