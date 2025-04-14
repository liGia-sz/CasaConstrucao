var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(); // ESSENCIAL para servir arquivos da wwwroot

app.MapPost("/contato", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();

    var nome = form["nome"];
    var email = form["email"];
    var mensagem = form["mensagem"];

    // Exibe no terminal
    Console.WriteLine($"[CONTATO] {DateTime.Now}: {nome} ({email}) - {mensagem}");

    // Salva em arquivo
    var texto = $"{DateTime.Now} - Nome: {nome}, Email: {email}, Mensagem: {mensagem}\n";
    await File.AppendAllTextAsync("contatos.txt", texto);

    // Retorno ao usuário
    return Results.Content("<h2>Mensagem enviada com sucesso!</h2><p><a href='/contato.html'>Voltar</a></p>", "text/html");
});

app.Run();
