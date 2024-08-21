using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Voracious.Core.Interface;
using Voracious.Core.ViewModel;
using Voracious.Database;

namespace Voracious.Gutenberg;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousGutenberg(this IServiceCollection collection)
    {
        collection
            .AddHttpClientFactory()
            ;
        return collection;
    }
}
