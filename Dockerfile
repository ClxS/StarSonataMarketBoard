FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

RUN apt-get update && \
    apt-get install -y dos2unix

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

#FROM mcr.microsoft.com/dotnet/core/sdk AS update-env
#COPY . .

#RUN chmod +x ./entrypoint.sh
#CMD /bin/bash ./entrypoint.sh

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS runtime-env
COPY --from=build-env /out .

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "SSMB.Blazor.dll"]