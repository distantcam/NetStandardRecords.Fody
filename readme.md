# <img src="package_icon.png" height="30px"> NetStandardRecords.Fody

[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg)](https://gitter.im/Fody/Fody)

Allows use of C# 9 records in a .NET Standard project, and allows other .NET Standard projects to reference the records project.

### This is an add-in for [Fody](https://github.com/Fody/Home/)


## Usage

See also [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md).


### NuGet installation

Install the [NetStandardRecords.Fody NuGet package](https://www.nuget.org/packages/NetStandardRecords.Fody) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package NetStandardRecords.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Add to FodyWeavers.xml

Add `<NetStandardRecords/>` to [FodyWeavers.xml](https://github.com/Fody/Home/blob/master/pages/usage.md#add-fodyweaversxml)

```xml
<Weavers>
  <NetStandardRecords/>
</Weavers>
```

### Use C# 9 as the language version

In your project, set C# 9 as the language version.

```xml
<Project>
  ...
  <PropertyGroup>
    <LangVersion>9.0</LangVersion> <!-- Or 'latest' -->
  </PropertyGroup>
  ...
</Project>
```

## Why is this needed?

C# 9 records are by default immutable. However to allow them to work with existing serialization frameworks the property setters need to be public.

https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#record-types

https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#init-only-setters

So that the compiler knows that these properties are immutable they are marked with a special marker in IL called `System.Runtime.CompilerServices.IsExternalInit`. However that type is only availble in .NET 5 and later. To use C# 9 records in .NET Standard and .NET Core the type has to be supplied by this package instead.

https://github.com/dotnet/roslyn/issues/45510

Now, to reference a project with records in another .NET Standard or .NET Core project you either need C# 9, or the `IsExternalInit` marker needs to be removed. For projects like Xamarin, that rely on the mono compiler that currently don't use C# 9 it can cause issues having that marker in the assembly.

https://github.com/dotnet/runtime/issues/34978#issuecomment-614845405

TL;DR - I had a C# 9 server project with a common library that was also being used in a Xamarin iOS project that was still using C# 8 and could not be upgraded. This allowed the common project to be shared between C# 8 and 9 projects.


## Icon

Icon courtesy of [The Noun Project](https://thenounproject.com)
