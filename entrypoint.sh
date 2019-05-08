#!/bin/bash

set -e
cd SSMB.SQL
run_cmd="dotnet ../out/SSMB.Blazor.dll"

until dotnet ef database update; do
>&2 echo "SQL Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up"
#exec $run_cmd