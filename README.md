# RMC Frameworks

Rivello Multimedia Consulting has multiple frameworks for MVC in Unity.


| --              | Requires MonoBehaviour?  | Lightweight?       | More Info    | Created    | Updated    |
|-----------------|--------------------------|--------------------|--------------|------------|------------|
| uMVCS           | ✔️                      | ✔️ (Light)         | [rmc-umvcs](https://github.com/SamuelAsherRivello/rmc-umvcs/)   | 2018       | 2023       |
| Mini MVCS       | ❌                       | ✔️ (Even Lighter!)  | See Below    | 2023   | 2023   |



# RMC Mini MVCS

Mini MVCS is a custom Unity library framework embracing the [MVCS architecture](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller).

- One of many solutions for organizing a Unity project efficiently. It may or may not be the best solution for you.
- Features few classes and a flexible pattern. Following the conventions of MVCS requires discipline as the system is purposefully light and flexible. For example the a model instance can access another model instance, but it is recommended not to do so.
- The ibrary has no requirement on **MonoBehaviour**. Using MonoBehaviours is optional.




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


# Table Of Contents

- [Install](#install)
  - [via Git URL](#via-git-url)
  - [Tests](#tests)

<!-- toc -->

## Install

### via Git URL

Open `Packages/manifest.json` with your favorite text editor. Add following line to the dependencies block:
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

### Import Samples
To see clear use cases, import and run the samples:

1. Open Unity and then open `Window > Package Manager`
1. Select `RMC Mini MVCS` 
1. Select Samples and import

Created By
=============

- Samuel Asher Rivello 
- Over 23 years XP with game development (2023)
- Over 10 years XP with Unity (2023)

Contact
=============

- Twitter - <a href="https://twitter.com/srivello/">@srivello</a>
- Resume & Portfolio - <a href="http://www.SamuelAsherRivello.com">SamuelAsherRivello.com</a>
- Git - <a href="https://github.com/SamuelAsherRivello/">Github.com/SamuelAsherRivello</a>
- LinkedIn - <a href="https://Linkedin.com/in/SamuelAsherRivello">Linkedin.com/in/SamuelAsherRivello</a> <--- Say Hello! :)




