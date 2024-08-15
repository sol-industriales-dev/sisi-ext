
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

RUN apt-get update && apt-get --fix-broken install -y \
	wget \
	dpkg \
	cabextract \
	xfonts-utils \
	fontconfig \
	libfontconfig1 \
	libgdiplus \
	libc6-dev \
	libfreetype6

RUN ln -s /usr/lib/libgdiplus.so
RUN ln -s /lib/x86_64-linux-gnu/libdl.so.2 
RUN ln -s /lib/x86_64-linux-gnu/libdl.so

RUN wget http://ftp.de.debian.org/debian/pool/contrib/m/msttcorefonts/ttf-mscorefonts-installer_3.8_all.deb 
RUN dpkg -i ttf-mscorefonts-installer_3.8_all.deb

RUN echo ttf-mscorefonts-installer msttcorefonts/accepted-mscorefonts-eula select true | debconf-set-selections
RUN apt-get install ttf-mscorefonts-installer

RUN fc-cache -f -s -v

EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Doconut.Docker.Core/Doconut.Docker.csproj", "Doconut.Docker.Core/"]

COPY ["Doconut.Docker.Core/Doconut.Config.dll", "Doconut.Docker.Core/"]
COPY ["Doconut.Docker.Core/Doconut.Formats.dll", "Doconut.Docker.Core/"]
COPY ["Doconut.Docker.Core/Doconut.Viewer.dll", "Doconut.Docker.Core/"]

RUN dotnet restore "Doconut.Docker.Core/Doconut.Docker.csproj"
COPY . .
WORKDIR "/src/Doconut.Docker.Core"
RUN dotnet build "Doconut.Docker.csproj" -c Release -r linux-x64 -o /app/build

FROM build AS publish
RUN dotnet publish "Doconut.Docker.csproj" -c Release -r linux-x64 -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doconut.Docker.dll"]
