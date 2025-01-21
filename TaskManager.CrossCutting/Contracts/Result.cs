using Swashbuckle.AspNetCore.Annotations;

namespace TaskManager.CrossCutting.Contracts
{
    [SwaggerSchema(Description = "Representa o resultado de uma operação.")]
    public class Result
    {
        [SwaggerSchema(Description = "Indica se a operação foi bem-sucedida.")]
        public bool Success { get; set; }
        [SwaggerSchema(Description = "Mensagem associada ao resultado da operação, se houver.")]
        public string? Message { get; set; }
        [SwaggerSchema(Description = "Objeto que contém a resposta da operação, se houver.")]
        public object? Response { get; set; }

        public Result(bool success = true, string? message = null, object? response = null)
        {
            Success = success;
            Message = message;
            Response = response;
        }
    }
}
