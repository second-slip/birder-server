FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS base
WORKDIR /app
EXPOSE 80
 
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Birder.csproj", ""]
RUN dotnet restore "Birder.csproj"
COPY . .
# WORKDIR "/src/"
RUN dotnet build "Birder.csproj" -c Release -o /app
 
FROM build AS publish
RUN dotnet publish "Birder.csproj" -c Release -o /app

#Angular build
FROM node as nodebuilder

# set working directory
RUN mkdir /usr/src/app
WORKDIR /usr/src/app

# add `/usr/src/app/node_modules/.bin` to $PATH
ENV PATH /usr/src/app/node_modules/.bin:$PATH


# install and cache app dependencies
COPY ClientApp/package.json /usr/src/app/package.json
RUN npm install
RUN npm install -g @angular/cli@8.0.2 --unsafe

# add app

COPY ClientApp/. /usr/src/app

RUN npm run build

#End Angular build

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN mkdir -p /app/ClientApp/dist
COPY --from=nodebuilder /usr/src/app/dist/. /app/ClientApp/dist/
ENTRYPOINT ["dotnet", "Birder.dll"]