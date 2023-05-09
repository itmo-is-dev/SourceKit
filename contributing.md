# Creating analyzer

## Create analyzer project

> All analyzer projects must target netstandard2.0 due to Visual Studio not loading other runtimes in code designing
> stage.\n
> You should specify `<LangVersion>11</LangVersion>` to use latest language features

### Add NuGet build properties

Add properties for package generation

```xml

<PropertyGroup>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    <IncludeBuildOutput>false</IncludeBuildOutput>
</PropertyGroup>
```

Add separate property group for constant NuGet package properties

```xml

<PropertyGroup>
    <PackageId>SourceKit.Analyzers.MustBePartial</PackageId>
    <Title>SourceKit.Analyzers.MustBePartial</Title>
    <Authors>ronimizy</Authors>
</PropertyGroup>
```

And separate group for more volatile properties

```xml

<PropertyGroup>
    <PackageReleaseNotes>Analyzers for checking whether type is partial</PackageReleaseNotes>
</PropertyGroup>
```

### Add Roslyn SDK dependencies

```xml

<ItemGroup>
    <PackageReference Include="Microsoft.CodeAnalysis.Common"/>
    <PackageReference Include="Microsoft.CodeAnalysis.CSharp" VersionOverride="4.5.0"/>
    <PackageReference Include="Microsoft.CodeAnalysis.Workspaces.Common"/>
</ItemGroup>
```

You have to manually add assemblies to your analyzers NuGet package path

```xml

<ItemGroup>
    <None Include="$(OutputPath)\$(AssemblyName).dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false"/>
    <None Include="$(OutputPath)\$(AssemblyName).Annotations.dll" Pack="true" PackagePath="analyzers/dotnet/cs"
          Visible="false"/>
    <None Include="$(OutputPath)\SourceKit.dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false"/>
</ItemGroup>
```

[Docs](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#use-functionality-from-nuget-packages)

## Implementation

Choose diagnostic category, based on MS Docs listing \
https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories

## Writing sample code

After implementing your analyzer code, include sample code, describing all use cases of your feature
into `SourceKit.Sample` project

Use designated folder for it (ex: `Analyzers/MustBePartial`)

## Unit testing

You should validate your sample code using unit tests. 

### Analyzers

1. Specify type alias for your verifier
   ```csharp
   using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<{AnalyzerType}>;
   ```
2. Create `CSharpAnalyzerTest<{AnalyzerType}, XUnitVerifier>` instance
3. Add source files using `SourceFile.LoadAsync`
   ```csharp
   var test = new CSharpAnalyzerTest<{AnalyzerType}, XUnitVerifier>
   {
      TestState =
      {
          Sources =
          {
              await SourceFile.LoadAsync("..."),
          },
      },
   };
   ```
4. Add reference to annotations package
   ```csharp 
   var test = new CSharpAnalyzerTest<{AnalyzerType}, XUnitVerifier>
   {
      TestState =
      {
          AdditionalReferences = { typeof(SomeAttribute).Assembly },
      },
   };
   ```
5. Specify diagnostics if they are expected
   ```csharp
   var diagnostic = Verify.Diagnostic(Analyzer.Descriptor)
      .WithLocation(sourceFile.Name, 3, 14)
      .WithArguments(nameof(SomeType));
   
   var test = new CSharpAnalyzerTest<{AnalyzerType}, XUnitVerifier>
   {
      ExpectedDiagnostics = { diagnostic },
   };
   ```
6. Run test 
   ```csharp
   await test.RunAsync();
   ```
   
### CodeFix providers
1. Specify type alias for your verifier
   ```csharp
   using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<{AnalyzerType}>;
   ```
2. Specify type alias for your code fix test 
   ```csharp
   using CodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
       {AnalyzerType},
       {CodeFixProviderType},
       Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;
   ```
3. Add source files using `SourceFile.LoadAsync`
   ```csharp
   var test = new CodeFixTest
   {
      TestState =
      {
          Sources =
          {
              await SourceFile.LoadAsync("..."),
          },
      },
   };
   ```
4. Add reference to annotations package
   ```csharp 
   var test = new CodeFixTest
   {
      TestState =
      {
          AdditionalReferences = { typeof(SomeAttribute).Assembly },
      },
   };
   ```
5. Specify diagnostics if they are expected
   ```csharp
   var diagnostic = Verify.Diagnostic(Analyzer.Descriptor)
      .WithLocation(sourceFile.Name, 3, 14)
      .WithArguments(nameof(SomeType));
   
   var test = new CodeFixTest
   {
      ExpectedDiagnostics = { diagnostic },
   };
   ```
6. Specify fixed state
   ```csharp
   var test = new CodeFixTest
   {
      FixedState =
      {
          Sources = { fixedSource, interfaceSource },
      },
   };
   ```
7. Run test
   ```csharp
   await test.RunAsync();
   ```