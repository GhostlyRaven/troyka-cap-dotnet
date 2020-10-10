dotnet publish -c Debug --no-self-contained
pushd $1
pscp -pw $5 -v -r ./* $4@$3:$2
popd