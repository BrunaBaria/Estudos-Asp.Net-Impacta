using Projeto04.Front.APIConsumidor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// adicionar o service de HttpClient() - recurso que auxiliar� na constru��o das requisi��es http - com origem no front
builder.Services.AddHttpClient();

// adicionar as classes de servi�o para acessar as APIs e "consumir" os resultados das opera��es de dados
builder.Services.AddScoped<EstudanteService>();
builder.Services.AddScoped<CursosService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
