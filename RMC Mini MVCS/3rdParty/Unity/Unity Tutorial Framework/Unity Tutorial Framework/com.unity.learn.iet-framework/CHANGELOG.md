# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this package adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.1.3] - 2022-12-09
### Fixed
- Fixed issue where RichTextParser was incorrectly validating HTML as XML

## [3.1.2] - 2022-12-01
### Fixed
- Fixed issue where Maximizing a view breaks the TutorialWindows.
- AssetStore links will no longer go through Unity authentication processes. This previously prevented the asset store from being able to be opened through direct links.

## [3.1.1] - 2022-08-30
### Added
- Added an error log when a folder without a Translator.cs file is selected as the target folder of content localization files
- Added support for dynamic rendering of tutorial page's paragraphs. This allows tutorial pages to have multiple paragraphs of the same type
- Tutorials can now have a "Completion dialog" which is displayed whenever a user completes the tutorial and goes back to the Table Of Content
- Added live authoring of the title of the Welcome/Completion Dialog window
- When going "back", criteria of the current page are checked again. This allows for redoing instructions of previous tutorial steps, assuming non-destructive operations have been performed since the start of the tutorial.

### Changed
- The minimum version of the editor compatible with the package is now 2020.3
- IET no longer creates the InitCodeMarker files to determine whether to show the welcome dialog or not. A project settings, editable from the "Preferences" menu, is used instead.
- Many public APIs have been marked as Obsolete. When you update to this version, the console will warn you about which APIs to use instead. APIs that became obsolete in this version will cause compilation errors in the next major version of IET, and will be removed completely later on.

### Fixed
- Fixed Criteria being still checked even if a tutorial was quit, slowing the editor down
- Fixed Criteria continuing checking while not testing, slowing the editor down
- Fixed tutorial masking settings-related inspector being broken in Unity 2021 LTS
- Fixed missing arrow icon in uncompleted tutorial instruction paragraphs
- Fixed "Object reference" inspector field being of the wrong size in Unity 2021 or higher
- Fixed live authoring of "Next" & "Done" buttons of Tutorial Pages not working
- Fixed "404 not found" warning logs appearing at project startup
- Fixed tutorial completion status not being properly loaded at project startup
- Fixed content localization files not appearing immediately after their generation
- Fixed live authoring of content localization files not working until a domain reload was triggered
- Fixed performance issues caused by unneeded calls to InternalEditorUtility.RepaintAllViews(), which caused the whole editor's UI to be redrawn
- Fixed performance issues caused by unnecessary redraws of the making/highlighting. The masking logic has been optimized to avoid redrawing of masking / highlighting when it is not needed
- Fixed masking data being constantly recalculated even when a tutorial wasn't running
- Fixed Tutorial URLs not being opened if they didn't start with http:// or https:// and didn't have at least a 3rd level domain in the url
- Fixed TutorialPage's "URL that points to local resource/folder/file" feature not working

## [2.2.2] - 2022-06-02
### Fixed
- Fixed errors with masking settings on Unity 2021 LTS
- Fix from .net core 2.1 API to 2.0, documentation cleanup
- Clean up RTP with a callback, clear changelog
- Fix to naming and scope issues
- Fixed missing support for relative file links in Rich Text Parser

### Changed
- Documentation and some cleanup


## [2.2.1] - 2022-02-23
### Fixed
- Fixed one test failing when running in the Unity Editor's test environment

## [2.2.0] - 2022-02-14
### Added
- Added `Package Installed` Criterion
- Added a possibility to specify which element(s) (the last, the first, all of them) is/are chosen for unmasking if multiple elements match the chosen selector.
- Added text wrapping for "narrative description" and "instruction description" fields of the Inspectors of tutorial pages

### Changed
- Documentation: Added PackageInstalledCriterion explanation

### Fixed
- Fixed masking and highlighting not refreshing when hierarchy or project window content changes
- Fixed "Cannot save invalid window <window> (Unity.Tutorials.Core.Editor.TutorialModalWindow) to layout." warning message appearing when a button of the welcome dialog was used to trigger a layout reload
- Fixed unmasking not working properly for the next 1st time when switching tutorials without reloading the Editor's layout

## [2.1.2] - 2022-01-17
### Changed
- Updated version number.

