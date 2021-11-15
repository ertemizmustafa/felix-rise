using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Schedule.Core.Infrastructure
{
    public class BackgroundProcessActivator : JobActivator
    {
        private readonly IServiceProvider _container;

        public BackgroundProcessActivator(IServiceProvider provider)
        {
            _container = provider;
        }

        public override object ActivateJob(Type type)
        {
            using var scope = _container.CreateScope();
            return scope.ServiceProvider.GetRequiredService(type);
        }
    }
}
