# Xamarin boilerplate 

This will serve as a general base for creating a Xamarin Application in .NET. 
Fair warning: this is currently under development so you'll see commented code and sometimes hacks until I build this to a final level of quality. 
Another fair warning: for now I intentionally do not support registration through the app (this is a particular case I need) but in the near future I will add normal/facebook registration.
## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. Deployment notes on how to deploy this on real devices will come later.

### Running in conjunction with an API server read [RUNNING](https://github.com/CiBuildOrg/Xamarin-Starter/blob/master/RUNNING.md)

### Prerequisites

You need Visual Studio 2017 to compile this. 
You will also need a Xamarin Agent enabled machine to run the IOS builds. Connect your Xamarin Mac Agent following
instructions found [here](https://developer.xamarin.com/guides/ios/getting_started/installation/windows/connecting-to-mac/). 
## Building Android

The **App.Template.Droid** project is configured to use the latest Android platform available to you, targeting Android 6.0.0. This can be adjusted to whichever settings you prefer by right-clicking the **App.Template.Droid** project and hitting "Properties". In the "Application" tab dropdowns for changing these are available.

To run the project on an Android device, simply set the Startup Project to "App.Template.Droid" and the platform configuration to "Droid". Your list of emulators and connected devices should populate the run button. Select the device you want to deploy to and hit run to begin.

For more information, see [Deployment, Testing, and Metrics](https://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/) on Xamarin's developer guide.

## Building iOS

To build and test iOS projects on a Windows machine, you must create and connect to a Mac build agent remotely. For a guide on doing so, please see [Connecting to the Mac](https://developer.xamarin.com/guides/ios/getting_started/installation/windows/connecting-to-mac/) on Xamarin's developer guide.

Once you have created a Mac build agent, you can connect to it by navigating to "Tools > iOS > Xamarin Mac Agent" in Visual Studio and following the dialog boxes. When you have successfully linked to the agent, set the Startup Project to "App.Template.iOS" and the platform configuration to "iPhoneSimulator". Your list of available simulators should populate the run button. Select the device you want to deploy to and hit run to begin.

If you wish to deploy to actual devices, you must register an Apple Developer Account and create provisioning profiles for your project. See [Device Provisioning](https://developer.xamarin.com/guides/ios/getting_started/installation/device_provisioning/) on Xamarin's developer site for an overview of this process.

For further information, see [Deployment, Testing, and Metrics](https://developer.xamarin.com/guides/ios/deployment,_testing,_and_metrics/) on Xamarin's developer guide.

# Template Features

## Sample Views

This template project features a number of sample views that can be used as starting points for the content of your application, including a Main Menu pull-out, blog-style copy pages, e-commerce pages, forms with submission capabilities, and several examples of dependency injection and custom rendering capabilities.

Example views contain samples of [XAML-driven](https://developer.xamarin.com/guides/xamarin-forms/xaml/) pages.

For more information about creating Xamarin Forms pages, views,view models, etcetera, see the [Xamarin Forms](https://developer.xamarin.com/guides/xamarin-forms/) section of the Xamarin developer guide but also [MvvmCross](https://github.com/MvvmCross/MvvmCross).

## Deploying Builds to HockeyApp

You can deploy packages to HockeyApp via command line using curl and a HockeyApp API Token. To create a HockeyApp API Token, go to "Account Settings > API Tokens" on [rink.hockeyapp.net](https://rink.hockeyapp.net).

A sample curl command is as follows:

```shell
curl -F "status=2" -F "notify=0" -F "notes=<Patch Notes>" -F "notes_type=0" -F "ipa=@<Path to IPA or APK>" -F "commit_sha=<Commit SHA of project>" -H "X-HockeyAppToken: <API Token for your HockeyApp Account>" https://rink.hockeyapp.net/api/2/apps/upload
```

For more information see [API: Apps](https://support.hockeyapp.net/kb/api/api-apps#upload-app) in the HockeyApp API documentation.

## Mobile Unit Testing

Each unit testing project can be built as described in the above MSBuild sections.

# Troubleshooting

## Common Xamarin Issues

>The name 'XYZ' does not exist in the current context

Intellisense does not quite know how to handle Xamarin at all times. Sometimes it needs to cache things or generate code from XAML in order to know it's actually doing just fine. Try the following:

1. Build the solution twice in a row.

2. Restart Visual Studio

3. Clean and Rebuild the solution.

4. Open one of the offending view's XAML file (the .xaml file, not the .xaml.cs file) and make an inconsequential change (i.e. adding or removing whitespace) and hit Build to force Intellisense to refresh the xaml projects.

5. Right-click the Core project and Unload it, then Reload it.

For further discussion on this issue, see [this thread on the Xamarin forums](https://forums.xamarin.com/discussion/62671/initializecomponent-does-not-exist-in-the-current-context-error).

>Build action 'EmbeddedResource' is not supported by one or more of the project's targets.

Typically caused by Visual Studio not properly referencing the Xamarin build tools. Consider the following:

1. Restart Visual Studio

2. Clean and Rebuild the solution.

3. Right-click the Core project and Unload it, then Reload it.

For further discussion on this issue, see [this thread on the Xamarin forums](https://forums.xamarin.com/discussion/56559/vs-2015-errors-on-sample-projects).

>My builds hang/take a really long time.

If you're building Android or Core, make sure you're targeting the Droid or Core build configuration and don't have one of your iOs projects listed as your target build. Otherwise Visual Studio will attempt to communicate with your Mac Build Agent every time you rebuild. Alternatively, you can disconnect the Mac Build Agent in "Tools > iOS > Xamarin Mac Agent".

If you're building iOS, check the Mac Build Agent and ensure there are no popup dialogs awaiting your confirmation (i.e. allowing access to your keychain so you can use your provisioning profiles.) If it is a timing issue, unfortunately the process of compiling core binaries, transferring them to the Mac, compiling the remaining code, then transferring them back, is quite time consuming. It may be worthwhile to do iOS-specific coding and debugging directly in a Mac environment if you find it to be too time consuming.

>Visual Studio is stuck connecting to my Mac Build Agent.

Sometimes Visual Studio will get stuck trying to connect. If this happens simply close every instance of Visual Studio on your machine (make sure all devenv.exe and msbuild.exe processes are closed as well), double-check that your Mac is connected to the network, and re-launch Visual Studio.

>Android Emulator failed to start.

If you have an Android emulator currently running, close it out and kill any adb.exe and qemu-system-XXX.exe processes in your machine, then try re-running the application via Visual Studio.

>I can't deploy the Android app to my device because it says it already exists!

Sometimes your application can get in a bad state on your device and cannot be uninstalled automatically by the build system. To remedy this, on your device, go to "Settings > Apps" and manually uninstall the application. (Note that it may be named its package name instead of the actual application name.) Then re-deploy the application.

>Could not find file 'App.Template.iOS.app.dSYM.zip'.

If you are building a platform configuration that builds the iOS projects and there is no Mac Build Agent connected, it will fail silently until it tries to extract the dSYM file produced by the build. Either connect to the Mac Build Agent or choose a different configuration.

*This Mobile Template is not affiliated or endorsed by Xamarin or Microsoft.*

## Contributing

Please read [CONTRIBUTING.md](https://github.com/CiBuildOrg/Xamarin-Starter/blob/master/CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/CiBuildOrg/Xamarin-Starter/tags). 

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