## [2.1.2-pre.2] - 2022-01-13
### Fixed
- Support for HTML character codes in Rich Text Parser (`&amp;` etc.).
- Fixed how UnityEditor.UIElementsModule is fetched in UIElementsHelper for latest editor versions.

## [2.1.1] - 2021-11-02
### Changed
- Minor documentation update.

## [2.1.1-pre.2] - 2021-11-01
### Fixed
- Fixed `BuildStarterCriterionTests.CustomHandlerIsInvoked_IsCompleted` test failing when the build target is not available.

## [2.1.1-pre.1] - 2021-10-28
### Fixed
- Fixed possibility to horizontally scroll tutorial content when there were one or more line breaks.
- Fixed `BuildPlayer_Succeeds` test failing when the build target is not available.

## [2.1.0] - 2021-10-13
### Changed
- When traversing back to an already completed tutorial page, the masking settings of the page are reapplied if the page doesn't have completion criteria set.
This new behavior improves the tutorial authoring experience and allows a tutorial user to reobserve pages in "a tour of the UI" type of tutorials.
- When applying masking, do not not throw an exception if unmasked view's **Editor Window Type** or **View Type** is explicitly set to **None**, meaning, the masking is likely not fully configured yet.

### Fixed
- Fixed auto-completion of `SceneViewCameraMovedCriterion` not working.
- Fixed unresolved `SerializedType` values being shown incorrectly as **None**. These values are now shown as **Not Found** with a red background in the Inspector.
- Fixed `SerializedType` values that were resolved but the resolved type had a different assembly-qualified type name than the original name being shown incorrectly as **None**. These values are now shown with a yellow background in the Inspector.
- Fixed NRE when auto-advancing a tutorial page with video on Unity 2021 and newer.

### Removed
- UI: Removed drop shadows from title and subtitle of tutorial category/project cards and tutorial window's header as the `text-shadow` USS property is not supported on Unity 2020 and older.

## [2.1.0-pre.1] - 2021-09-28
### Added
- Added support for UI Toolkit masking and highlighting. **Pick Visual Element** functionality can be used to easily pick the wanted visual elements.
- Added multiple scene support for tutorials: the first element of **Scenes** list is considered to be the main scene and the rest of scenes are loaded additively.
- Added tutorial start-up scene management options: as a new option, it's possible to reuse the currently active scene(s).
- `Tutorial`: Added getters and setters for all serialized fields.
- Added unit tests back to the package.
- Rich text parser: Added `<style>` tag support, making it possible to set a text block to any style class.
- UI: Added drop shadows to title and subtitle of tutorial category/project cards and tutorial window's header.
- Warn user about `TutorialProjectSettings.RestoreDefaultAssetsOnTutorialReload` behavior in all tutorial asset editors if authoring tools are not present.
- Added sanitization for tutorial assets' text content so that unprintable control characters, for example, a carriage return, are removed automatically.
- Scripting API: Made `TutorialModalWindow` part of the public API of the package. This class can be used to implement welcome/closing dialogs for the tutorial project.

### Fixed
- Fixed misplaced unmasking of UI controls in floating editor windows on Unity 2021.2.0.
- UI: Disabled horizontal scrollbars in all windows.
- UI: Fixed unwanted offset in the interactable area of **Back to previous view** button.
- Fixed original scenes not being restored correctly when exiting a tutorial which contained multiple scenes.
- Fixed original scenes not being restored correctly when exiting a tutorial, when its original state contained multiple scenes.
- Fixed memory leak errors ("A Native Collection has not been disposed...") that occurred on Unity 2021.
- Fixed undo/redo of tutorial and tutorial page editing not being reflected in the Inspector and **Tutorials** window immediately.
- Fixed editing of active tutorial not being reflected in **Tutorials** window after an assembly reload.

## [2.0.0] - 2021-07-05
### Added
- Added setters for `TutorialWelcomePage` class's properties.

### Changed
- **Breaking change**: Changed `TutorialWelcomePage` class's events to use `UnityEvent` class instead of the standard C# `event` implementation.
- UI: Disabled horizontal scrollbars in the **Tutorials** windows altogether.

