//uso da diretiva Identity
using Microsoft.AspNetCore.Identity;

namespace ProjetoASPNET03MVCIdentityDb.Models
{
    // esta classe assume o "papel" de representação/entidade/entity da table do Db. Para tal propósito, será praticado o mecanismo de herança com a classe IdentityUser
    public class AppUser : IdentityUser // superclasse/pai/base
    {
        // a table do DB será criada a partir deste model. Portanto, toda e qualquer propriedade que, aqui, for descrita será definida na table do DB como uma coluna que a compõe

        //O que eu preciso colocar aqui? Absolutamante Nada! não será definida, neste model, nenhuma propriedade. Pq? R: em função do mecanismo de herança com a superclasse IdentityUser - algumas propriedades-padrão(default) serão dispoinibilizadas - SEM A NECESSIDADE DE REFERENCIÁ-LAS. Exemplo:

        // Id
        // Username
        // Email
        // HashPassword

    }
}
