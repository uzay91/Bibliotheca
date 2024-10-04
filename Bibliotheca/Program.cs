
using Core.Extensions;
using Core.Utilities.IoC;
using Core.DependencyResolvers;
using Business.Concrete;
using Business.Abstract;
using Bibliotheca;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Business.DependencyResolvers.Autofac;
using DataAccess.Concrete.EntityFramework.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var localSeq = "http://host.docker.internal:5341/";

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .WriteTo.Seq(localSeq)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));

//builder.Services.AddDbContext<PostgreSqlDbContext>();

var app = builder.Build();

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

startup.Configure(app, app.Environment);

await app.RunAsync();