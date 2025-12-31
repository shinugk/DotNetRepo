#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 👇 IMPORTANT: point to subfolder
COPY ["InterviewTracker/InterviewTracker.csproj", "InterviewTracker/"]
RUN dotnet restore "InterviewTracker/InterviewTracker.csproj"

COPY . .
WORKDIR "/src/InterviewTracker"

RUN dotnet publish "InterviewTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InterviewTracker.dll"]
