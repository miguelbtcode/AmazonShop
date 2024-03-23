using Newtonsoft.Json;

namespace Ecommerce.Api.Errors;

public class CodeErrorResponse 
{
    [JsonProperty(PropertyName = "statusCode")]
    public int StatusCode { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string[]? Message { get; set; }
    
    public CodeErrorResponse(int statusCode, string[]? message = null)
    {
        StatusCode = statusCode;

        if (message is null)
        {
            Message = new string[0];
            var text = GetDefaultMessageStatusCode(statusCode);
            Message[0] = text;
        }
        else
        {
            Message = message;
        }
    }

    private static string GetDefaultMessageStatusCode(int statusCode)
    {
        return statusCode switch 
        {
            400 => "El request enviado tiene errores",
            401 => "No tienes authorization para este recurso",
            404 => "No se encontró el recurso solicitado",
            500 => "Se produjeron errores en el servidor",
            _ => string.Empty
        };
    }
}