FROM microsoft/dotnet:2.2-aspnetcore-runtime
LABEL maintainer="caoyue"

WORKDIR /app
EXPOSE 8001
ENTRYPOINT ["dotnet", "MessagePusher.Web.dll"]

