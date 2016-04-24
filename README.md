# PCL SpecialFolder

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

## Thanks

- Base Library : [PCLStorage](https://github.com/dsplaisted/PCLStorage "PCLStorage")
