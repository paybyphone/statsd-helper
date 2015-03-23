# StatsDHelper

Simple wrapper around [statsd-csharp-client](https://github.com/lukevenediger/statsd-csharp-client) which adds a prefix based on the hosts Fully Qualified Domain Name and a configurable application name prefix.

So for a metric named *api.responses.httpstatus.500* coming from a host named *servername.example.com* with a configured application name of *api*.

The emitted metric will be:

```
com.example.servername.api.responses.httpstatus.500
```

The helper class is designed to fail silently in the event of misconfiguration (look for warnings in your build log) and to be used similarly to something like log4net.

###Configuration

```xml
    <add key="StatsD.ApplicationName" value="personalizationservice" />
    <add key="StatsD.Host" value="vvq-mon004" />
    <add key="StatsD.Port" value="8125" />     
```

###Example Usage (for a Web Api status code metric):

```csharp
    public class InstrumentStatusCodeFilterAttribute : ActionFilterAttribute
    {
        readonly IStatsDHelper _statsDHelper = StatsDHelper.StatsDHelper.Create();

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            _statsDHelper.LogCount(string.Format("{0}{1}", actionName, actionExecutedContext.Response.StatusCode));
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
```