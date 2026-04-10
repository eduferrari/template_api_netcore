using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TemplateApi.Application;
using Serilog;
using TemplateApi.API.Middleware;
using TemplateApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TemplateApi API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Informe: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();