FROM microsoft/dotnet:runtime
MAINTAINER caoyue

WORKDIR /app
EXPOSE 8001
ENTRYPOINT ["dotnet", "MessagePusher.Web.dll"]

