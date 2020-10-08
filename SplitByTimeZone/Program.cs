// Copyright © 2019-2020 Mavidian Technologies Limited Liability Company. All Rights Reserved.

using Mavidian.DataConveyer.Common;
using Mavidian.DataConveyer.Logging;
using Mavidian.DataConveyer.Orchestrators;
using System;

namespace SplitByTimeZone
{
   class Program
   {
      static void Main()
      {
         Console.WriteLine("Data Conveyer process is starting (SplitByTimeZone)");

         //Restore configuration named TimeZones
         var config = OrchestratorConfig.RestoreConfig(@"..\..\..\Common\ConfigData\TimeZones");

         // To facilitate troubleshooting, logger can be enabled; like so (output will go into DataConveyer.log file):
         //var config = OrchestratorConfig.RestoreConfig(@"..\..\..\Common\ConfigData\TimeZones", LoggerCreator.CreateLogger(LoggerType.LogFile, "Split by Time Zone process", LogEntrySeverity.Information));

         if (config == null)
         {
            Console.WriteLine("Oops! Failed to restore TimeZones config. More information in the log.");
         }
         else
         {  //TimeZones configuration restored
            // We need to adjust some properties as the TimeZones configuration was specific to IncludeTimeZone
            // project (but also included the RecordRouter that is needed here in SplitByTimeZone).
            config.TransformerType = TransformerType.Recordbound;
            config.RecordboundTransformer = r => r;  //reset transformer to its default
            config.RouterType = RouterType.PerRecord;
            config.OutputDataKind = KindOfTextData.Delimited;
            config.OutputFileNames = @"..\..\..\Common\Output\output_eastern.csv|..\..\..\Common\Output\output_central.csv|..\..\..\Common\Output\output_mountain.csv|..\..\..\Common\Output\output_pacific.csv|..\..\..\Common\Output\output_alaskan.csv|..\..\..\Common\Output\output_hawaiian.csv";

            // Execute Data Conveyer process:
            ProcessResult result;
            using (var orchtr = OrchestratorCreator.GetEtlOrchestrator(config))
            {
               var execTask = orchtr.ExecuteAsync();
               result = execTask.Result; //sync over async
            }
            Console.WriteLine(" done!");

            // Evaluate completion status:
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
