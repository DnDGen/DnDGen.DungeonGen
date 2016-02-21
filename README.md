DungeonGen
============

Generate random Dungeons & Dragons dungeons

[![Build Status](https://travis-ci.org/DnDGen/DungeonGen.svg?branch=master)](https://travis-ci.org/DnDGen/DungeonGen)

**Note: this is still in development and is not yet released**

### Use

To use DungeonGen, simply use the DungeonGenerator.  Levels up to 20 are supported.

```C#
var fromDoor = dungeonGenerator.GenerateFromDoor(15);
var fromHall = dungeonGenerator.GenerateFromHall(7);
```

### Getting the Generators

You can obtain generators from the bootstrapper project.  Because the generators are very complex and are decorated in various ways, there is not a (recommended) way to build these generator manually.  Please use the Bootstrapper package.  **Note:** if using the EnounterGen bootstrapper, be sure to also load modules for RollGen, TreasureGen, CharacterGen, and EncounterGen, as it is dependent on those modules

```C#
var kernel = new StandardKernel();
var rollGenModuleLoader = new RollGenModuleLoader();
var treasureGenModuleLoader = new TreasureGenModuleLoader();
var characterGenModuleLoader = new CharacterGenModuleLoader();
var encounterGenModuleLoader = new EncounterGenModuleLoader();
var dungeonGenModuleLoader = new DungeonGenModuleLoader();

rollGenModuleLoader.LoadModules(kernel);
treasureGenModuleLoader.LoadModules(kernel);
characterGenModuleLoader.LoadModules(kernel);
encounterGenModuleLoader.LoadModules(kernel);
dungeonGenModuleLoader.LoadModules(kernel);
```

Your particular syntax for how the Ninject injection should work will depend on your project (class library, web site, etc.)

### Installing DungeonGen

The project is on [Nuget](https://www.nuget.org/packages/DungeonGen). Install via the NuGet Package Manager.

    PM > Install-Package DungeonGen

#### There's DungeonGen and DungeonGen.Bootstrap - which do I install?

That depends on your project.  If you are making a library that will only **reference** DungeonGen, but does not expressly implement it, then you only need the DungeonGen package.  If you actually want to run and implement the dice (such as on the DnDGenSite or in the integration tests for DungeonGen), then you need DungeonGen.Bootstrap, which will install DungeonGen as a dependency.
