ildMax = 15;

% ILD to angle
ild = -ildMax:.1:ildMax;
angle = -(1 ./ (1 + exp(3 * ild / ildMax)) - .5) * 90 * 2.22;
figure(10);
subplot(121);
plot(ild, angle);
xlabel('ILD (dB)');
ylabel('Angle (\circ)');

% angle to ILD
angle = -90:90;
x = .5 - angle / 90 / 2.22;
ild = - log(x ./ (1 - x)) * ildMax / 3;

subplot(122);
plot(angle, ild);
xlabel('Angle (\circ)');
ylabel('ILD (dB)');
