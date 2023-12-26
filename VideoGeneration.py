from moviepy.editor import VideoFileClip, AudioFileClip, CompositeAudioClip, CompositeVideoClip;
import argparse;
import os;
import time;

parser = argparse.ArgumentParser(description = "Procedural Video Generation");

parser.add_argument("-l", "--video-length", help = "The desired length of the video in seconds;");
parser.add_argument("-a", "--audio-directory", help = "The full path to the folder containing the audio files;");
parser.add_argument("-v", "--video-directory", help = "The full path to the folder containing the video files;");
parser.add_argument("-m", "--match", help = "Force the desired video length;", default = False);
parser.add_argument("-n", "--name", help = "The output file name;", default = "%i.mp4" % (time.time()));

arguments = parser.parse_args();
videoClips = [];
audioClips = [];
totalVideoDuration = 0;
totalAudioDuration = 0;

videoFilenames = os.listdir(arguments.video_directory);

for videoClip in videoFilenames : 
    if(videoClip.__contains__(".mp4")) : 
        currentVideoClip = VideoFileClip(os.path.join(arguments.video_directory, videoClip));
        currentVideoClip = currentVideoClip.set_start(totalVideoDuration);
        if(totalVideoDuration + currentVideoClip.duration > float(arguments.video_length)) : 
            break;
        
        totalVideoDuration += currentVideoClip.duration;
        videoClips.append(currentVideoClip);
        videoFilenames.append(videoClip);


audioFilenames = os.listdir(arguments.audio_directory);

for audioClip in audioFilenames : 
    if(audioClip.__contains__(".mp3") or audioClip.__contains__(".wav")) : 
        currentAudioClip = AudioFileClip(os.path.join(arguments.audio_directory, audioClip));
        currentAudioClip = currentAudioClip.set_start(totalAudioDuration);
        if(totalAudioDuration + currentAudioClip.duration > float(arguments.video_length)) : 
            break;
        
        totalAudioDuration += currentAudioClip.duration;
        audioClips.append(currentAudioClip);
        audioFilenames.append(audioClip);


#breakpoint();
compositeAudio = CompositeAudioClip(audioClips);
compositeVideo = CompositeVideoClip(videoClips);
compositeVideo = compositeVideo.set_audio(compositeAudio);
compositeVideo.set_duration(min(totalAudioDuration, totalVideoDuration)).write_videofile(arguments.name);
