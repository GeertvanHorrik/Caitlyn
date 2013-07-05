Caitlyn
=======

Visual Studio extension able to link files of a base project to all other projects.

For example, assume the following project structure:

Catel.Core.NET35
Catel.Core.NET40
Catel.Core.NET45
Catel.Core.SL4
etc

The projects can be linked automatically.

The main features are:
* Automatically synchronizes linked files from a root project to a selectable list of other projects
* Copies xaml files for NET40 because they cannot be linked (bug in Visual Studio)
* Does not remove files that are not linked (so actual files are not removed, only linked files)
* Rules so files can not be added for specific target projects or not be removed from specific target projects
* Configuration editor to be able to easily manage the rules

## Documentation

Documentation can be found at https://github.com/GeertvanHorrik/Caitlyn/wiki

## Issue tracking

The issue tracker including a roadmap can be found at https://github.com/GeertvanHorrik/Caitlyn/issues