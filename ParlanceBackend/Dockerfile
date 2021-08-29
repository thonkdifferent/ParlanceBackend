FROM mcr.microsoft.com/dotnet/sdk:5.0
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs ssh git
COPY . Sources/
WORKDIR /Sources/Frontend
RUN npm install
RUN npm run build
WORKDIR /Sources
RUN dotnet publish -c Release -o ../App
WORKDIR /App
RUN mkdir /Data
ENTRYPOINT ["dotnet", "ParlanceBackend.dll"]