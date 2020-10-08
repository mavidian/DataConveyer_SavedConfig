# DataConveyer_SavedConfig

DataConveyer_SavedConfig solution demonstrates ability to save and
restore Data Conveyer configuration. The configuration is named
TimeZones and consists of TimeZones.cfg and TimeZones.dll files.

The basic idea is to comply with the DRY (Don't Repeat Yourself)
principle; specifically, to avoid redundant implementation of the same
transformation functionality needed in 2 different applications:
IncludeTimeZone and SplitByTimeZone. The TimeZones configuration
allows transformation functionality to be reused in both applications.

The functionality inside TimeZones.dll calculates the US time
zone based on the US state. This functionality is used in two
projects: one project adds the time zone to each person in the
input list and the other project splits the list of people by the
time zone.

## Contents Summary

* Config folder with 2 projects to create TimeZones configuration data:

  - ConfigCreator - will create TimeZones.cfg file upon execution.

  - TimeZones     - will create TimeZones.dll file upon Build.

* IncludeTimeZone folder containing first of the 2 projects that use TimeZones
configuration. Upon execution, will create output_with_time_zone.csv file.

* SplitByTimeZone folder containing second of the 2 projets that use TimeZones
configuration. Upon execution will create 6 output_...csv files (one per time zone).

* Common folder containing data shared between projects.
Subfolders of the Common folder:

  - ConfigData folder intended to contain saved configuration data named TimeZones, i.e. the results from the two projects located in the Config folder.

    + TimeZones.cfg is the XML configuration data created by executing the ConfigCreator application.

    + TimeZones.dll is the build target of the TimeZones project; it is copied to ConfigData during the TimeZones project build (via post-build event).

  - Input folder - input location for the two projects located in the main folder, i.e. IncludeTimeZone and SplitByTimeZone (note that both projects use the TimeZones configuration from the ConfigData folder). A sample input file (input.csv) is included.

  - Output folder - output destination for files produced by the two projects located in the main folder, i.e. IncludeTimeZone and SplitByTimeZone.

## Installation

* Fork this repository and clone it onto your local machine, or

* Download this repository onto your local machine.

## Usage

1. Open DataConveyer_SavedConfig solution in Visual Studio.

2. Rebuild the DataConveyer_SavedConfig solution. Build of the TimeZones project will post the
TimeZones.dll assembly to the ConfigData folder.

3. Run ConfigCreator, e.g. (F5) as it is the Startup Project. It will add the TimeZones.cfg
file to the ConfigData folder.

4. Run IncludeTimeZone, e.g. by double-clicking exe file in IncludeTimeZone/bin/Debug folder.
The process will use the TimeZones configuration contained in TimeZones.cfg and TimeZones.dll files. 

5. Run SplitByTimeZone, e.g. by double-clicking exe file in SplitByTimeZone/bin/Debug folder.
The process will reuse the TimeZones configuration contained in TimeZones.cfg and TimeZones.dll files. 

6. Examine output files placed in the Common/Output folder.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)

## Copyright

```
Copyright © 2019-2020 Mavidian Technologies Limited Liability Company. All Rights Reserved.
```
