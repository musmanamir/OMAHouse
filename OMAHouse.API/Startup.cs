using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OMAHouse.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// specifies if hangfire job run or not 
        /// </summary>
        //public bool HangFireEnabled => Configuration.GetSection("AppSetting").GetValue<bool>("RunBackgroundJobs");

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration["Jwt:Issuer"],
            //        ValidAudience = Configuration["Jwt:Issuer"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //    };
            //});

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      builder =>
            //                      {
            //                          builder.WithOrigins("http://localhost:3000",
            //                            "https://backtowork.firebaseapp.com",
            //                            "https://backtoworkdev.firebaseapp.com",
            //                            "https://b2wapp.aciano.net",
            //                            "https://b2w.aciano.net",
            //                            "https://back2work.mazikglobal.com",
            //                            "https://back2workapi.mazikglobal.com");
            //                      });
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Version = "v1",
            //        Title = "Back 2 Work API",
            //        Description = "Back 2 Work ASP.NET Core Web API",
            //        //TermsOfService = new Uri("https://example.com/terms"),
            //        Contact = new OpenApiContact
            //        {
            //            Name = "Usman Amir",
            //            Email = "usman.amir@aciano.net",
            //            //Url = new Uri("https://twitter.com/spboyer"),
            //        },
            //        License = new OpenApiLicense
            //        {
            //            Name = "Aciano Technologies",
            //            Url = new Uri("http://aciano.net/"),
            //        }
            //    });
            //    c.AddSecurityDefinition("Bearer", //Name the security scheme
            //        new OpenApiSecurityScheme
            //        {
            //            Description = "JWT Authorization header using the Bearer scheme.",
            //            Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
            //            Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
            //        });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            //        {
            //            new OpenApiSecurityScheme{
            //                Reference = new OpenApiReference{
            //                    Id = "Bearer", //The name of the previously defined security scheme.
            //                    Type = ReferenceType.SecurityScheme
            //                }
            //            }, new List<string>()
            //        }
            //    });
            //});

            //if (HangFireEnabled)
            //    ConfigureHangfire(services);

            services.AddControllers();
        }

        ///// <summary>
        ///// configures Hangfire functionality (async jobs)
        ///// </summary>
        ///// <param name="services"></param>
        //public void ConfigureHangfire(IServiceCollection services)
        //{
        //    bool prepare = !Configuration.GetSection("AppSetting").GetValue<bool>("HangFireSkipPrepareSchemaIfNecessary");

        //    services.AddHangfire(opt => opt.UseSqlServerStorage(Configuration.GetSection("ConnectionString").GetValue<string>("Back2Work"),
        //        new SqlServerStorageOptions
        //        {
        //            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        //            // SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        //            QueuePollInterval = TimeSpan.FromSeconds(30),
        //            UseRecommendedIsolationLevel = true,
        //            UsePageLocksOnDequeue = true,
        //            DisableGlobalLocks = true,
        //            //TODO: run when deployed only (do not run on dev to reduce run times)
        //            PrepareSchemaIfNecessary = prepare
        //        }));
        //}

        ///// <summary>
        ///// schedules hangfire jobs
        ///// </summary>
        ///// <param name="app"></param>
        ///// <param name="serviceProvider"></param>
        //public void StartHangFireJobs(IApplicationBuilder app, IServiceProvider serviceProvider)
        //{
        //    app.UseHangfireServer();
        //    app.UseHangfireDashboard("/hangfire", new DashboardOptions
        //    {
        //        Authorization = new[] { new HangfireDashboardAuthorizationFilter() },
        //        StatsPollingInterval = 30000,
        //        // no back link
        //        AppPath = null
        //    });

        //    /*
        //     * CRON FORMATS
        //        # * * * * *  
        //        # │ │ │ │ │
        //        # │ │ │ │ │
        //        # │ │ │ │ └───── day of week (0 - 6) (0 to 6 are Sunday to Saturday, or use names; 7 is Sunday, the same as 0)
        //        # │ │ │ └────────── month (1 - 12)
        //        # │ │ └─────────────── day of month (1 - 31)
        //        # │ └──────────────────── hour (0 - 23)
        //        # └───────────────────────── min (0 - 59)
        //     * 
        //     * 0/15 run every 15 minutes starting at hh:00
        //     * 1/15 run every 15 minutes starting at hh:01
        //     */

        //    List<KeyValuePair<string, string>> listOfJobsAndTimes = new List<KeyValuePair<string, string>>
        //    {
        //        //new KeyValuePair<string, string>( "RewardExports", "0 10 * * 5 "), //Every Friday at 10:00am
        //        //new KeyValuePair<string, string>( "LetterExports", "0 16 * * 4"), //Every Thursday at 4:00PM
        //        //new KeyValuePair<string, string>( "UpdateFromFsvViews", "0 * * * *"), //Every hour
        //        //new KeyValuePair<string, string>( "FrontierImport", "*/5 * * * *"), //Every 5 minutes
        //        //new KeyValuePair<string, string>( "RewardNotificationEmails", "0 10 * * 4"), //Every Thursday at 10:00am
        //        //new KeyValuePair<string, string>( "RewardReminderNotificationEmails", "0 12 * * 4"), //Every Thursday at noon
        //        //new KeyValuePair<string, string>( "ResendEmails", "0 13 * * 3,5"), //Friday and Wed at 1pm
        //        //new KeyValuePair<string, string>( "ManualGarbageCollection", "* * * * *"), //Every minute
        //        new KeyValuePair<string, string>( "EmployeeAssessmentNotification", "*/30 * * * *"), //Every 30 minutes
        //        //new KeyValuePair<string, string>("SendEmail", "*/5 * * * *") //Every minute
        //    };

        //    HashSet<string> disabledJobs;
        //    var storage = new SqlServerStorage(Configuration.GetSection("ConnectionString").GetValue<string>("Back2Work"));
        //    using (var connection = storage.GetConnection())
        //    {
        //        disabledJobs = connection.GetAllItemsFromSet("disabled-jobs");
        //    }

        //    foreach (var jobAndTime in listOfJobsAndTimes)
        //    {
        //        if (!disabledJobs.Contains(jobAndTime.Key))
        //        {
        //            var method = typeof(ScheduledJobs).GetMethod(jobAndTime.Key);
        //            if (method != null)
        //            {
        //                var expression = Expression.Lambda<Action>(Expression.Call(method));
        //                RecurringJob.AddOrUpdate(expression, jobAndTime.Value, TimeZoneInfo.Local);
        //            }
        //        }
        //    }
        //}

        //public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
        //{
        //    public bool Authorize(DashboardContext context)
        //    {
        //        //var owinContext = new OwinContext(context.GetOwinEnvironment());
        //        //return owinContext.Authentication.User.Identity.IsAuthenticated && owinContext.Authentication.User.IsInRole(ConfigurationManager.AppSettings["AdminRole"]);
        //        return true;
        //    }
        //}


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors(builder => builder
            //    .WithOrigins("http://localhost:3000",
            //    "https://backtowork.firebaseapp.com",
            //    "https://backtoworkdev.firebaseapp.com",
            //    "https://b2wapp.aciano.net",
            //    "https://b2w.aciano.net",
            //    "https://back2work.mazikglobal.com",
            //    "https://back2workapi.mazikglobal.com")
            //    .AllowAnyHeader()
            //    .AllowAnyMethod()
            //    .AllowCredentials()
            //);

            app.UseCors(options => options.AllowAnyOrigin());
            //app.UseCors(MyAllowSpecificOrigins);
            //app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseElmah();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Back 2 Work V1");
            //});

            //if (HangFireEnabled)
            //    StartHangFireJobs(app, serviceProvider);
        }
    }
}
