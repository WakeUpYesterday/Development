using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHubGatewayService
{
    using TestHubGatewayService.AlarmService;

    class Program
    {
        static void Main(string[] args)
        {
            AlarmService.AlarmNotificationServiceClient client = new AlarmNotificationServiceClient();
            string a = string.Empty;
            while ( a != "e")
            {
                a = Console.ReadLine();

                if (a == "a")
                {
                    client.PublishAlarmOccurred(GetActiveAlarm());
                }
            }

        }

        public  static ActiveAlarmDto GetActiveAlarm()
        {
            ActiveAlarmDto activeAlarmDto = new ActiveAlarmDto();

            activeAlarmDto.Severity = Severity.Critical;
            activeAlarmDto.SourceElementId = 5;
            activeAlarmDto.AlarmStatus = AlarmStatus.Active;
            activeAlarmDto.CreationTime = DateTime.Now;
            activeAlarmDto.ProbableCauseCode = "Test";
            activeAlarmDto.AddressExtension = "addr";

            return activeAlarmDto;
            
        }

    }
}
