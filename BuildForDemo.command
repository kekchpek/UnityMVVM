dotnet build src/UnityMVVM.sln --no-dependencies --property:OutputPath=build
cp src/UnityMVVM/build/UnityMVVM.dll DemoUnityProj/CCG/Assets/Libs/UnityMVVM.dll
cp src/UnityMVVM/build/UnityMVVM.dll DemoUnityProj/QuickStartUnityMVVM/Assets/Libs/UnityMVVM.dll