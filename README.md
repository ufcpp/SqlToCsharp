# SqlToCsharp

Target Platform is `netcoreapp2.1`, however dependent package `Microsoft.SqlServer.TransactSql.ScriptDom` is for .NET Framework 4.6.
this means SqlToCsharp guaranteed to run on Windows but not for other platform.

> confirm work on linux-x64 with sample project.

## dotnet global tool

Install.

```shell
dotnet tool install -g SqlToCsharp
```

Run with samples.

```shell
dotnet-sqltocsharp ./samples/SqlToCsharpSampleDatabase ./samples/SqlToCsharpSampleConsoleApp SqlToCsharpSample
```

## Single executable

Build Single executable.

**windows**

```shell
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
```

run with samples.

```shell
SqlToCsharp_win-x64.exe ./samples/SqlToCsharpSampleDatabase ./samples/SqlToCsharpSampleConsoleApp SqlToCsharpSample
```

**linux**

confirm work on linux-x64 (ubuntu 18.04).

```shell
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true
```

```shell
./SqlToCsharp_linux-x64 ./samples/SqlToCsharpSampleDatabase ./samples/SqlToCsharpSampleConsoleApp SqlToCsharpSample
```

**linux**

not-confirm on macos.

```shell
dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true
```

```shell
./SqlToCsharp_osx-x64 ./samples/SqlToCsharpSampleDatabase ./samples/SqlToCsharpSampleConsoleApp SqlToCsharpSample
```