### Fixed
- Fixed masking and highlighting on Unity 2021.2.0b.
- Fixed occasionally appearing half-window-sized vertical scrollbar in **Tutorials** windows on Unity 2021.2.

## [2.0.0-pre.6] - 2021-06-17
### Added
- Added support for tutorial categories by making it possible for `TutorialContainer` assets to refer to each other.
- `TutorialContainer`: Added **Order In View** and **Parent Container** fields, allowing creation and ordering of tutorial categories.
- Added **Use default editors for editing tutorial assets** preference, disabled by default.
- `TutorialPage`: Added `Showing`, `Shown`, and `Staying` events.
- Added `CommonTutorialCallbacks` assets from the Tutorial Authoring Tools package.

### Changed
- Disallowed dragging of **Sections** of a tutorial container freely on Unity 2020.2 and newer; use the **Order In View** to reorder the sections.
- UI: Clarified tutorial page's completion criteria settings by moving **Auto Advance** field next to other completion criteria settings.
- Tutorial project cards are now sorted using **Order In View** values instead of **Title**.
- **Order In View** values of tutorial container sections are no longer reassigned automatically, allowing users to mix sections and tutorial categories easily.
- UI: Renamed **All Tutorials** button to **Back to overview**.
- Removed tutorial page editor's **Force default Inspector** option, superseded by the **Use default editors for editing tutorial assets** preference.
- All tutorial assets can be edited without having Tutorial Authoring Tools present.
- `TutorialPage`: Deprecated `OnBeforePageShown`, `OnAfterPageShown`, `OnTutorialPageStay`, and `OnBeforeTutorialQuit` events.
These events are superseded by the new `Showing`, `Shown`, `Staying` and `Tutorial.Quit` (added in 2.0.0-pre.5) events.
- **Breaking change**: Made `TutorialContainer.Section.TutorialCompleted`, `LoadState()`, and `SaveState()` internal.

### Removed
- **Breaking change**: Removed `TutorialPage.Guid()` function.

### Fixed
- Fixed tutorial cards to immediately reflect to changes of tutorial's **Progress Tracking Enabled** field.
- UI: Fixed disappearing back-arrow when having too much text in the tutorial container header.

## [2.0.0-pre.5] - 2021-05-18
### Added
- Added support for multiple `TutorialContainer` assets within a project, allowing multiple set of tutorials ("tutorial projects") for the user to choose from.
- Checkbox to enable progress tracking in tutorials. Enabling progress tracking generates a GUID for the tutorial's **Lesson Id** automatically.
- UI: Added **Select Container** button to the authoring toolbar. This button selects the currently effective tutorial container in the project.
- UI: Added **Tutorials** > **Welcome Dialog** menu item for accessing the possible welcome dialog of the project conveniently at all times.
- UI: Added custom editor views for `TutorialProjectSettings` and `TutorialStyles` assets.
- `Tutorial`: Added `Quit` event.
- `Criterion`: Added `Completed` and `Invalidated` events.
- `TutorialPage`: Added `CriteriaValidated`, `MaskingSettingsChanged`, and `NonMaskingSettingsChanged` events.

### Changed
- Tutorial logic: Tutorials are not shown as completed until the criteria of its last page are completed.
- UI: Tutorial cards do not have completion markers unless progress tracking is enabled.
- UI: Improved tutorial page's **Custom Callbacks** authoring view by making it more compact.
- UI: Cleaned up and restructured the **Tutorials** menu, authoring-related items can be now found under the **Tutorials** > **Authoring** submenu.
- UI: Changed authoring toolbar's buttons to use icons instead of text labels.
- Scripting API: Made `TutorialManager` part of the public API of the package.
- **Breaking change**: Split `UnityEngine.InteractiveTutorialsFramework` assembly into `Unity.InternalAPIEditorBridge.007` and `Unity.InternalAPIEngineBridge.007`.
Note that these assemblies are only for internal use and they should not be depended on.
- **Breaking change**: Made `LocalizationDatabaseProxy`, `GUIViewProxy`, and `POFileUtils` classes internal.
- **Breaking change**: Renamed `Whitespace` to `WhiteSpace` in `StringExtensions` and `StringExt` function names.
- **Breaking change**: Changed tutorial assets' events to use the `UnityEvent` class instead of the standard C# `event` implementation. 
- **Breaking change**: Made the following functions internal or private:
  - `Tutorial`: constructor
  - `TutorialPage`: `OnPageCompleted`, `RaiseOnBeforePageShownEvent`, `RaiseOnAfterPageShownEvent`, `UpdateFutureObjectReferenceName`, and `ResetUserProgress`
  - `UserStartupCode`: `IsInitialized` and `SetInitialized`
