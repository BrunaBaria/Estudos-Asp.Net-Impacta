using Microsoft.EntityFrameworkCore;
using Projeto04.AspNet.WebAPI.BackEnd.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// estes services são especificos de uma aplicação WebAPI
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

// ADICIONAR O SERVICE PARA INCIALIZAÇÃO DO CORS - TE COMO OBJETIVO "HABILITAR" O CROSS-DOMAIN (CRUZAMENTO DE DOMINIO ENTRE APLICAÇÕES) 
// adicionar as politicas de aceitação de qualquer slicitação de aplicações client/front - a partir de qualquer outro dominio/ambiente 
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

// aqui, abaixo, a aplicação fará uso das politicas de integração definidas acima
app.UseCors("Cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
