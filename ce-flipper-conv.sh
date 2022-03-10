#!/bin/bash
#2022 B.M.Deeal
#this script distributed under CC0

#show help
#this also stops rogue arguments from being passed to ffmpeg or imagemagick
if [ $# -eq 0 ] || [[ "$1" =~ ^-.* ]] || [ "$1" == ""  ]
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
	echo "  ce-flipper-conv.sh video-file"
	echo ""
	exit 0
fi

#construct filenames
fn="$1" #name of file
fn_base="$(basename -- "$fn")" #name of file without path
fn_ext="${fn_base##*.}" #extension of file
fn_strip="$(basename "$fn" ".$fn_ext")" #filename without extension
fn_dir="$(dirname "$fn")" #directory of file
out_dir="$fn_dir/ce-flipper-output" #output directory for script

#generate output folder
mkdir "$out_dir"
if [ -d "$out_dir" ]
then
	#make flag file for ce-flipper
	touch "$out_dir/$fn_strip.anim"
	#convert video to .png
	if ! ffmpeg -i "$fn" -r 3 "$out_dir/$fn_strip.anim-%5d.png"
	then
		echo "error: ffmpeg could not convert '$1'!"
		exit 1
	fi
	#convert .png to .bmp
	#you can play with that convert command to your liking
	#would just use normal error-diffusion dithering but imagemagick produces particularly ugly results for monochrome images (contrast doing it in GIMP, which looks quite nice)
	#CE-Flipper doesn't need to use 1-bit .bmp files, but my device is grayscale (and slow) so it works better with them
	cd "$out_dir"
	for f in *.png
	do
		echo "generating $(basename "$f" ".png").bmp"
		convert "$f" -resize "240x120>" +dither -ordered-dither o2x2 -monochrome -colors 2 "BMP3:$(basename "$f" ".png").bmp"
		rm "$f"
	done
else
	echo "error: could not make '$out'!"
	exit 1
fi