- **Breaking change**: Renamed `Tutorial` class's `TutorialInitiated` to `Initiated` (event), `TutorialCompleted` to `Completed` (event), and `Completed` to `IsCompleted` (get-property).
- **Breaking change**: Renamed `Criterion` class's `Completed` property to `IsCompleted`.
- **Breaking change**: Renamed the following event-raising functions:
  - `Tutorial.RaiseTutorialModifiedEvent` to `RaiseTutorialModified`
  - `TutorialPage.RaiseTutorialPageMaskingSettingsChangedEvent` to `RaiseMaskingSettingsChanged`
  - `TutorialPage.RaiseTutorialPageNonMaskingSettingsChangedEvent` to `RaiseNonMaskingSettingsChanged`
  - `TutorialWelcomePage.RaiseModifiedEvent` to `RaiseModified`
  - `TutorialContainer.RaiseModifiedEvent` to `RaiseModified`
- **Breaking change**: Renamed `TutorialWindowMenuItem` to `MenuItems`.

### Removed
- Omitted tests from the package.
- Documentation: Excluded `*.Tests` and `SerializableCallback` namespaces from the Scripting API documentation.
- UI: Removed the searchable menu that was added in 2.0.0-pre.3 in Unity 2019, as the menu implementation was causing serialization issues for the tutorial assets.
- **Breaking change**: Removed `ScriptableObjectUtils` class.
- **Breaking change**: Removed `Tutorial.TutorialPagesModified` event and `RaiseTutorialPagesModified` function, superseded by the `Modified` event.
- **Breaking change**: Removed `Tutorial.SkipTutorialBehavior`.
- **Breaking change**: Removed `TutorialProjectSettings.StartupTutorial`. This functionality can be now implemented by using `TutorialManager.StartTutorial()` if wanted.

