using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjetoASPNET03MVCIdentityDb.Models
{
    // esta classe proporciona o contexto/ referencia do DB SQL Server, aqui, representado

    //para este propósito será praticado o mecanismo de herança entre as classes: AppDbContext e a classe embarcada/nativa IdentityDbContext <> (<> significa genérica)

    //o objetivo é: oferecer a subclasse todos os recursos necessários para o contexto referência de integração entre as aplicações front-end e back-end 
    public class AppDbContext : IdentityDbContext<AppUser> //a especificidade do elemento genérico IdentityDbContext é dada pela representação/model/entity AppUser
    {
        //definir o construtor desta classe porque é necessário "priorizar" a referencia do contexto, aqui, definido
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
