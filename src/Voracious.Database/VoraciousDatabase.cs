using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Voracious.Core.Interface;
using Voracious.Core.ViewModel;

namespace Voracious.Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection VoraciousDatabase(this IServiceCollection collection)
    {
        collection
            .AddEntityFrameworkSqlite()
            .AddDbContext<VoraciousDataContext>()
            ;
        return collection;
    }
}
