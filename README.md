
# LogicCircle based on NodifyAvalonia <img src="https://github.com/BAndysc/nodify-avalonia/assets/5689666/3b9fe4bd-30c8-4ac7-9b4c-5f1864b83e41" width="120px" alt="Nodify.Avalonia" align="right">

[![NuGet](https://img.shields.io/nuget/v/NodifyAvalonia?style=for-the-badge&logo=nuget&label=release)](https://www.nuget.org/packages/NodifyAvalonia/)
[![NuGet](https://img.shields.io/nuget/dt/NodifyAvalonia?label=downloads&style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/NodifyAvalonia)
[![License](https://img.shields.io/github/license/bandysc/nodify-avalonia?style=for-the-badge)](https://github.com/bandysc/nodify-avalonia/blob/master/LICENSE)
[![C#](https://img.shields.io/static/v1?label=docs&message=WIP&color=orange&style=for-the-badge)](https://github.com/miroiu/nodify/wiki)

This is a direct port of [Nodify by miroiu](https://github.com/miroiu/nodify) to Avalonia. 

> A collection of highly performant controls for node-based editors designed for MVVM.

💻 A simple "real-time" calculator where each node represents an operation that takes input and feeds its output into other node's input.

> [Examples/Nodify.Calculator](Examples/Nodify.Calculator)

![Calculator](https://i.imgur.com/rup58xn.gif)

## 📥 Installation
Use the NuGet package manager to install `NodifyAvalonia`.

```
Install-Package NodifyAvalonia
```

And include Nodify resources:

```
<ResourceInclude Source="avares://Nodify/Theme.axaml" />
```

⚠️⚠️ **Please do not confuse with `Nodify.Avalonia` which is a different package** ⚠️⚠️

Avalonia version compatibility chart:

| Nodify version | Avalonia version |
|----------------|------------------|
| 6.5.0          | 11.1.0           |
| 6.2.0          | 11.1.0           |
| 6.1.0          | 11.1.0           |
| 6.0.0          | 11.1.0-beta-2    |
| 5.3.0          | 11.1.0-beta-2    |
| 5.2.0          | 11.1.0-beta-1    |

## ⭐️ Features
 
 - Designed from the start to work with **MVVM**
 - **No dependencies** other than ~~WPF~~ Avalonia
 - **Optimized** for interactions with hundreds of nodes at once
 - Built-in dark and light **themes**
 - **Selecting**, **zooming**, **panning** with **auto panning** when close to edge
 - **Select**, **move** and **connect** nodes
 - Lots of **configurable** dependency properties
 - Ready for undo/redo
 - Example applications: 💻 [**Calculator**](Examples/Nodify.Calculator)

## 😿 Unsupported Features

 - Cutting Lines (blocker: https://github.com/AvaloniaUI/Avalonia/issues/16549)

## 📝 Documentation

For the wiki please refer to the original [miroiu's Wiki](https://github.com/miroiu/nodify/wiki) since the API is identical, but please report bugs here. However, if you find a bug, please try to check if it also occurs in the original WPF's version.

> [!NOTE]  
> Avalonia.Point should be used in place of System.Windows.Point (Anchor points).

