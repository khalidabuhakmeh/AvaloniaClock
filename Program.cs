using Avalonia.Media.Imaging;

var currentTime = Observable.Create<string>(
    observer =>
    {
        var timer = new System.Timers.Timer();
        timer.Interval = 1000;
        timer.Elapsed += (_, _) => observer.OnNext($"{DateTime.Now:hh:mm:ss tt}");
        timer.Start();
        return Disposable.Empty;
    });

var backgroundImage = Observable.Create<Bitmap>(
        observer =>
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var stream = assets!.Open(new Uri("avares://AvaloniaClock/Assets/bg.png"));
            var bitmap = Bitmap.DecodeToWidth(stream, 800);
            observer.OnNext(bitmap);
            return Disposable.Empty;
        }
    );

Window Build() =>
    Window()
        .Width(400).Height(200).CanResize(false)
        .WindowStartupLocation(WindowStartupLocation.CenterScreen)
        .Content(
                Grid()
                .Children(
                    Image()
                        .ZIndex(0)
                        .Source(backgroundImage)
                        .Width(400).Height(400)
                        .Stretch(Stretch.Fill),
                    Border()
                        .Margin(25, 0, 25, 0)
                        .Height(100)
                        .CornerRadius(10)
                        .BoxShadow(BoxShadows.Parse("5 5 10 2 Black"))
                        .Background(Brushes.White)
                        .Child(
                            TextBlock()
                            .Foreground(Brushes.Black)
                            .TextAlignmentCenter()
                            .ZIndex(1)
                            .FontSize(40)
                            .FontStretch(FontStretch.Expanded)
                            .VerticalAlignment(VerticalAlignment.Center)
                            .Text(currentTime)
                        )
                )
        );

AppBuilder
    .Configure<Application>()
    .UsePlatformDetect()
    .UseFluentTheme()
    .StartWithClassicDesktopLifetime(Build, args);