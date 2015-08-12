# StatsDHelper

Simple wrapper around [statsd-csharp-client](https://github.com/lukevenediger/statsd-csharp-client) which adds a prefix based on the hosts Fully Qualified Domain Name and a configurable application name prefix.



So for a metric named *api.responses.httpstatus.500* coming from a host named *servername.example.com* with a configured application name of *api*.

The emitted metric will be:

```
com.example.servername.api.responses.httpstatus.500
```

The helper class is designed to fail silently in the event of misconfiguration (look for warnings in your build log) and to be used similarly to something like log4net.

## Quick Start

### Configuration

```xml
    <add key="StatsD.ApplicationName" value="foocalculator" />
    <add key="StatsD.Host" value="fooserver" />
    <add key="StatsD.Port" value="8125" />
```

### Example Usage:

```csharp
    public class FooService
    {
        private readonly IStatsDHelper _statsDHelper = StatsDHelper.StatsDHelper.Instance;

        public void CalculateFoo(int a, int b){
            _statsDHelper.LogCount("calculatefoo");
        }

        public List<FooWidget> GetFooWidgets(){
            var repository = new FooRepository();

            using(_statsDHelper.LogTiming("foorepository.getfoowidgets.latency") {
                return repository.GetFooWidgets();
            }
        }
    }
```





