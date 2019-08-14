clear; close all; clc

% list of file names -- add or remove files as needed
fnames = {'GLOVE_OG.wav';'GLOVE_Matlab.wav';'GLOVE_STRETCHED_AUDITION.wav';'GLOVE_STRETCHED_UNITY.wav'};

%delta: time-shift for all signals,
%change the numbers with hard coding
%(e..g, delta = [0.1; 0; -0.1; 0]; % shifts first signal by +0.1 s and third signal by -0.1s)
%note: to determine delta, start with delta = [0;0;0;0]; and use visual
%inspection of the plots. It's a hack, but I don't have time to code it
%more cleanly.
delta = zeros(length(fnames),1);%
%read in the audiofiles you'd like to compare
for which_file = 1 :  length(fnames)
    [tmpy,tmpFs ] =audioread(fnames{which_file});
    tmpy = tmpy./rms(tmpy);% make sure you are comparing sounds with equal RMS. Here: RMS= 1
    % apply time shift in delta(which_file) seconds... you may want this to
    % closely time-align traces, for comparing rising / falling slope as a function of time etc.
    tmpy = tmpy + delta(which_file);
    
    y{which_file} = tmpy;
    Fs{which_file} = tmpFs;
    % define time vector in units of seconds
    t{which_file} = 0 : 1/tmpFs : (length(tmpy)-1)/tmpFs;
    
    % plot out the recording
    figure(111); hold on; hl(which_file) = plot(t{which_file},y{which_file}); xlabel('Time [s]');ylabel('Amplitude');
    
    % plot out the envelope of the recording
    [b,a] = butter(4,[100/(tmpFs/2)],'low');% define gentle lowpass filter
    env{which_file} = filtfilt(b,a,abs(hilbert(tmpy)));
    figure(222); hold on; hl2(which_file) = plot(t{which_file},env{which_file},'linewidth',2); xlabel('Time [s]');ylabel('Envelope');
    
    
    tstr = fnames{which_file}; tstr(tstr == '_') = ' ';% cosmetics. Matlab interprets _ as subscript for labels
    hlstr{which_file} = tstr;
    
    
    nbits = 9;% number of steps to represent your spectrogram; default = 10. play with it, try 8, 9, 10
    %large nbits : fine spectral but poor temporal resolution
    %small nbits : poor spectral but fine temporal resolution
    
    figure(333+which_file);
    %2^nbits-1: The window length over which the sound energy should be computed. Choose and odd number, so that the time point 0 is always included,
    %70: the number of samples that subsequent windows should overlap, and this number should not be bigger than half the window size.
    % 2^(nbits+1): the number of frequency points that are used to calculate the discrete Fourier transform. This last number should be a power of 2
    [S,F,T] = spectrogram(tmpy,2^nbits-1,70,2^(nbits+1),tmpFs);
    args = {T,F,20*log10(abs(S)+eps)};
    hndl = surf(args{:},'EdgeColor','none');
    xlabel('Time [s] '); ylabel('Frequency [Hz] ') ;zlabel('Magnitude [dB] ')
    title(tstr)
    caxis([-60 40])
    ylim([0 tmpFs/2]);
    xlim([min(t{which_file}) max(t{which_file})])
    view(0,90)%also try view(7,65)
    %     % play the sound, continue after button push (uncomment to run)
    %     p = audioplayer(tmpy,tmpFs);
    %     playblocking(p);
    %     pause
end
figure(111);legend(hl,hlstr)
figure(222);legend(hl2,hlstr)

