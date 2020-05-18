using App.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebApiClientCore.OAuths;

namespace App
{
    /// <summary>
    /// ����ҳ
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ��ȡ����
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ����
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>     
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }


        /// <summary>
        /// ���ӷ���
        /// </summary>
        /// <param name="services"></param>  
        public void ConfigureServices(IServiceCollection services)
        {
            // ���ӿ�����
            services.AddControllers().AddXmlSerializerFormatters();

            // ע��userApi
            services.AddHttpApi<IUserApi>(o =>
            {
                o.HttpHost = new Uri("http://localhost:6000/");
            });

            // ����token��ȡѡ��
            services.Configure<ClientCredentialsOptions<IUserApi>>(o =>
            {
                o.Endpoint = new Uri("http://localhost:6000/api/tokens");
                o.Credentials = new ClientCredentials
                {
                    Client_id = "clientId",
                    Client_secret = "xxyyzz"
                };
            });

            // userApi�ͻ��˺�̨����
            services.AddScoped<UserService>().AddHostedService<UserHostedService>();
        }

        /// <summary>
        /// �����м��
        /// </summary>
        /// <param name="app"></param>    
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}