// Copyright © 2019-2020 Mavidian Technologies Limited Liability Company. All Rights Reserved.

using Mavidian.DataConveyer.Logging;
using Mavidian.DataConveyer.Orchestrators;
using System;

namespace IncludeTimeZone
{
   class Program
   {
      static void Main()
      {
         Console.WriteLine("Data Conveyer process is starting (IncludeTimeZone)");

         //Restore configuration named TimeZones
         var config = OrchestratorConfig.RestoreConfig(@"..\..\..\Common\ConfigData\TimeZones");

         // To facilitate troubleshooting, logger can be enabled; like so (output will go into DataConveyer.log file):
         //var config = OrchestratorConfig.RestoreConfig(@"..\..\..\Common\ConfigData\TimeZones", LoggerCreator.CreateLogger(LoggerType.LogFile, "Include Time Zone process", LogEntrySeverity.Information));

         if (config == null)
         {
            Console.WriteLine("Oops! Failed to restore TimeZones config. More information in the log.");
         }
         else
         {  //TimeZones configuration restored
            //No need to adjust any config properties, "TimeZones" config lined up all config properties
            //(note that the RecordRouter function is not used when RouterType is SingleTarget).

            //Execute Data Conveyer process:
            ProcessResult result;
            using (var orchtr = OrchestratorCreator.GetEtlOrchestrator(config))
            {
               var execTask = orchtr.ExecuteAsync();
               result = execTask.Result; //sync over async
            }
            Console.WriteLine(" done!");

            //Evaluate completion status:
            if (result.CompletionStatus == CompletionStatus.IntakeDepleted)
               Console.WriteLine($"Successfully processed {result.RowsWritten} records");
            else
               Console.WriteLine($"Oops! Processing resulted in unexpected status of " + result.CompletionStatus.ToString());
         }

         Console.Write("Press any key to exit...");
         Console.ReadKey();
      }
   }
}
