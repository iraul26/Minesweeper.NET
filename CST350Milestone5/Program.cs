using CST350Milestone5.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Setup NLog configuration as per https://github.com/NLog/NLog/wiki/Configure-from-code
NLog.Config.LoggingConfiguration config = new NLog.Config.LoggingConfiguration();
// Targets
NLog.Targets.FileTarget logFile = new NLog.Targets.FileTarget("logfile") { FileName = "${basedir}/logs/${shortdate}.log" };
NLog.Targets.ConsoleTarget logConsole = new NLog.Targets.ConsoleTarget("logconsole");
// Layouts
logFile.Layout = "${longdate} ${uppercase:${level}} ${message}";
logConsole.Layout = "${longdate} ${uppercase:${level}} ${message}";
// Rules
config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logFile);
config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logConsole);
// Set config
NLog.LogManager.Configuration = config;
// END Nlog

// Register Logger
builder.Services.AddSingleton<IMyLogger, MyLogger>();

// Add HTML Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
