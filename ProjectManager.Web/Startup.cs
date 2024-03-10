using AutoMapper;
using ProjectManager.Business;
using ProjectManager.Business.Interfaces;
using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Db;
using ProjectManager.Db.Context;
using ProjectManager.Db.Repositories;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Interfaces.Repositories;
using ProjectManager.Web.Models.Authenticacao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace ProjectManager.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthentication(services);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); options.EnableForHttps = true; });

            services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Error);
            services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("pt-BR")
                };

                opts.DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });

            var connectionString = Configuration.GetConnectionString("ConnectionString");
            if (string.IsNullOrEmpty(connectionString))
                connectionString = Configuration.GetValue<string>("ConnectionString");

            MigrationRunner.Up(connectionString);

            services.AddEntityFrameworkNpgsql().AddDbContext<DbProjectManagerContext>(options => { options.EnableSensitiveDataLogging(); options.UseNpgsql(connectionString); });//, ServiceLifetime.Scoped


            services.AddScoped<IUnitOfWork, UoW>();
            services.AddScoped<IMapper, Mapper>();

            ConfigureRepositoriesClasses(services);
            ConfigureBusinessClasses(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Project Manager API",
                        Version = "v1",
                        Description = "System Project Manager",
                    });
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
            });

            var testaEstrutura = services.BuildServiceProvider().GetService<DbProjectManagerContext>();
            testaEstrutura.TestarTodasTabelas();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
        }

        private static void ConfigureRepositoriesClasses(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(_RepositoryBase<>));


            services.AddScoped<ISimNaoRepository, SimNaoRepository>();
            services.AddScoped<IProjetoRepository, ProjetoRepository>();
            services.AddScoped<IProjetoResponsavelRepository, ProjetoResponsavelRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IResponsavelRepository, ResponsavelRepository>();
        }

        private void ConfigureBusinessClasses(IServiceCollection services)
        {
            services.AddScoped(typeof(IBusinessBase<>), typeof(_BusinessBase<>));
            services.AddScoped<ISimNaoBusiness, SimNaoBusiness>();
            services.AddScoped<IProjetoBusiness, ProjetoBusiness>();
            services.AddScoped<IProjetoResponsavelBusiness, ProjetoResponsavelBusiness>();
            services.AddScoped<IUsuarioBusiness, UsuarioBusiness>();
            services.AddScoped<IResponsavelBusiness, ResponsavelBusiness>();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var tokenConfigurations = new TokenConfigurations();

            IConfigurationSection conf = Configuration.GetSection("TokenConfigurations");
            if (conf != null && string.IsNullOrEmpty(conf.GetValue<string>("SymmetricSecurityKey")))
            {
                var token = new
                {
                    TokenConfigurations = new
                    {
                        SymmetricSecurityKey = Configuration.GetValue<string>("SymmetricSecurityKey"),
                        TokenLifetimeInMinutes = Configuration.GetValue<int>("TokenLifetimeInMinutes"),
                        Audience = Configuration.GetValue<string>("Audience"),
                        Issuer = Configuration.GetValue<string>("Issuer")
                    }
                };
                string jsonString = JsonSerializer.Serialize(token);
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(jsonString);

                using (MemoryStream stream = new MemoryStream(utf8Bytes))
                {
                    IConfiguration confs = new ConfigurationBuilder().AddJsonStream(stream).Build();
                    conf = confs.GetSection("TokenConfigurations");
                }

            }
            new ConfigureFromConfigurationOptions<TokenConfigurations>(conf).Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.SymmetricSecurityKey));

                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
                bearerOptions.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = OnAuthenticationFailed,
                    OnTokenValidated = OnTokenValidated
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireClaim("conta")
                    .Build();
            });
        }

        public async Task OnTokenValidated(TokenValidatedContext arg)
        {
            await Task.Run(() => Debug.Write(arg));
        }

        //
        static async Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            await Task.Run(() => Debug.Write(context));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(corsPolicyBuilder =>
              corsPolicyBuilder
                 .WithOrigins("*")
                 .WithOrigins("http://localhost:4200")
                 .WithOrigins("http://localhost:4201")
                 .AllowAnyMethod().AllowAnyHeader()
            );

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && !context.Request.Path.Value.StartsWith("/api"))
                {
                    context.Request.Path = "/index.html";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Project Manager API");
            });


            app.UseMvc();
        }
    }
}
