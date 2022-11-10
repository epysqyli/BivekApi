using Api.Data;
using Api.Interfaces;
using Api.Configuration;

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// database connection
string ConnString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(ConnString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// jwt configuration
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddAuthentication(opts => opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOpts =>
                    {
                        var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
                        jwtOpts.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            RequireExpirationTime = false,
                            ValidateLifetime = true
                        };
                        jwtOpts.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = ctx =>
                            {
                                ctx.Token = ctx.Request.Cookies["token"];
                                return Task.CompletedTask;
                            }
                        };
                    });

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApiDbContext>();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string domain = builder.Configuration.GetValue<string>("Domain");

string CorsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy,
                      policy => policy.WithOrigins($"http://{domain}:3000")
                                      .AllowCredentials()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
});

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseRouting();
app.UseCors(CorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
