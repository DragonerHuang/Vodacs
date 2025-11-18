using Autofac;
using log4net;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vodace.Api.Controllers.Hubs;
using Vodace.Core.Configuration;
using Vodace.Core.Extensions;
using Vodace.Core.Filters;
using Vodace.Core.Hubs;
using Vodace.Core.Localization;
using Vodace.Core.Middleware;
using Vodace.Core.ObjectActionValidator;
using Vodace.Core.Quartz;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Core.Utilities.PDFHelper;
using Vodace.Entity.AutoMppper;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices.Biz.Partial;
using Vodace.Sys.Services.Biz.Partial;

namespace Vodace.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IServiceCollection Services { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //初始化模型验证配置
            services.UseMethodsModelParameters().UseMethodsGeneralParameters();
            services.AddSingleton<IObjectModelValidator>(new NullObjectModelValidator());
            Services = services;
            // services.Replace( ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            services.AddSession();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiAuthorizeFilter));
                options.Filters.Add(typeof(ActionExecuteFilter));
                //  options.SuppressAsyncSuffixInActionNames = false;
            });
            services.AddControllers()
              .AddNewtonsoftJson(op =>
              {
                  op.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                  op.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
              });

            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     SaveSigninToken = true,//保存token,后台验证token是否生效(重要)
                     ValidateIssuer = true,//是否验证Issuer
                     ValidateAudience = true,//是否验证Audience
                     ValidateLifetime = true,//是否验证失效时间
                     ValidateIssuerSigningKey = true,//是否验证SecurityKey
                     ValidAudience = AppSetting.Secret.Audience,//Audience
                     ValidIssuer = AppSetting.Secret.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.Secret.JWT))
                 };
                 options.Events = new JwtBearerEvents()
                 {
                     OnChallenge = context =>
                     {
                         context.HandleResponse();
                         context.Response.Clear();
                         context.Response.ContentType = "application/json";
                         context.Response.StatusCode = 401;
                         context.Response.WriteAsync(new { message = "授权未通过", status = false, code = 401 }.Serialize());
                         return Task.CompletedTask;
                     }
                 };
             });
            //必须appsettings.json中配置
            string corsUrls = Configuration["CorsUrls"];
            if (string.IsNullOrEmpty(corsUrls))
            {
                throw new Exception("请配置跨请求的前端Url");
            }
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.AllowAnyOrigin()
                           .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
                            .AllowAnyHeader().AllowAnyMethod();
                        });
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                string swagger_des = "这是对文档的描述。。";
                swagger_des = @"
                    公共字段说明：
                    创建时，guid系统自动生成
                    delete_status 是否删除（0：正常；1：删除；2：数据库手删除）
                    create_id 创建人id 
                    create_name 创建人名称
                    create_date 创建时间
                    modify_id 修改人id
                    modify_name 修改人名称
                    modify_date 修改时间
                    以上字段均为系统自动生成，无需手动维护
                ";

                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(
                        description.GroupName,
                        new OpenApiInfo
                        {
                            Title = description.GroupName.Contains("v2") ? "Vodace对外三方Api" : "Vodace后台Api",
                            Version = description.ApiVersion.ToString(),
                            Description = description.GroupName.Contains("v2") ? "xxx接口文档" : swagger_des
                        });
                }

                //c.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                //    var versions = methodInfo.DeclaringType
                //        .GetCustomAttributes(true)
                //        .OfType<ApiVersionAttribute>()
                //        .SelectMany(attr => attr.Versions);

                //    var maps = methodInfo.GetCustomAttributes(true)
                //        .OfType<MapToApiVersionAttribute>()
                //        .SelectMany(attr => attr.Versions)
                //        .ToArray();

                //    return versions.Any(v => $"v{v}" == docName) && (maps.Length == 0 || maps.Any(v => $"v{v}" == docName));
                //});

                //控制器里使用[ApiExplorerSettings(GroupName = "v2")]              
                //启用中文注释功能
                // var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Vodace.Api.xml");
                c.IncludeXmlComments(xmlPath, true);//显示控制器xml注释内容
                //添加过滤器 可自定义添加对控制器的注释描述
                //c.DocumentFilter<SwaggerDocTag>();

                var security = new Dictionary<string, IEnumerable<string>> { { AppSetting.Secret.Issuer, new string[] { } } };
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT授权token前面需要加上字段Bearer与一个空格,如Bearer token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            })
             .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
                options.ClientErrorMapping[404].Link =
                    "https://*/404";
            });
            Services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddSignalR();
            //services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            //services.AddTransient<IPDFService, PDFService>();

            services.AddHttpClient();
            Services.AddTransient<HttpResultfulJob>();
            Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            Services.AddSingleton<Quartz.Spi.IJobFactory, IOCJobFactory>();
            //Services.AddHostedService<QuartzStartup>();
            //services.AddQuartzHostedService(OptionsBuilderConfigurationExtensions => { });
            services.AddSingleton<Log4NetHelper>();
            ILoggerRepository loggerRepository = LogManager.CreateRepository("NETCoreRepository");
            Log4NetHelper.SetConfig(loggerRepository, "log4net.config");

            services.AddScoped<IHomePageMessageSender, HomePageMessageSender>();

            //设置文件上传大小限制
            //设置文件上传大小限制
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 1024 * 1024 * 100;//100M
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 1024 * 1024 * 100;//100M
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 1024 * 1024 * 100;//100M
            });

            // 注册本地化服务
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Services.AddModule(builder, Configuration);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
                app.UseQuartz(env);
            //}
            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            app.UseDefaultFiles();
            app.UseStaticFiles().UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });
            app.Use(HttpRequestMiddleware.Context);

            //2021.06.27增加创建默认upload文件夹
            string _uploadPath = (env.ContentRootPath + "/Upload").ReplacePath();

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Upload")),
                //配置访问虚拟目录时文件夹别名
                RequestPath = "/Upload",
                OnPrepareResponse = (Microsoft.AspNetCore.StaticFiles.StaticFileResponseContext staticFile) =>
                {
                    //可以在此处读取请求的信息进行权限认证
                    //  staticFile.File
                    //  staticFile.Context.Response.StatusCode;
                }
            });
            //配置HttpContext
            app.UseStaticHttpContext();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.Contains("v2") ? $"Vodace对外三方Api {description.GroupName}" : $"Vodace后台Api {description.GroupName}");
                }
                c.RoutePrefix = "";
            });
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //    //配置SignalR
            //    if (AppSetting.UseSignalR)
            //    {
            //        string corsUrls = Configuration["CorsUrls"];

            //        endpoints.MapHub<HomePageMessageHub>("/message")
            //        .RequireCors(t =>
            //        t.WithOrigins(corsUrls.Split(',')).
            //        AllowAnyMethod().
            //        AllowAnyHeader().
            //        AllowCredentials());
            //    }
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //配置SignalR
                if (AppSetting.UseSignalR)
                {
                    string corsUrls = Configuration["CorsUrls"];

                    endpoints.MapHub<HomePageMessageHubNew>("/message")
                    .RequireCors(t =>
                    t.WithOrigins(corsUrls.Split(',')). //允许跨域
                    AllowAnyMethod().
                    AllowAnyHeader().
                    AllowCredentials());
                }
            });
        }
    }

    /// <summary>
    /// Swagger注释帮助类
    /// </summary>
    public class SwaggerDocTag : IDocumentFilter
    {
        /// <summary>
        /// 添加附加注释
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //添加对应的控制器描述
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                new OpenApiTag { Name = "Test", Description = "这是描述" },
                //new OpenApiTag { Name = "你的控制器名字，不带Controller", Description = "控制器描述" },
                new OpenApiTag{ Name="Sys_User",Description="用户管理"  }
            };
        }
    }
}
