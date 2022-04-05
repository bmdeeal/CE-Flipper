#!/bin/bash
#2022 B.M.Deeal
#this script distributed under CC0

#TODO: way to pass default arguments (could just tell the user to pass "" for anything they don't want to think about, or pass _ to it -- I like _ more, personally)
#TODO: rewrite to non-positional arguments (TODO2: at some point I had made a template for handling that, where is it?)

#show help
#this also stops rogue arguments from being passed to ffmpeg or imagemagick
if [ $# -eq 0 ] || [ $# -gt 5 ] || [[ "$1" =~ ^-.* ]] || [[ "$2" =~ ^-.* ]] || [[ "$3" =~ - ]] || [[ "$4" =~ - ]] || [ "$1" == ""  ]
then
	echo "ce-flipper-conv.sh -- convert a video to CE-Flipper format"
	echo "2022 B.M.Deeal."
	echo ""
	echo "This script will convert an ffmpeg compatible video file into a folder of .bmp images with a .anim file."
	echo "The images will be in 'ce-flipper-output/'."
	echo "Use at own risk, almost certainly contains bugs!"
	echo "Requires ffmpeg and imagemagick."
	echo ""
	echo "usage:"
	echo "  ce-flipper-conv.sh video-file [dither mode] [framerate] [gamma] [color|color1|gray|bw]"
	echo ""
	echo "example:"
	echo "$ ce-flipper-conv.sh myvideo.webm o4x4 3 1 color"
	echo ""
	echo "By default, the dither mode is o8x8. This is passed directly to imagemagick."
	echo "To see a list of supported dither modes, run the command:"
	echo "  convert -list threshold"
	echo ""
	echo "Framerate is 3 by default. Use 0 to extract all frames."
	echo "Gamma is 1 by default. Above 1 is brighter, below 1 is dimmer."
	echo "To enable color, simply type the word 'color' as the 5th argument."
	echo ""
	exit 0
fi

#image settings
dither="${2:-o8x8}"
gamma="${4:-1}"

#construct filenames
fn="$1" #name of file
fn_base="$(basename -- "$fn")" #name of file without path
fn_ext="${fn_base##*.}" #extension of file
fn_strip="$(basename "$fn" ".$fn_ext")" #filename without extension
fn_dir="$(dirname "$fn")" #directory of file
out_dir="$fn_dir/ce-flipper-output" #output directory for script
framerate="${3:-3}"

#handle color
mono_flag="-monochrome"
colors="2"

#enable color if specified
if [[ "$5" == "color" ]] || [[ "$5" == "colour" ]]
then
	mono_flag=""
	colors="16"
	dither="$dither,3"
fi

#1-bit per channel 3-bit color
if [[ "$5" == "color1" ]] || [[ "$5" == "colour1" ]]
then
	mono_flag=""
	colors="16"
	dither="$dither"
fi

if [[ "$5" == "gray" ]] || [[ "$5" == "grey" ]]
then
	mono_flag="-colorspace Gray"
	colors="16"
	dither="$dither,5"
fi


#set dumping all frames
if ! [ "$framerate" == "0" ]
then
	framecmd="-r $framerate"
fi

#generate output folder
mkdir "$out_dir"
if [ -d "$out_dir" ]
then
	#make flag file for ce-flipper
	touch "$out_dir/$fn_strip.anim"
	#convert video to .png
	#if ! ffmpeg -i "$fn" -r 3 "$out_dir/$fn_strip.anim-%5d.png"
	if ! ffmpeg -i "$fn" $framecmd "$out_dir/$fn_strip.anim-%5d.png"
	then
		echo "error: ffmpeg could not convert '$1'!"
		exit 1
	fi
	#convert .png to .bmp
	#would just use normal error-diffusion dithering but imagemagick produces particularly ugly results for monochrome images (contrast doing it in GIMP, which looks quite nice)
	#CE-Flipper doesn't need to use 1-bit .bmp files, but my device is grayscale (and slow) so it works better with them
	cd "$out_dir"
	for f in *.png
	do
		echo "generating $(basename "$f" ".png").bmp"
		convert "$f" -gamma "$gamma" -resize "240x120>" +dither -ordered-dither "$dither" $mono_flag -colors "$colors" "BMP3:$(basename "$f" ".png").bmp"
		rm "$f"
	done
else
	echo "error: could not make '$out'!"
	exit 1
fi
