[![npm package](https://img.shields.io/npm/v/com.rmc.rmc-mini-mvcs)](https://www.npmjs.com/package/com.rmc.rmc-mini-mvcs)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

# RMC Architectures

Rivello Multimedia Consulting has multiple frameworks for MVC in Unity.


| --              | Requires MonoBehaviour?  | Lightweight?       | More Info    | Created    | Updated    |
|-----------------|--------------------------|--------------------|--------------|------------|------------|
| uMVCS           | ✔️                      | ✔️ (Light)         | [rmc-umvcs](https://github.com/SamuelAsherRivello/rmc-umvcs/)   | 2018       | 2023       |
| Mini MVCS       | ❌                       | ✔️ (Even Lighter!)  | See Below    | 2023   | 2023   |



# RMC Mini Mvcs Architecture - For Unity

Mini MVCS is a custom Unity library framework embracing the [MVCS architecture](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller).

- One of many solutions for organizing a Unity project efficiently. It may or may not be the best solution for you.
- Features few classes and a flexible pattern. Following the conventions of MVCS requires discipline as the system is purposefully light and flexible. For example the a model instance can access another model instance, but it is recommended not to do so.
- The library has no requirement on **MonoBehaviour**. Using MonoBehaviours is optional.

Enjoy!




## MVC Architecture For Unity - Udemy Course

Welcome to MVC Architecture For Unity!

This is the official repo for the online course.

The course gives you the training to create and maintain Unity projects which are faster to develop and easier to maintain.

COURSE TRAILER VIDEO:

<a target="_blank" href="https://bit.ly/mvc-architecture-for-unity-trailer"><img src="https://img.youtube.com/vi/ulclbvLL9A4/hqdefault.jpg" width = "350"></a>


COURSE LINK:

- <a target="_blank" href="https://bit.ly/mvc-architecture-for-unity-on-udemy">https://bit.ly/mvc-architecture-for-unity-on-udemy</a>


FEATURES:

- Rockstar content - Everything you DO need. Just the most relevant, powerful info!
- Punk-rock editing - Nothing you DON'T need. No "ums", no waiting, no fluff!


TAKEAWAY:

- After this course you will be confident to design and develop new projects with Unity and MVC


INCLUDED SECTIONS:

- Course Introduction - Set the vision and goals for the course
- Unity Overview - Review the popularity and power of Unity as a game engine
- Software Design - Gain insight on the design principles of master software developers
- Mini MVCS - Dive deep into this light, powerful architectural framework
- Sample Projects - Together we'll review 4 complete Unity projects
- Course Conclusion - Review the highlights, celebrate success, and set clear next steps
- EXTRA Content - With the foundation of MVC architecture, we'll add the power of Unit Testing and TDD


WHY WAIT?

- Avoid the common pitfalls with creating, maintaining, and scaling Unity projects
- You can't afford NOT to use Unit Testing and Test-Driven development on your projects
- Let's do this!

<img width = "400" src="https://raw.githubusercontent.com/SamuelAsherRivello/rmc-core/main/RMC%20Core/Documentation~/com.rmc_namespace_logo.png" />

# Table Of Contents

- [How To Use](#how-to-use)
- [Install](#install)
  - [Via NPM](#via-npm)
  - [Via Git URL](#via-git-url)
  - [Tests](#tests)
  - [Samples](#samples)
- [Configuration](#configuration)

<!-- toc -->

## How to use

Mini MVCS is a custom Unity library framework embracing the [MVCS architecture](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller).

- One of many solutions for organizing a Unity project efficiently. It may or may not be the best solution for you.
- Features few classes and a flexible pattern. Following the conventions of MVCS requires discipline as the system is purposefully light and flexible. For example the a model instance can access another model instance, but it is recommended not to do so.
- The ibrary has no requirement on **MonoBehaviour**. Using MonoBehaviours is optional.

Enjoy!


## Install

### Via NPM

You can either use the Unity Package Manager Window (UPM) or directly edit the manifest file. The result will be the same.

**UPM**

To use the [Package Manager Window](https://docs.unity3d.com/Manual/upm-ui.html), first add a [Scoped Registry](https://docs.unity3d.com/2023.1/Documentation/Manual/upm-scoped.html), then click on the interface menu ( `Status Bar → (+) Icon → Add Package By Name ...` ). Then enter the value from the snippet just below.

**Manifest File**

Or to edit the `Packages/manifest.json` directly with your favorite text editor, add a scoped registry then the following line(s) to dependencies block:

```json
{
  "scopedRegistries": [
    {
      "name": "npmjs",
      "url": "https://registry.npmjs.org/",
      "scopes": [
        "com.rmc"
      ]
    }
  ],
  "dependencies": {
    "com.rmc.rmc-mini-mvcs": "2.3.2"
  }
}
```
Package should now appear in package manager.


### Via Git URL

You can either use the Unity Package Manager (UPM) Window or directly edit the manifest file. The result will be the same.

**UPM**

To use the [Package Manager Window](https://docs.unity3d.com/Manual/upm-ui.html) click on the interface menu ( `Status Bar → (+) Icon → Add Package From Git Url ...` ).  Then enter the value from the snippet just below.

**Manifest File**

Or to edit the `Packages/manifest.json` directly with your favorite text editor, add following line(s) to the dependencies block:
```json
{
  "dependencies": {
      "com.rmc.rmc-mini-mvcs": "https://github.com/SamuelAsherRivello/rmc-mini-mvcs.git"
  }
}
```

### Tests

The package can optionally be set as *testable*.
In practice this means that tests in the package will be visible in the [Unity Test Runner](https://docs.unity3d.com/2017.4/Documentation/Manual/testing-editortestsrunner.html).

Open `Packages/manifest.json` with your favorite text editor. Add following line **after** the dependencies block:
```json
{
  "dependencies": {
  },
  "testables": [ "com.rmc.rmc-mini-mvcs" ]
}
```

### Samples

Some packages include optional samples with clear use cases. To import and run the samples:

1. Open Unity 
1. Complete the package installation (See above)
1. Click Unity Menu Option: `Tutorials → Reset PackageInstalled Switch` to validate dependencies
1. Open the [Package Manager Window](https://docs.unity3d.com/Manual/upm-ui.html)
1. Select this package 
1. Select samples
1. Import

## Configuration

* `Unity Target` - [Standalone MAC/PC](https://support.unity.com/hc/en-us/articles/206336795-What-platforms-are-supported-by-Unity-)
* `Unity Version` - Any [Unity Editor](https://unity.com/download) 2022.x or higher
* `Unity Rendering` - Any [Unity Render Pipeline](https://docs.unity3d.com/Manual/universal-render-pipeline.html)
* `Unity Aspect Ratio` - Any [Unity Game View](https://docs.unity3d.com/Manual/GameView.html)


Created By
=============

- Samuel Asher Rivello 
- Over 25 years XP with game development (2024)
- Over 11 years XP with Unity (2024)

Contact
=============

- Twitter - <a href="https://twitter.com/srivello/">@srivello</a>
- Git - <a href="https://github.com/SamuelAsherRivello/">Github.com/SamuelAsherRivello</a>
- Resume & Portfolio - <a href="http://www.SamuelAsherRivello.com">SamuelAsherRivello.com</a>
- LinkedIn - <a href="https://Linkedin.com/in/SamuelAsherRivello">Linkedin.com/in/SamuelAsherRivello</a> <--- Say Hello! :)


License
=============

Provided as-is under MIT License | Copyright © 2024 Rivello Multimedia Consulting, LLC




