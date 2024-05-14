using GrpcBlazor.Components;
using GrpcProtos.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add gRPC services.
builder.Services.AddGrpc();
builder.Services.AddSingleton(sp => new GrpcClient("https://localhost:7119"));
builder.Services.AddTransient<ReverseClient>();
//builder.Services.AddScoped(sp => new ReverseClient("https://localhost:7119"));
//builder.Services.AddTransient(sp => new ReverseClient("https://localhost:7119"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();



app.Run();
