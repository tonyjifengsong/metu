var builder = WebApplication.CreateBuilder(args);
//Data Source=SK-20220106ENJR;Initial Catalog=METUCORE;Integrated Security=True
// Add services to the container.
//builder.Services.AddDbContext<METUCOREContext>();
// Add services to the container.
//Scaffold-DbContext "Data Source=SK-20220106ENJR;Initial Catalog=METUCORE;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer  -OutputDir Models -Force
//builder.Services.AddElastic();
builder.Services.AddControllers();
//builder.Services.AddBasicHost();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

//app.UseRouting();
//app.UseCors("cors");
app.UseAuthorization();

app.MapControllers();

app.Run();
 