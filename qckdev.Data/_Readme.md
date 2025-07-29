
This project generates a different AssemblyName between frameworks. 
Net35 assembly name is qckdev.Data.2.dll
Other are qckdev.Data.dll

According to this article, this is broken in VS2022 and it is necessary to run dotnet restore and dotnet build manually
https://github.com/dotnet/sdk/issues/22469#issuecomment-1732733899

Before starts, it is necessary run the following commands

dotnet restore
dotnet build

Before packaging, it is necessary run the following commands

dotnet build --configuration Release
dotnet pack --configuration Release