### Fixed
- Authoring: Fixed `TutorialCallbacks.asset` not being guaranteed to be created in the same folder as `TutorialCallbacks.cs` when using the **Create Callback Handler** button.
- UI: Fixed the **Next** button's state (enabled/disabled) to match the completion criteria of a tutorial page in cases where the criteria are invalidated after the initial completion.
- UX: Fixed unnecessary window layout restoring when when quitting a tutorial which did not have a window layout set.
- Fixed "Editing of Tutorial Pages no longer works on pages that have a Criterion" (case [1332176](https://fogbugz.unity3d.com/f/cases/1332176/))
- Fixed `OnBeforePageShown` and `OnAfterPageShown` events not being raised for the first page of a tutorial when starting the tutorial.

## [2.0.0-pre.4] - 2021-03-10
### Changed
- Moved UI image files from `Editor/Resources` to `Editor/UI/Images`.

### Fixed
- Fixed addition of a new tutorial page completion criterion resetting all existing completion criteria on the same page.
- Documentation: Fixed all installation instructions to use "-pre.X" postfix.
- UI: Fixed unwanted horizontal scroll bar appearing on tutorial pages on Unity 2021 by disabling the horizontal scroll bar altogether.

## [2.0.0-pre.3] - 2021-03-02
## Added
- UI: Added **Show simplified type names** preference which affects the appearance of `SerializedType` fields.
This preference can be found under **Preferences** > **In-Editor Tutorials**.
- Rich text parser: Added word wrapping support for CJK characters.
- Rich text parser: Added support for `<wordwrap>` tag that can be used to force word wrapping even when Chinese, Japanese or Korean is detected.
- Rich text parser: leading whitespace can be used as indentation.
- Documentation: package documentation/manual added.

### Changed
- **Breaking change**: `TutorialContainer`'s `ProjectName` renamed to `Title`, old `Title` renamed to `Subtitle`.
- **Breaking change**: `TutorialContainer.Section`'s `AuthorizedUrl` removed, `LinkText` renamed to `Metadata`, `SessionStateKey` made `internal`.
- **Breaking change**: Renamed `SceneObjectGUIDComponent` to `SceneObjectGuid` and `SceneObjectGUIDManager` to `SceneObjectGuidManager`.
- Dependencies: Settings Manager dependency updated to 1.0.3.
- UX: Show a warning in the Console if the user is not signed in.
- UX: **Show Tutorials** menu item simply focuses **Tutorials** window in all cases, also when a tutorial is in progress.
- UX: If `TutorialContainer.ProjectLayout` has a layout without **Tutorials** window, the window is now shown as a free-floating window instead of not showing it at all.
- UI: `SerializedType` fields can now be edited using a searchable menu. This affects the following tutorial page settings:
  - The **Editor Window Type** setting of **Unmasked Views** when its Selector Type is set to **Editor Window**
  - The **View Type** setting of **Unmasked Views** when its Selector Type is set to **GUI View**
  - The **Type** setting of **Completion Criteria**
- UI: Abstract `Criterion` classes, which cause errors when selected, are no longer shown in the Inspector.
- Documentation: updated _Known issues_ section.

### Removed
- **Breaking change**: `TutorialProjectSettings.UseLegacyStartupBehavior` field removed.
- **Breaking change**: `ParagraphType` enumeration's `UnorderedList`, `OrderedList`, and `Icons` removed.
- **Breaking change**: `Tutorial.ExitBehavior` removed as obsolete.
- **Breaking change**: Removed `TriggerTaskCriterion`, `*CollisionBroadcaster*`, `IPlayerAvatar`, and `SelectionRoot` classes.
- Dependencies: Removed Physics and Physics2D dependencies from the package.

### Fixed
- Initialize `TutorialPage`'s `NextButton` and `DoneButton` to "Next" and "Done" respectively.
- Fixed null reference exception and **Tutorials** window being broken when updating the package.
- Fixed having **Auto Advance** option enabled on the last page of a tutorial making the first page of the tutorial to be skipped upon a rerun.
- Fixed **Scene(s) Have Been Modified** dialog being shown multiple times when **Cancel** or **Don't Save** was chosen.
- Fixed **Scene(s) Have Been Modified** dialog not being shown while having unsaved changes and quitting a tutorial.
- Fixed null reference exception when tutorial ended by auto-advancing while having unsaved changes.
- Fixed null reference exception when **Inspector** was docked as a child of another view and **Tutorials** window was shown using the auto-docking mechanism.
- Authoring: Fixed window layouts not being preprocessed until the project is restarted.
- Authoring: Fixed **Tutorials** > **Genesis** > **Clear all statuses** to clear the tutorial cards' completion markers correctly.
- Authoring: Fixed "HTTP/1.1 401 Unauthorized" warning spam in the Console when the tutorial author was not signed in.
- UI: Fixed tutorial cards' completion markers not showing the correct state when the project was just opened while having **Tutorials** window visible.
- UI: Fixed tutorial card not being marked as completed when a completed tutorial was quit by clicking the **Close** (**X**) button.

## [2.0.0-pre.1] - 2020-11-17
## Added
- Documentation: All public APIs documented.

### Changed
- **Breaking change**: all public APIs reviewed; many APIs made internal and some new public APIs added.
- **Breaking change**: all public APIs are now PascalCase instead a mix of camelCase and PascalCase.
- **Breaking change**: `Unity.InteractiveTutorials` namespace rename to `Unity.Tutorials.Core(.Editor)`.
- **Breaking change**: `Unity.InteractiveTutorials.Core` assembly renamed to to `Unity.Tutorials.Core.Editor`.
- **Breaking change**: `Unity.InteractiveTutorials.Core.Scripts` assembly renamed to to `Unity.Tutorials.Core`.

## [1.2.3] - 2021-03-04
### Fixed
- Fixed null reference exception on Unity 2020.2.6 and newer (2021 does not have the problem) when editing the **Type** of **Completion Criteria**.

## [1.2.2] - 2021-01-15
### Fixed
- Null reference exception when starting a project which shows the Tutorials window by using the new window docking mechanism.
- Don't show Tutorials window when starting a project if there are no tutorials configured to be ran in the project (no `TutorialContainer` asset).
- Tutorial not auto-advancing when a tutorial instruction involved exiting Play Mode.
- Null reference exception upon returning to a tutorial page with video when the same tutorial page was exited earlier by choosing _Show Tutorials_ menu item.

## [1.2.1] - 2020-11-17
### Fixed
 - Tutorial pages without completion criteria were auto-advanced even if not configured to do so if the previous page was set to auto-advance.
 - UI: Fix the layout to make cards 100% wide when used with less content.

### Added
- Localization: added translations for CJK languages.

## [1.2.0] - 2020-11-10
### Added
- Localization: Finalize localization support for CJK languages. No translations provided yet.
- Localization: Changes to IET project translations are applied automatically, _Translate Current Project_ menu item removed as unnecessary.
- Localization: Made the UI strings of `Tutorial`, `TutorialPage` and `TutorialParagraph` localizable.
- Tutorials window can be shown by anchoring and docking next to the Inspector (new default behavior) in addition of loading a window layout containing the window (old behavior).
- Provide static `Modified` events for `TutorialWelcomePage`, `Tutorial` and `TutorialContainer`.
- Tutorial authors can now define callbacks for `OnTutorialPageStay` (called each Editor update) and `OnBeforeTutorialQuit` (called right before a user force-quits a tutorial).
- Added _Enable Masking and Highlighting_ Preference, can be found under _Preferences_ > _In-Editor Tutorials_.
- Exposed `SceneViewCameraSettings` class and enums.
- Documentation: Known Issues section added.

### Changed
- TutorialWindow no longer has hardcoded "TUTORIALS" text, instead `TutorialContainer.Subtitle` shown here.
- Deprecated `TutorialContainer.Description`, renamed `Title` to `Subtitle`, `ProjectName` will be renamed most likely to `Title` in 2.0.
- `TutorialProjectSettings.RestoreDefaultAssetsOnTutorialReload` made `false` by default.
- deprecated `TutorialContainer.Section.AuthorizedUrl`; the Unity authentication need is detected automatically from the URL.
- UI: Updated Link Card styling.
- UI: Enable word wrapping for Tutorial Card title.
- UI: Enable word wrapping for `TutorialContainer`'s header title and subtitle.

### Fixed
- Hyperlinks in tutorial pages support also to non-Unity URLs and work when the user was is not logged in.

## [1.1.0] - 2020-09-22
### Added
- Support for both Personal/Light & Professional/Dark style sheets. The styles can be customized on a per-project basis using `TutorialStyles`.
- Rich text parser: validate input, create a clickable error label for invalid input.
- It is now possible to highlight/unmask Unity Object specific UI element by referencing the object properly instead of relying solely to a name matching logic.

### Changed
- Raised the required Unity version to 2019.4.
- Hide the tutorial media header if the page has no media set.
- Unit tests are omitted from the package. Also renamed the unit test assembly in order to prevent name clashes with other internal Unity projects if the package is used as a local file package.

### Fixed
- Fixed masking and highlighting for Unity 2020.1 and newer.
- Fixed overwhelming memory usage of `VideoPlaybackManager`.
- Fixed all UI textures use the correct (GUI) texture type for better visual quality.
- `TutorialPage`: fixed null reference exception if selecting _None_ as _Media_ while having _Video_ as _Media Type_.
- Cleaned up PO files from dummy test translations.
- Fixed null reference exception spam when rerunning a newly created tutorial.

## [1.0.3] - 2020-08-26
### Fixed
- Fixed potential problems with copying of the layouts during the initial import of a project.
- Removed a duplicate error print if window layout loading fails.
- UI: Fix stretched header image and improve styling of next tutorial button.

## [1.0.2] - 2020-08-06
### Added
- Blink the Start tutorial -button on the first page.

### Fixed
- Fixed highlighting of views not working if an Instructive paragraph was present in the current TutorialPage
- Header image stretching disabled

### Changed
- Step completion checkmark (green color) only shown on steps with criteria.
- Lightened the tutorial background for better contrast
- Ensured long title texts wrap, and not overflow out of the screen
- Made instruction boxes look less like a button
- Ensured tutorial videos are not stretched
- Bold button labels
- Reduced margin between cards

## [1.0.1] - 2020-07-24
### Fixed
- Rich text hyperlinks: supporting only hyperlinks to Unity's websites. The user needs to be logged in in order for the hyperlinks to work.
- Next button was not disabled on a new tutorial page when the previous page was auto-completed.

## [1.0.0] - 2020-07-16
### Fixed
* Scene view camera's state is restored properly when exiting a tutorial after an assembly reload.
* If clicking a Switch Tutorial button while having unmodified changes in the scene, and choosing to cancel, do not return to the tutorial selection page, instead keep the current tutorial page.
* Prevent showing of Close Tutorials dialog when Play Mode is changing
* Fix tutorial window layouts not functioning after a tutorial project is restarted.
* Hide the instruction title element when it has no content.
* Fixed loading the tutorial scene twice when starting a tutorial, improving the tutorial start-up time.

## [0.5.1] - 2020-07-06
### Added
- `ArbitraryCriterion`: allows tutorial author to specify arbitrary completion criterion.
- "Force default Inspector" checkmark state in `TutorialPageEditor` is now remembered when you click on another asset.
- `TutorialPage` has now `OnBeforePageShown` and `OnAfterPageShown` events to which tutorial author can subscribe to in order perform custom code.

### Changed
- The package is now known as Tutorial Framework instead of Interactive Tutorial Core.

### Fixed
- `BuildStartedCriterion` is evaluated properly also in cases where a build is not started from default Build Settings window.
- Null reference exception when trying to view `TutorialPage` without paragraphs in the Inspector.

## [0.5.0] - 2020-06-23
### Added
- `TutorialWelcomePage`/`TutorialModalWindow`:
  - can be authored at real-time,
  - added a simple `TutorialWelcomePageEditor` with "Show Welcome Dialog" functionality,
  - rich text support, and
  - fully configurable button row.
- `LocalizableTextArea` support for `LocalizableString`s, similar to `TextArea` for `string`s.
- Allow specifying `InitialCameraSettings` in `TutorialProjectSettings`.

### Changed
- Masking is not assumed and enforced for every tutorial page, allowing to mix masked and unmasked pages in the same tutorial.
- **Breaking change**: `TutorialWelcomePage` asset refactored without backward-compatibility.
- `TutorialProjectSettings`:
  - the welcome page for the project needs to be set explicitly,
  - `StartupTutorial` is not started automatically, and
  - `UseLegacyStartupBehavior` false by default.
- Authoring: single line breaks rendered as expected instead of new paragraphs.
- Refactored and combined all the styles, removed `WelcomeDialog.uss`.
- Updated the style of the welcome dialog.
- Improvement: `SceneViewCameraSettingsDrawer` shows rotation as Euler angles instead of raw Quaternion components.
- Improvement: Save and restore SceneView's state (i.e. camera's state) when entering and exiting tutorials.
- Dependencies: update Editor Coroutines to 1.0.0.

### Fixed
- Single line breaks now make a line break and two line breaks make a paragraph.
- `TutorialModalWindow`: Fixed hiding of `HeaderContainer` if none/null image set.
- Instead of modifying the original window layout files in the project, a working copy is created and modified.
- Fixed null reference exceptions when starting a Tutorial which has no pages.

## [0.4.0] - 2020-06-02
###  Changed
- Refactor `TutorialWindow` to use UI Toolkit instead of IMGUI.
- **Breaking change**: `TutorialContainer` and `TutorialParagraph` assets refactored without backward-compatibility.
- Analytics: using `EditorAnalytics` instead of `UsabilityAnalytics` as the API for all events.
- `TutorialPage`: instead of arbitrary set of `TutorialParagraph`s, the page has a fixed set of fields. `TutorialParagraph` will be deprecated in the near future.
### Added
- `TutorialPageEditor`: a new simplified authoring view.
- Preliminary localization support.
- Analytics: added an event for external reference (e.g. link cards) impressions

## [0.3.0] - 2020-03-25
### Changed
 - Raised the required Unity version to 2019.3.
 - Removed flexible spaces surrounding a video paragraph.
### Fixed
 - UI flickering on macOS and Unity 2019.3 when a tutorial page had a video.

## [0.2.3] - 2020-02-26
### Added
 - Analytics event for clicking external references/URLs.

## [0.2.2] - 2020-02-14
### Added
 - Support for authorized URLs (Unity Connect auto-login).
 
## [0.2.1] - 2019-11-11
### Fixed
 - Do not show the Welcome dialog and load the IET window layout every time an IET project is started.
 - Fixed IET initialization when a Microgame is loaded from the Asset Store.

### Changed
 - Do not clear the description of a tutorial card when a tutorial is marked as completed.
 
## [0.2.0] - 2019-10-21
### Changed
 - `Readme` class renamed to more suitable `TutorialContainer`.
 
## [0.1.18] - 2019-10-21
### Changed
 - New single-panel approach, Readme and Tutorials are shown in the same window which is always visible.
 - Ability to save the Project window's state for the end-user when saving layouts for tutorials.
 - `Readme` class moved into `Unity.InteractiveTutorials` namespace.

## [0.1.17] - 2019-07-19
### Changed
 - Updated UI styles.

## [0.1.16] - 2019-05-15
### Changed
 - Updated warning message when the user is about to exit the tutorial.

## [0.1.15] - 2019-03-04
### Added
 - Adding the ability to unmask elements based on the name of the GUIStyle used to draw them.
 - Warning message when the user is about to exit the tutorial.

### Changed
 - When clicking on *Help > Template Walkthroughs* if the inspector window is not visible, the Inspector window will be shown

## [0.1.14] - 2019-02-12
### Fixed
 - If the user opens an Undocked window, that is not part of the tutorial, the window tabs are unmasked, so they can close or move the window.
 - Improved compatibility with old content.

## [0.1.13] - 2019-02-04
### Added
- Add support for specifying alternate EditorWindow types when configuring unmasked views
- Expand unmask region to include foldout arrow when unmasking property that is collapsed

### Removed
- Remove "Couldn't find a readme" message when there is no Readme asset in project

## [0.1.12] - 2019-01-24
### Fixed
- Fix editor entering and exiting play mode on project load
- Fix unmasked property unmasking entire window when ancestor property is collapsed

## [0.1.11] - 2019-01-17
### Fixed
- Fix 2019.1 compilation errors
- Improve invalid ScriptableObject reference workaround to always exit play mode after project load

## [0.1.10] - 2019-01-11
### Fixed
- Fix invalid CHANGELOG formatting.

## [0.1.9] - 2019-01-11
### Changed
- SceneViewCameraMovedCriterion will also complete if the user changes the camera orientation.

### Fixed
- Added work around for issue where tutorial is not loaded initial project load

## [0.1.8] - 2018-12-11
### Fixed
- Fixed build script

## [0.1.7] - 2018-12-10
### Fixed
- Fixed authoring of scene object references.
### Removed
- Remove *Window > Tutorials* menu item.

## [0.1.6] - 2018-12-06
### Fixed
- Fix AudioClip import errors.
- Fix compilation errors at build time due to incorrectly configured Assembly Definition asset.
- Fix inconsistent line endings.
- Fix CS0649 warnings.
- Fix *Help > Template Walkthroughs* menu item not finding Readme asset.

## [0.1.5] - 2018-12-04
### Fixed
- Fixed ReflectionTypeLoadException when inspecting TutorialPage
- Fixed GUI layout errors when starting tutorial from Readme asset

## [0.1.4] - 2018-12-03
### Added
- Integrated the readme asset with the Tutorials
- Ability to have more than a single Tutorial in a project
- Proper flow for users to go into and out of a tutorial
- Ability to add Images, Video to a tutorial
- New Color type added to PropertyModificaitonCriterion
- PropertyModificationCriterion has a new mode where it will complete if the user changes a property to a different value than initial
- Added option to the masking system to prevent interactions to the unmasked area
- New Criterions: FrameSelectedCriterion, MaterialPropertyChanged, ActiveToolCriterion, SceneCameraViewMovedCritertion
- Ability for Tutorials to reference each other
- "Home/Skip" button have 2 modes. Legacy will open the Hub, and CloseWindow will close the Tutorial window
- Ability to choose the name of the Tutorial Window

### Fixed
- Updated usages of obsolete APIs

### Changed
- Initial version of the in editor tutorial framework as a package.
- Contained the use of internals to a single folder.
