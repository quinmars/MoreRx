using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

var config = DefaultConfig
    .Instance
    .AddJob(Job.Default.WithCustomBuildConfiguration("Baseline"))
    .AddJob(Job.Default.WithCustomBuildConfiguration("Current"));

BenchmarkSwitcher
    .FromAssemblies(new[] { typeof(Program).Assembly })
    .Run(args, config);
