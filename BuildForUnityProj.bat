dotnet build src/UnityMVVM.sln --no-dependencies --output build
copy src\UnityMVVM\build\UnityMVVM.dll MvvmUnityProj\CCG\Assets\Packages\com.kekchpek.umvvm\UnityMVVM.dll
copy src\UnityMVVM\build\UnityMVVM.dll MvvmUnityProj\QuickStartUnityMVVM\Assets\Libs\UnityMVVM.dll