# NuCheck [![Build status](https://ci.appveyor.com/api/projects/status/h8dq23fjvtevlc18)](https://ci.appveyor.com/project/mbenford/nucheck)

NuGet is a great tool and a great package manager for .NET applications. Unfortunately it currently doesn't alert us when we are trying to add a different version of a package already in use by some other project in a solution. And since the Manage NuGet Packages dialog box always installs the latest version of a package, as we add new projects we can easily end up having several different versions of a package in use across the solution. Despite being a perfectly valid scenario, having multiples versions of an assembly in use can lead to nasty runtime errors.

To help catch this problem earlier in the development process I created NuCheck, a little tool to check if there are different versions of a NuGet package in use in a solution. Just provide it with a solution file and you're done: it'll tell you if everything is fine or if there is something to worry about.

## Downloading

You can grab the latest version of NuCheck by accessing the [Artifacts page](https://ci.appveyor.com/project/mbenford/nucheck/build/artifacts) on AppVeyor.

## Building from the source code

Building NuCheck from the source code is just a three-step process:

	> git clone https://github.com/mbenford/nucheck.git
	> cd NuCheck
	> build.cmd

When the build script finishes running, NuCheck executable file will be found in the newly created Output directory.

## Using NuCheck

NuCheck accepts a solution file as its first command line argument:

	> nucheck solution.sln

If different versions of a package are being used by different projects, a summary will be displayed:

	> nucheck solution.sln	
	2 issues found
	
	Ninject (2 versions)
	=> 3.0.0.15 (1 project)
	   - Project2
	=> 3.0.1.10 (2 projects)
	   - Project1
	   - Project3
	
	RabbitMQ.Client (3 versions)
	=> 3.0.0 (1 project)
	   - Project2
	=> 3.0.2 (1 project)
	   - Project3
	=> 3.0.4 (1 project)
	   - Project1

Optionally you can pass a pattern as a second argument to NuCheck so it selects only a subset of all packages in use (wildcards are supported):

    > nucheck solution.sln ServiceStack*
    3 issues found

    ServiceStack.Common (2 versions)
	=> 3.9.71 (1 project)
	   - Project1
	=> 4.0.17 (2 projects)
	   - Project2
	   - Project3
	
	ServiceStack.Redis (3 versions)
	=> 3.9.37 (1 project)
	   - Project1
	=> 3.9.71 (1 project)
	   - Project2
	=> 4.0.17 (1 project)
	   - Project3

As you would expect, NuCheck returns a non-zero exit code when an issue is found so that you can use it as part of a build script and/or a CI system to keep an eye on the packages being added by your team.
