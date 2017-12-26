# Building instructions

In order to succesfully build HueHue, there's a few libraries that are used:

### MyGet

* [RGB.NET](https://github.com/DarthAffe/RGB.NET) for interfacing with RGB devices.

My get MUST be added as a package source on Visual Studio.

### Nuget

* [CSCore](https://github.com/filoe/cscore) for audio processing.
* [MaterialDesignInXAML](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit) for UI.
* [SharpDX](https://github.com/sharpdx/SharpDX) for joystick input processing.
* [Newtonsoft.Json.NET](https://github.com/JamesNK/Newtonsoft.Json) for .json manipulation.
* [Spectrum](https://github.com/nigel-sampson/spectrum) for working with and converting between colorspaces.

All of these should download on first build automagically.

### Github
* [ColorControl](https://github.com/BrianLima/ColorControl) for providing the ColorPicker used on the project.

You can download and build it manually from my fork, but it is already added on \HueHue\Resources\

I also recomend using [Visual Micro](http://www.visualmicro.com/) for working with .ino files on Visual Studio, the free version is enough, but the normal Arduino IDE or VSCode or even Notepad should be just as good.