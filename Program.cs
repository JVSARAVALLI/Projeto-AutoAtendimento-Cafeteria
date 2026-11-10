using Projeto_do_semestre.Dao;
using Projeto_do_semestre.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Adiciona suporte aos Controllers com Views (MVC)
builder.Services.AddControllersWithViews();

// 2. Configura a memória temporária e a Session (necessária para o carrinho de compras do totem)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Regista as Injeções de Dependência (Banco de Dados e DAOs)
builder.Services.AddSingleton<Banco>();
builder.Services.AddScoped<ProdutoDao>();
builder.Services.AddScoped<PedidoDao>();

var app = builder.Build();

// 4. Garante que a base de dados SQLite é inicializada ao arrancar a aplicação
using (var scope = app.Services.CreateScope())
{
    try
    {
        var banco = scope.ServiceProvider.GetRequiredService<Banco>();
        Banco.CriarBanco();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao inicializar o banco de dados: {ex.Message}");
    }
}

// 5. Configurações de Middleware do pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Permite carregar ficheiros CSS/JS da pasta wwwroot

app.UseRouting();

// O UseSession OBRIGATORIAMENTE tem de vir antes do UseAuthorization
app.UseSession();
app.UseAuthorization();

// 6. Configuração das rotas de navegação
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Produto}/{action=Cardapio}/{id?}");

app.Run();