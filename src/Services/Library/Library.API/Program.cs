using Hub.Infrastructure.Data.Interceptors;
using Library.Application;
using Library.Application.Data;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using StackExchange.Redis; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();

var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, options) =>
    {
        var interceptors = sp.GetServices<ISaveChangesInterceptor>();
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        });
        options.AddInterceptors(interceptors);
    },
    contextLifetime: ServiceLifetime.Scoped,
    optionsLifetime: ServiceLifetime.Scoped
);

builder.Services.AddScoped<IApplicationDbContext>(sp =>
    sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
    return new MongoClient(mongoConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("LibraryDb_Read");
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "LibraryApp:";
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(redisConnectionString!);
});

builder.Services.AddScoped<ICacheInvalidationService, DistributedCacheInvalidationService>();


builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<BookReadRepository>();
builder.Services.AddScoped<IBookReadRepository>(sp =>
{
    var mongoRepo = sp.GetRequiredService<BookReadRepository>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    return new CachedBookReadRepository(mongoRepo, cache);
});

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<MemberReadRepository>();
builder.Services.AddScoped<IMemberReadRepository>(sp =>
{
    var mongoRepo = sp.GetRequiredService<MemberReadRepository>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    return new CachedMemberReadRepository(mongoRepo, cache);
});

builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<LoanReadRepository>();
builder.Services.AddScoped<ILoanReadRepository>(sp =>
{
    var mongoRepo = sp.GetRequiredService<LoanReadRepository>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    return new CachedLoanReadRepository(mongoRepo, cache);
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler(options => { });

app.Run();