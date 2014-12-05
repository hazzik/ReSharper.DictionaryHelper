set msbuild="%ProgramFiles(x86)%\MSBuild\14.0\Bin\msbuild.exe"
%msbuild% Build.msbuildproj -T:All -P:PackageOutputDir=release
