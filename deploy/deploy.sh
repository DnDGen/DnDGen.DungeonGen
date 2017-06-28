 
echo "Deploying DungeonGen to NuGet"

ApiKey=$1
Source=$2

echo "Nuget Source is $Source"
echo "Nuget API Key is $ApiKey (should be secure)"

echo "Listing bin directory"
for entry in "./DungeonGen/bin"/*
do
  echo "$entry"
done

echo "Packing DungeonGen"
nuget pack ./DungeonGen/DungeonGen.nuspec -Verbosity detailed

echo "Packing DungeonGen.Domain"
nuget pack ./DungeonGen.Domain/DungeonGen.Domain.nuspec -Verbosity detailed

echo "Pushing DungeonGen"
nuget push ./DungeonGen.*.nupkg -Verbosity detailed -ApiKey $ApiKey -Source $Source

echo "Pushing DungeonGen.Domain"
nuget push ./DungeonGen.Domain.*.nupkg -Verbosity detailed -ApiKey $ApiKey -Source $Source