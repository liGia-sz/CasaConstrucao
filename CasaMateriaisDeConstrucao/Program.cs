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

app.MapPost("/carreira", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var nome = form["nome"];
    var email = form["email"];
    var arquivo = form.Files["curriculo"];

    var filePath = Path.Combine("curriculos", $"{DateTime.Now:yyyyMMddHHmmss}_{arquivo.FileName}");

    Directory.CreateDirectory("curriculos");

    using var stream = new FileStream(filePath, FileMode.Create);
    await arquivo.CopyToAsync(stream);

    Console.WriteLine($"[CARREIRA] {nome} ({email}) enviou o currículo: {arquivo.FileName}");

    return Results.Content("<script>alert('Currículo enviado com sucesso!'); window.history.back();</script>", "text/html");
});

app.Run();

