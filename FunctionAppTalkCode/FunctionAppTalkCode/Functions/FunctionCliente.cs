using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace FunctionAppTalkCode
{
    using Models;

    public static class FunctionCliente
    {
        [FunctionName("cliente")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            MySQLRepository<Cliente>.Initialize("Cliente");

            var message = "Cliente {0} Criado com Sucesso!";
            bool valid = true;

            //// Get request body
            dynamic json = await req.Content.ReadAsAsync<object>();

            string cliente = JsonConvert.SerializeObject(json);

            //Tipagem
            Cliente data = JsonConvert.DeserializeObject<Cliente>(cliente);

            IEnumerable<Cliente> clientes = new List<Cliente>();

            clientes = await MySQLRepository<Cliente>.GetItemsAsync(new string[] { "cpf" }, new string[] { "=" }, new string[] { string.Format("'{0}'", data.CPF) });

            Document document = new Document();

            ////Campos obrigatórios
            valid = data.Nome != null && data.CPF != null && data.DataNascimento != null;

            if (!clientes.Any() && valid)
            {
                Guid key = Guid.NewGuid();

                document = MySQLRepository<Cliente>.CreateItem(key, new Cliente()
                {
                    Id = key.ToString(),
                    Nome = data.Nome,
                    CPF = data.CPF,
                    DataCadastro = DateTime.UtcNow,
                    DataNascimento = data.DataNascimento,
                    Status = data.Status
                });
            }
            else
            {
                message = !valid ? "Verifique os dados que são obrigatórios e tente novamente." : "Já existe um cliente cadastrado com esse cpf.";
                valid = false;
            }

            return !valid
                ? req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = message,
                    ok = false,
                    errors = new string[] { message },
                    success = false
                })
                : req.CreateResponse(HttpStatusCode.OK, new
                {
                    messages = new string[] { string.Format(message, data.Nome) },
                    data = new
                    {
                        id = document.Id,
                        name = data.Nome
                    },
                    ok = true,
                    success = true
                });
        }
    }
}
