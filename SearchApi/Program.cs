
using ProviderOneLib;
using ProviderTwoLib;
using Quartz;
using SearchApi.Cache;
using SearchApi.Filters;
using SearchApi.Services;

namespace SearchApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                options =>
                {
                    options.SchemaFilter<SearchRequestFilter>();
                });

            builder.Services
                .AddQuartz(q =>
                    {
                        q.AddJob<SearchResultCacheCareJob>(opts => opts.WithIdentity(nameof(SearchResultCacheCareJob)));

                        q.AddTrigger(opts => opts
                            .ForJob(nameof(SearchResultCacheCareJob))
                            .WithIdentity($"{nameof(SearchResultCacheCareJob)}-trigger")
                            .WithSimpleSchedule(o =>
                            {
                                o.RepeatForever()
                                    .WithInterval(TimeSpan.FromMinutes(1));
                            }));
                    })
                .AddQuartzHostedService(c => c.AwaitApplicationStarted = true);

            builder.Services
                .AddProviderOne("http://localhost:5109")
                .AddProviderTwo("http://localhost:5169")
                .AddSingleton<SearchResultsCache>()
                .AddTransient<AggregatedSearchProvider>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
