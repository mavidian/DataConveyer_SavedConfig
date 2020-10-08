// Copyright © 2019-2020 Mavidian Technologies Limited Liability Company. All Rights Reserved.

using Mavidian.DataConveyer.Common;
using Mavidian.DataConveyer.Logging;
using Mavidian.DataConveyer.Orchestrators;
using System;

namespace ConfigCreator
{
   class Program
   {
      static void Main()
      {
         //Configure Data Conveyer process for the IncludeTimeZone project.
         var config = new OrchestratorConfig()
         // alternatively, we can add logger to facilitate troubleshooting:
         //var config = new OrchestratorConfig(LoggerCreator.CreateLogger(LoggerType.LogFile, "Configuration Creator process", LogEntrySeverity.Information))
         {
            ReportProgress = true,
            ProgressInterval = 1,
            InputDataKind = KindOfTextData.Delimited,
            InputFileName = @"..\..\..\Common\Input\input.csv",
            HeadersInFirstInputRow = true,
            AllowTransformToAlterFields = true,
            TransformerType = TransformerType.Recordbound,
            RouterType = RouterType.SingleTarget,
            OutputDataKind = KindOfTextData.Delimited,
            HeadersInFirstOutputRow = true,
            OutputFileName = @"..\..\..\Common\Output\output_with_time_zone.csv"
         };

         var configName = "TimeZones";
         //Configuration name ("TimeZones") must match the accompaying project that contains configuration functions.
         if (config.SaveConfig(@"..\..\..\..\Common\ConfigData\" +configName))
         {
            Console.Write($"Configuration saved in {configName}.cfg file.");
         }
         else
         {
            Console.Write("Ooops... error occurred when saving configuration.");
            //Console.Write(" Details can be found in the log file.");
         }
         Console.Write(" Press any key to exit...");

         Console.ReadKey();
      }
   }
}
