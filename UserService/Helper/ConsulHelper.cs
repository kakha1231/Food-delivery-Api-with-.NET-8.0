using Consul;

namespace UserService.Helper;

    public static class ConsulHelper
    {
        public static IApplicationBuilder RegisterConsul(
            this IApplicationBuilder app,
            IConfiguration configuration,
            IHostApplicationLifetime lifetime)
        {
            var consulClient = new ConsulClient(c => { c.Address = new Uri("http://consul:8500"); });

            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(), //服务实例ID
                Name = "user-service",//服务名
                Address =  "userservice",//服务主机
                Port = 5038,//服务端口
                Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔

                    // 健康检查地址
                    HTTP =
                        $"http://userservice:5038/health",

                    // 超时时间
                    Timeout = TimeSpan.FromSeconds(60)
                }
            };

            // 注册服务
            consulClient.Agent.ServiceRegister(registration).Wait();

            // 应用程序终止时，取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
