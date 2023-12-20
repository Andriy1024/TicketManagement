namespace TMS.Observability;

public class JaegerConfig
{
    public bool Enabled { get; set; }

    public string JAEGER_HOST { get; set; }

    public int JAEGER_PORT { get; set; }
}
