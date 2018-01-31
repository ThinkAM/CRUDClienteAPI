using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;

namespace FunctionAppTalkCode.Models
{
    using Enums;

    public class Cliente : IsoDateTimeConverter
    {
        public Cliente()
        {
            base.DateTimeFormat = "dd-MM-yyyy hh:mm:ss";
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nome")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "cpf")]
        public string CPF { get; set; }

        [JsonProperty(PropertyName = "dataCadastro")]
        public DateTime DataCadastro { get; set; }

        [JsonProperty(PropertyName = "dataNascimento")]
        public DateTime DataNascimento { get; set; }

        [JsonProperty(PropertyName = "status")]
        public EStatus Status { get; set; }
    }
}
