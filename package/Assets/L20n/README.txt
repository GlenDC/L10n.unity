-----
L20n - v1.0
---

table of content:

    1. Introduction
    2. How to use this plugin?
    3. FAQ
    4. License

-----
1. Introduction
---

This Unity plugin is developed by Glen De Cauwsemaecker (me).
You can reach me at contact@glendc.com for any questions and other inquiries.
I'm also available for hire in case you need consultancy
related to localization of your game(s) using this technology.

This package is part of the bigger L20n For Games Project
and is the first module of that project.
The project's main goal is to make great translations for game possible and easy.
It's based on the Proof Of Concept L20n Design document by Mozilla.

The design document for the L20n For Games language
(based on that original Proof of Concept Design)
can be found at: https://github.com/GlenDC/L20n/blob/master/design/l20n-specs.md

This Unity plugin exists out of the L20nCore library, which is developed in C#,
and is open source available at https://github.com/GlenDC/L20n.cs
I made and maintain that library, just as I made this plugin.
It's made open source because I'm a believer of open source technology
and its benefits to the world we all share.

The library separate, and can be used in any project
that uses the .NET environment. It's developed using the TDD principle
and should be quite stable already. On top of that it means that any feature
bug fixes or extensions won't break any previously tested and proven features.

I would appreciate if you do not use the core library standalone
within the Unity environment, as I do need to make a living as well.

The second part is the actual Unity-specific wrapper, which consists out of
a static class `L20n` and some components. Translations can be made purely
via code by using the `L20n` class, or by using one of the available
Text Components. Take a look at the FAQ in case you need some extra components.

-----
2. How to use this plugin?
---

You can see all the different parts of this plugin in action
by taking a look at the `L20n/examples/demo` project.

What follows is a more technical description.

    ----------------------------------------------------------------
    [Static Class]                                              L20n
    ----------------------------------------------------------------
    This is the main point and can be used to make translations
    directly from code using the `Translate` methods.

    In case you would prefer you could also Initialize the L20n
    Plugin yourself by calling the `Initialize` method.
    I would however recommend using the L20nSettings component OR prefab.

    If preferred you could translate your entire game,
    just by using this static class, it's up to you really.

    ----------------------------------------------------------------
    [Prefab]                                            L20nSettings
    ----------------------------------------------------------------
    An empty object with the `L20nSettings` component attached to it.
    This can be dragged into your startup/load screen and will allow
    you to initialize the L20n plugin with ease.

    ----------------------------------------------------------------
    [Component]                                         L20nSettings
    ----------------------------------------------------------------
    A self-destructing component that can be used to handle the
    initialization of L20n at startup for you.

    In its editor you can set the manifest resource path
    and other settings. You can use this component in every scene,
    this way you don't always have to start from the actual loading screen.

    ----------------------------------------------------------------
    [Component]                               L20nSubmitLocaleAction
    ----------------------------------------------------------------
    A component that can be used to attach to a UI button object.
    Its `OnSubmit` method should in that case be assigned as
    a submit action.

    ----------------------------------------------------------------
    [Component]                             L20nUIText, L20nTextMesh
    ----------------------------------------------------------------
    The provided components for plugging in translations straight
    into your text fields all from within the editor of this component.

    ----------------------------------------------------------------
    [Component]                         L20nUIImage, L20nUIRawImage,
                                   L20nAudioSource, L20nMeshRenderer
    ----------------------------------------------------------------
    The provided components for localizing sprites, textures,
    audio and materials. These components are more experimental,
    but can be used in production and can easily be used for inspiration
    for custom L20n components of your own.

    Developing your own L20nComponents could be as easy
    as duplicating existing L20n Components and modifying it
    to suite your needs. More information available in the FAQ.

More in-depth information can be found as code-comments.


-----
3. FAQ
---

> Help I got stuck!

    Send a mail to contact@glendc.com and I will try to help you ASAP.

> I have a feature request!

    Send a mail to contact@glendc.com and I will consider it.
    Or simply leave it as a comment,
    in the same section where you gave a review? :)

> I just downloaded your plugin, what now?

    As a first step I would recommend you to open `L20n/examples/demo`
    and play through it. This will give you a quick overview of what is
    possible and what you can expect.

    After that I would recommend opening the Localize (.byte) files.
    These contain the actual localizations used in the demo.
    You can find them under `Resources/L20n/examples/demo`.

    After that you are ready to start. It takes a while to get used to it
    as it means that the relationship between developer and translator shifts.
    The translator has more power, but more communication might be necessary.

> I'm using some custom GUI plugin,
  how can I localize my text with a component?

    This is easy, and can be done in following steps (1 to 5 minutes):

        1. Duplicate an L20n textComponent (e.g. `L20nUIText`);
        2. Name it appropriately and point to the correct component type;
        3. Modify the `SetText` method body appropriately;
        4. Modify the names of the CustomEditor Unity Editor Code;

> Is syntax highlighting available for the L20n (.byte) files?

    There is a plugin available for Atom, simply look for `l20n`.
    For other editors I have no idea. Feel free to send requests
    for other editors and I might develop the ones with enough interest,
    for free.

> Is there no simpler way to add/modify translations?

    A free-to-use editor is in development and will be released
    in the near future.

-----
4. License
---

This project: Licensed under the Apache License, Version 2.0.
