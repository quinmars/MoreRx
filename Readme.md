# MoreRx - More reactive extensions

![Nuget](https://img.shields.io/nuget/v/MoreRx) [![Unit Tests](https://github.com/quinmars/MoreRx/actions/workflows/unittests.yml/badge.svg)](https://github.com/quinmars/MoreRx/actions/workflows/unittests.yml) [![codecov](https://codecov.io/gh/quinmars/MoreRx/branch/main/graph/badge.svg?token=CKB7I7NJXT)](https://codecov.io/gh/quinmars/MoreRx)

This library contains a collection of operators, that are not important enough to be part of the official [`System.Reactive`](https://github.com/dotnet/reactive/) package, but still useful enough to be shared in a library.

The library currently comprises the following operators:

  - `Chunk`
  - `DelayOn` and `DelayOff`
  - `OrderBy`, `LargestBy`, `SmallestBy`, and `ThenBy`
  - `Pairwise`
  - `SwitchFirst`
  - `TakeUntil` + `CancellationToken`