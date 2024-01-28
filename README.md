# Lindengine

Three-dimensional multiplatform [.Net](https://dotnet.microsoft.com) game framework / engine based on [OpenTK](https://github.com/opentk/opentk).

The goal of the project is to create a multiplatform more or less workable game engine with support for importing popular formats of 3d models, images and animations.

The repository contains:

1. **core** - the heart of the framework. Contains the core classes and assets.
2. **gui** - library for working with graphical interfaces.
3. **shader** - library for working with GLSL shaders.

## Table of Contents

- [Install](#install)

## Install

### Manjaro Linux

```sh
sudo pacman -S dotnet-sdk
```

[Official .Net packages for some other linux distributions](https://learn.microsoft.com/en-us/dotnet/core/install/linux)

### Windows

Download latest .Net from [this page](https://dotnet.microsoft.com/en-us/download) and follow instructions.

### Then

Just go to projects directory and run this:

```sh
cd core
```
```sh
dotnet run
```
