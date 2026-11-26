using Projeto_do_semestre.Dao;
using Projeto_do_semestre.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Suporte aos Controllers com Views (MVC)
builder.Services.AddControllersWithViews();

// 2. Memória temporária e Session para o carrinho
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Registro dos DAOs
// Banco não precisa ser registrado porque seus métodos são estáticos.
builder.Services.AddScoped<ProdutoDao>();
builder.Services.AddScoped<PedidoDao>();

var app = builder.Build();

// 4. Inicialização e teste de conexão com SQLite
try
{
    Banco.CriarBanco();

    using var conexao = Banco.ObterConexao();

    Console.WriteLine(
        $"SUCESSO: Conectado ao SQLite! Arquivo: {conexao.DataSource}"
    );
}
catch (Exception ex)
{
    Console.WriteLine(
        $"Erro ao inicializar/conectar o banco de dados: {ex.Message}"
    );
}

// 5. Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Session antes da autorização
app.UseSession();

app.UseAuthorization();

// 6. Rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Produto}/{action=Cardapio}/{id?}"
);

app.Run();
