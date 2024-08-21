dotnet build src/UnityMVVM.sln --no-dependencies --property:OutputPath=build
cp src/UnityMVVM/build/UnityMVVM.dll MvvmUnityProj/CCG/Assets/Packages/com.kekchpek.umvvm/UnityMVVM.dll
cp src/UnityMVVM/build/UnityMVVM.dll MvvmUnityProj/QuickStartUnityMVVM/Assets/Libs/UnityMVVM.dll