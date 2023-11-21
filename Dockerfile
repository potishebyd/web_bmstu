ARG CONFIGURATION
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /backend
EXPOSE 80
EXPOSE 443

ARG CONFIGURATION
RUN echo ${CONFIGURATION}
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /web_bmstu

COPY ./*.sln ./
COPY . .

ARG CONFIGURATION
RUN echo ${CONFIGURATION}

RUN dotnet restore 
WORKDIR /web_bmstu/web_bmstu
RUN dotnet build "web_bmstu.csproj" -c "$CONFIGURATION" -o /backend/build

ARG CONFIGURATION
RUN echo ${CONFIGURATION}

FROM build AS publish
RUN dotnet publish "web_bmstu.csproj" -c "$CONFIGURATION" -o /backend/publish

FROM base AS final
WORKDIR /backend
COPY --from=publish /backend/publish .
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "web_bmstu.dll"]