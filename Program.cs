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

var windowIcon = Observable.Create<WindowIcon>(
    observer =>
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var stream = assets!.Open(new Uri("avares://AvaloniaClock/Assets/icon.ico"));
        var windowIcon = new WindowIcon(stream);
        
        observer.OnNext(windowIcon);
        return Disposable.Empty;
    }
);

var background = Observable.Create<IImageBrush>(
    observer =>
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var stream = assets!.Open(new Uri("avares://AvaloniaClock/Assets/bg.png"));
        var bitmap = Bitmap.DecodeToWidth(stream, 800);
        var imageBrush = new ImageBrush(bitmap);
        imageBrush.Stretch = Stretch.Fill;

        observer.OnNext(imageBrush);
        return Disposable.Empty;
    }
);
Window Build() =>
    Window()
        .Width(400).Height(200).CanResize(false)
        .WindowStartupLocation(WindowStartupLocation.CenterScreen)
        .Icon(windowIcon)
        .Background(background)
        .Content(
                Grid()
                .Children(
                    Border()
                        .Margin(25, 0, 25, 0)
                        .Height(100)
                        .CornerRadius(10)
                        .BoxShadow(BoxShadows.Parse("5 5 10 2 Black"))
                        .Background(Brushes.White).Opacity(0.5)
                        .Child(
                            TextBlock()
                            .Foreground(Brushes.Black)
                            .TextAlignmentCenter()
                            .FontSize(40)
                            .FontStretch(FontStretch.Expanded)
                            .VerticalAlignment(VerticalAlignment.Center)
                            .Text(currentTime).Opacity(50)
                        )
                )
        );

AppBuilder
    .Configure<Application>()
    .UsePlatformDetect()
    .UseFluentTheme()
    .StartWithClassicDesktopLifetime(Build, args);