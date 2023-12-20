namespace TMS.RabbitMq;

public class Exchange 
{
    public sealed class Type
    {
        public readonly static string Direct = "direct";
        public readonly static string Fanout = "fanout";
    }

    public sealed class Name
    {
        public readonly static string Payments = "ProjectX.Payments";
        public readonly static string Notifications = "ProjectX.Notifications";
        public readonly static string Realtime = "ProjectX.Realtime";
    }
}