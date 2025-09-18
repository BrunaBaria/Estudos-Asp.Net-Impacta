using Microsoft.EntityFrameworkCore;
using Projeto04.AspNet.WebAPI.BackEnd.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// estes services s�o especificos de uma aplica��o WebAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REFERENCIAR O DBCONTEXT CONFIGURADO NA PASTA MODELS PARA INICIALIZAR O SERVICE QUE INTEGRA A API E O DB
builder.Services.AddDbContext<MeuDbContext>(
        options => {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            
        });

// **** IMPORTANTE INDICAR ESTE SERVICE PARA LIDAR COM TODOS OS JOINS DA API
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// ADICIONAR O SERVICE PARA INCIALIZA��O DO CORS - TE COMO OBJETIVO "HABILITAR" O CROSS-DOMAIN (CRUZAMENTO DE DOMINIO ENTRE APLICA��ES) 
// adicionar as politicas de aceita��o de qualquer slicita��o de aplica��es client/front - a partir de qualquer outro dominio/ambiente 
builder.Services.AddCors(
        options =>
        {
            options.AddPolicy("Cors", p =>
            {
                p.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });
        });




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// aqui, abaixo, a aplica��o far� uso das politicas de integra��o definidas acima
app.UseCors("Cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
