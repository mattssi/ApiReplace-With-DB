# Utiliza la imagen de SDK de .NET Core como base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establece el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copia el archivo de proyecto y restaura las dependencias
COPY ./*.csproj ./
RUN dotnet restore

# Copia todo el c�digo fuente y compila la aplicaci�n
COPY . ./
RUN dotnet publish -c Release -o out

# Crea una imagen para la aplicaci�n final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "ChistesAPI_DB.dll"]
