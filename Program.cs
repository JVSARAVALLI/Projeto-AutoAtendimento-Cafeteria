var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços de Controllers com Views (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do ambiente de execução
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Rota padrão ajustada para abrir diretamente a Tela de Descanso do Totem
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AreaPublica}/{action=IndexPublico}/{id?}");

app.Run();