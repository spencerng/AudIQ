clear; close all; clc
fname = 'bla.wav';%output file name
Fs = 44.1e3;%sampling frequency in Hz
fmax = 10e3; %max permitted frequency in the signal in kHz (depends on device, here assumption is that most devices don't play out sound above 10kHz)

totalDuration = 1;%total duration in seconds

rampDuration = 20e-3;% onset and offset ramps in seconds (20 ms is a good default)
rampLength = rampDuration * Fs;
tramp = sin(pi/2*(0:rampLength-1)/rampLength).^2;

[b,a] = butter(4,fmax/(Fs/2),'low');%anti-aliasing filter.

% define time vector
t = 0 : 1/Fs : totalDuration;
which_sound = 2;
switch which_sound
    case 1, % one click
        yraw = 0.*t;
        yraw(round(length(yraw)*0.3)) = 1;
    case 2, % click train
        pitch = 100; % pitch in Hz, hard code here (define)
        dt = round(1/pitch*Fs);% one period duration in samples
        tmp = zeros(1,dt); 
        tmp(1) = 1;
        N = floor(length(t)/dt);
        yraw = repmat(tmp,1,N);
        yraw = [yraw, zeros(1,length(t)-length(yraw))];
    case 3, %white noise
        yraw = rand(size(t));
        yraw = yraw - mean(yraw);
    case 4, % pure tone
        pitch = 1000; % pitch in Hz, hard code here (define)
        yraw = sin(2*pi*pitch*t);
    case 5, % harmonic stack
        pitch = 1000; % pitch in Hz, hard code here (define)
        nharmonics = 5;% number of harmonics, hard code here (define), be sure that nharmonics*pitch <= fmax
        yraw = sin(2*pi*pitch*t);
        for nn = 2 : nharmonics
            yraw = yraw + sin(2*pi*pitch*t*nn);
            
        end
    otherwise
        yraw = [];
        disp('I generated a boring, empty signal, Please modify which_sound to generate something more exciting...')
end


yraw = filtfilt(b,a,yraw);% anti-aliasing




% ramp sound on and off
ramp = [tramp, ones(1,length(t)-2*rampLength), fliplr(tramp)];
yraw = yraw .* ramp;
yraw = 0.9*yraw/max(abs(yraw(:)));% avoid peak clipping and quantization noise when saving the audio file

nbits = 16;% or 24


audiowrite(fname,yraw,Fs,'BitsPerSample',nbits);
audiq_testsignals