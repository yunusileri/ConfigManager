using System;

namespace ConfigManager.Core.DependencyInjection
{
    public class InstanceFactory
    {
        private static IServiceProvider _serviceProvider = null!;

        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            var service = (T)_serviceProvider.GetService(typeof(T));
            if (service == null)
            {
                throw new Exception($"Service is null {typeof(T)}");
            }

            return service;
        }
    }
}