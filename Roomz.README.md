Forked, currently to be manually built and published in our private NuGet feed.

Before releasing, increase the version in Directory.Build.props.

Requires `rust` and `cross`.

To install `rust`, check https://www.rust-lang.org/tools/install'

Then, to install `cross`:
```shell
cargo install cross --git https://github.com/cross-rs/cross
```

To build the nuget package, from project root (it might be needed to install targets manually):
```shell
cd native/resvg-wrapper

cargo build --release --target x86_64-pc-windows-msvc
cross build --release --target x86_64-unknown-linux-gnu
# cross build --release --target aarch64-apple-darwin # does not work

cd ../..

# only the 1st time
mkdir build\runtimes\win-x64\native\ 
cp native\resvg-wrapper\target\x86_64-pc-windows-msvc\release\resvg_wrapper.dll build\runtimes\win-x64\native\

# only the 1st time
mkdir build\runtimes\linux-x64\native\
cp native\resvg-wrapper\target\x86_64-unknown-linux-gnu\release\libresvg_wrapper.so build\runtimes\linux-x64\native\

rm .\nupkgs\*

dotnet restore
dotnet build --configuration Release --no-restore
dotnet pack src/ResvgSharp/ResvgSharp.csproj --configuration Release --no-build --output nupkgs
```

Pushing the package to our private NuGet feed:
```shell
dotnet nuget push nupkgs/*.nupkg --source "https://nuget.pkg.github.com/roomz-io/index.json" --api-key <<api key>> --skip-duplicate
```

