# PCL SpecialFolder

[![icon64](https://cloud.githubusercontent.com/assets/17246426/14784059/9e02ebde-0b2e-11e6-9b3e-b2597545f605.png)](http://illustrain.com/?p=8895 "illustrain")
[![Build status](https://ci.appveyor.com/api/projects/status/c09he542tgvps8nl?svg=true)](https://ci.appveyor.com/project/picolyl/xamarinplugins-pclspecialfolder)

Support library for [PCLStorage](https://github.com/dsplaisted/PCLStorage "PCLStorage") (Xamarin.Plugins).

## SpecialFolder

OS-Specific folders.

||Desktop|Win8.1|WP8.1|UWP|Android|iOS|Mac|
| --- |:---:|:---:|:---:|:---:|:---:|:---:|:---:|
|Local    |o|o|o|o|o|o|o|
|Roaming  |o|o|o|o|-|-|-|
|Temporary|o|o|o|o|-|o|o|
|Cache    |o|-|o|o|o|o|o|
|Documents|o|o|o|o|-|o|o|
|Pictures |o|o|o|o|o|o|o|
|Music    |o|o|o|o|o|o|o|
|Videos   |o|o|o|o|o|o|o|
|App      |o|o|o|o|-|o|o|

## Usage

```C#
using Plugin.PCLSpecialFolder;
var localFolder = PCLSpecialFolder.Current.Local;
var file = await localFolder.CreateFileAsync("test.txt", CreationCollisionOption.ReplaceExisting);
```

### WinRT

You must declare the capability to be required by the package.appxmanifest.

```
<Capabilities>
 <Capability Name="picturesLibrary" />
 ...
</Capabilities>
```

### Desktop

You need to set the company name in the AssemblyInfo.cs.

```
AssemblyInfo.cs
[assembly: AssemblyCompany("TestApp")]
```

## License

[Ms-PL](https://msdn.microsoft.com/library/gg592960.aspx "Ms-PL")

## Link

- Icon : [illustrain](http://illustrain.com/?p=8895 "illustrain")
- Base Library : [PCLStorage](https://github.com/dsplaisted/PCLStorage "PCLStorage")
- FolderPath : [FolderPath](https://github.com/picolyl/XamarinPlugins.FolderPath "FolderPath")
