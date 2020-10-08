// Copyright © 2019-2020 Mavidian Technologies Limited Liability Company. All Rights Reserved.

using Mavidian.DataConveyer.Common;
using Mavidian.DataConveyer.Entities.KeyVal;
using Mavidian.DataConveyer.Orchestrators;
using System;
using System.Linq;

namespace TimeZones
{
   public enum TimeZone
   {
      Eastern,
      Central,
      Mountain,
      Pacific,
      Alaskan,
      Hawaiian
   }


   /// <summary>
   /// A set of configuration functions to be added to OrchestatorConfig by the RestoreConfig method.
   /// </summary>
   public static class ConfigFunctions
   {
      //Special convention:
      //  All public methods must have the corresponding properties in Data Conveyer's OrchestatorConfig object.
      //  The "corresponding" means matching names and signatures. For example, RecordboundTransformer method below
      //  is declared as 'public static IRecord RecordboundTransformer(IRecord inRec)' below, which has this
      //  corresponding property of config object: 'public Func<IRecord, IRecord> RecordboundTransformer'.


      /// <summary>
      /// Add a new field named TimeZone to the record
      /// </summary>
      /// <param name="inRec"></param>
      /// <returns></returns>
      public static IRecord RecordboundTransformer(IRecord inRec)
      {
         var outRec = inRec.GetClone();
         outRec.AddItem("TimeZone", ToTimeZone((string)outRec["STATE"]));
         return outRec;
      }

      /// <summary>
      /// Determine the output file based on the time zone associated with the record
      /// </summary>
      /// <param name="rec"></param>
      /// <param name="clstr"></param>
      /// <returns>Target number</returns>
      public static int RecordRouter(IRecord rec, ICluster clstr)
      {
         switch (ToTimeZone((string)rec["STATE"]))
         {
            case TimeZone.Eastern: return 1; // 1st output file
            case TimeZone.Central: return 2;
            case TimeZone.Mountain: return 3;
            case TimeZone.Pacific: return 4;
            case TimeZone.Alaskan: return 5;
            default: return 6;  //TimeZone.Hawaiian
         }
      }
 
      /// <summary>
      /// Report progress once the record 
      /// </summary>
      /// <param name="s"></param>
      /// <param name="e"></param>
      public static void ProgressChangedHandler(object s, ProgressEventArgs e)
      {
         if (e.Phase == Phase.Output)
            Console.Write($"\rProcessed {e.RecCnt} records so far...");
      }
     
      /// <summary>
      /// Helper method to translate state abbreviation to a time zone the state is located in.
      /// </summary>
      /// <param name="state"></param>
      /// <returns></returns>
      private static TimeZone ToTimeZone(string state)
      {
         //Note that this is not exact as some states span time zones.
         var easternStates = new string[] { "CT", "DE", "FL", "GA", "IN", "KY", "ME", "MD", "MA", "MI", "NH", "NJ", "NY", "NC", "OH", "PA", "RI", "SC", "VT", "VA", "WV" };
         var centralStates = new string[] { "AL", "AR", "IL", "IA", "KS", "LA", "MN", "MS", "MO", "NE", "ND", "OK", "SD", "TN", "TX", "WI" };
         var mountainStates = new string[] { "AZ", "CO", "ID", "MT", "NM", "UT", "WY" };
         var pacificStates = new string[] { "CA", "NV", "OR", "WA"};
         if (easternStates.Contains(state)) return TimeZone.Eastern;
         if (centralStates.Contains(state)) return TimeZone.Central;
         if (mountainStates.Contains(state)) return TimeZone.Mountain;
         if (pacificStates.Contains(state)) return TimeZone.Pacific;
         if (state == "AK") return TimeZone.Alaskan;
         if (state == "HI") return TimeZone.Hawaiian;
         throw new ArgumentException("Argument value must be one of the valid US state abbreviations.");
      }
   }
}
