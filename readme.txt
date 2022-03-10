CE-Flipper - a really, really basic animation program for Windows CE.
(C) 2022 B.M.Deeal <brenden.deeal@gmail.com>

===

CE-Flipper is distributed under the ISC License, see license.txt for details.
CE-Flipper was developed under Visual Studio .NET 2003 running on Windows 2000 and tested devices running CE2.0 and 2.11 with the .NET Compact Framework installed.

===
About:

CE-Flipper plays back low framerate flipbook-style animations on a device with .NET Compact Edition.

This program was written for a particularly slow handheld system and is a quick and dirty hack.
Faster CE devices can just use a real video player like TCPMP and real video formats.
In addition, you can convert image sequences like used in CE-Flipper to a real animation or video format with ffmpeg.

The file format is quite simple, consisting of a set of files:
* a .anim file (eg, "my-anim.anim"). This file contains no information inside and is just used to identify an image set as an animation.
* a set of .bmp format images (eg, "my-anim.anim-00120.bmp")

All files must be together in the same folder. The drawing area is 240x120 -- larger images will be cropped.

===
Installation:

Simply copy ce-flipper.exe to your Windows CE device.
If you are using a device running CE2.x, you must rename the ce-flipper.exe to ce-flipper.net, or else it will not run.
Your system must have the .NET Compact Framework installed to run CE-Flipper.
If you are running CE 4.1 or later and do not have it, you can download it from here:
	https://www.microsoft.com/en-us/download/details.aspx?id=24690

If you are running CE 2.0 to 4.0, you can compatible versions on HPCFactor:
	https://www.hpcfactor.com/scl/search.asp?freetext=.net+compact+framework&Search=Search&title=true
<TODO: find another link/re-host the files myself -- software from HPCFactor is no longer downloadable for new users>

A device with a newer version of Windows CE is highly recommended, although CE-Flipper's usefulness rapidly decreases due to a newer device being able to play back video or .gif animations.

Do not attempt to run CE-Flipper on a desktop Windows system, as the program will freeze when attempting to load a file.
It wouldn't be too hard to provide a version of CE-Flipper for desktop Windows, but the current .exe targets only the .NET Compact Framework.

===
Usage:

Go to File > Load to select a .anim file.
File > Play will begin playback from the start.
By default, animations run with about 330ms of delay between frames. This can be changed in the Options menu. You can also enable looping of the animation in Options. Timing is not precise, and on a slower device, you may want to select the 110ms fast speed.
File > Stop will stop the animation.
File > Exit will end the program.

If CE-Flipper does not open, and you have .NET Compact Framework installed, try moving the application to a different location. In addition, try plugging in your device.

===
Creating files:

A Bash shell script is provided to convert video files from (nearly) any format that ffmpeg supports. The script requires ffmpeg and imagemagick to be installed.
I wrote the script using WSL on Windows 10 running Ubuntu 20.04.
The script will generate 1-bit monochrome .bmp files.
Use at your own risk, it was very slapped together and might clobber some of your files if you're unlucky. You'll probably want to run it in a folder with nothing but your target video in it.

You can also manually create your own animations.
Create an empty file and call it filename.anim (you can replace filename with whatever you want, just keep it consistent).
Then, create a sequence of 240x120 (or smaller) images in Paint, each named filename.anim-00001.bmp, filename.anim-00002.bmp, filename.anim-00003.bmp, etc, etc.
Some utilities save .BMP files that CE-Flipper will not be able to display. Re-saving them in MS Paint should fix this.

Up to 99999 frames are supported -- possibly higher, but this has not been tested.

===
Some musings:

I have a handful of old Windows CE systems, and I wanted to make them more useful. They're never going to be as useful as a modern system of any variety, mostly due to their extremely low speed, but one thing that stands out is the relative lack of software, especially since a lot of the shareware that was available can no longer be purchased, so there are only crippled trial versions around.

I had the idea to maybe get some animation playing back on one of my B/W CE systems. It's slow, but it's also the most fun machine for me to use because of how clunky and limited it is.
Really, retrocomputing is like that.

That particular system has CE2.0 on it, and .NET on CE2.0 has some major problems. In fact, if the device is not plugged in, CE-Flipper refuses to start, which is something I only found out as I was finishing up.
I have absolutely no clue as to why this is the case, and my only guess is that something on startup is timing out (the machine cuts its speed in half when on battery power).
The machine fails to run a lot of .NET CF programs, so I'm glad to say I've written one that works on it, even if only under certain conditions.

My CE2.11 devices are quite a bit nicer (especially my Jornada 680), although I still hadn't gotten video on them. They run CE-Flipper quite well.
Still, because .NET Compact Framework on anything below CE 3 is a bit of a total hack, even that machine has a few issues with it. Namely, running it from \ce-flipper.net does not work, it has to be in a subfolder or on a memory card.
At least it has far fewer problems running .NET CF applications overall.

If I was willing to trudge through writing WinAPI code for CE... I probably would have never written this. The standard CE development tools are deeply unpleasant, while writing a C# and WinForms program is easy and fun.
It would probably be faster and more reliable, but this went from "vague idea" to "basically working" in like two hours, most of which were spent looking up ffmpeg and imagemagick commands.
Still, I may end up writing another player entirely if the issues this one has on the intended machines grow to be too much of a problem.

The code is a bit messy because Visual Studio 2003 lacks a bunch of features that later versions do that help a lot, like easy renaming of functions and variables. VS2003 isn't vastly different from using VS2019, but it is missing a lot of things I'm used to.

I hope this is useful to someone other than me. At some point, I might do the less than ten minutes of work it would be to get this running on desktop .NET, saving this program from absolute obscurity, although there's really no point in animating like this vs just opening up GIMP, drawing a frame for each layer, and saving as an animated .gif when you're done there.

--B.M.Deeal
