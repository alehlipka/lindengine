# ![lindengine][lindengine-logo] Lindengine framework

![OpenTK NuGet Version](https://img.shields.io/nuget/v/opentk?style=flat-square&label=OpenTK)
![StbImageSharp NuGet Version](https://img.shields.io/nuget/v/StbImageSharp?style=flat-square&label=StbImageSharp)
![StbTrueTypeSharp NuGet Version](https://img.shields.io/nuget/v/StbTrueTypeSharp?style=flat-square&label=StbTrueTypeSharp)
![BepuPhysics NuGet Version](https://img.shields.io/nuget/v/BepuPhysics?style=flat-square&label=BepuPhysics)
![AssimpNet NuGet Version](https://img.shields.io/nuget/v/AssimpNet?style=flat-square&label=AssimpNet)

Three-dimensional multiplatform [.Net][dotnet-url] game framework / engine based on [OpenTK][opentk-url].

The goal of the project is to create a multiplatform more or less workable game engine with support for importing popular formats of 3d models, images and animations.

The repository contains:

1. **core** - the heart of the framework. Contains the core classes and assets.
2. **gui** - library for working with graphical interfaces.
3. **shader** - library for working with [GLSL][glsl-url] shaders.

## Table of Contents

- [Install](#install)
    - [Linux](#manjaro-linux)
    - [Windows](#windows)
- [Run](#run)

## Install

### Manjaro Linux

![Manjaro Linux Logo][manjaro-logo]

```sh
sudo pacman -S dotnet-sdk
```

[Official .Net packages for some other linux distributions][dotnet-linux-url]

### Windows

Download latest .Net from [this page][dotnet-download-url] and follow instructions.

### Run

Just go to projects directory and run this:

```sh
cd core
```
```sh
dotnet run
```

[lindengine-logo]: https://raw.githubusercontent.com/alehlipka/lindengine/main/core/assets/lindengine/lindengine-logo-icon.png
[manjaro-logo]: https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Manjaro-logo.svg/100px-Manjaro-logo.svg.png
[dotnet-url]: https://dotnet.microsoft.com
[dotnet-download-url]: https://dotnet.microsoft.com/en-us/download
[dotnet-linux-url]: https://learn.microsoft.com/en-us/dotnet/core/install/linux
[opentk-url]: https://github.com/opentk/opentk
[glsl-url]: https://www.khronos.org/opengl/wiki/OpenGL_Shading_Language
