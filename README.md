# myRsa .NET

## Getting started / build & install / Use

### Build & install
Require .net sdk for be builded 7 :
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.401-windows-x64-installer

PS : Les binaires de l'applications sont dans le repos, dans le dossier application, si jamais vous ne voulez pas build l'application & télecharger le sdk .net.

Compilation & géneration du binaire :

Mode dev : dotnet build
Le dossier content l'application et les binaires -> ./bin/Debug/net7.0/myrsa.exe
Mode prod : dotnet publish -c Release
Le dossier content l'application et les binaires -> ./bin/Release/net7.0/publish/myrsa.exe

### Exemple d'utilisation
dotnet publish -c Release

Le dossier content l'application et les binaires est généré dans le dossier suivant : ./bin/Release/net7.0/publish/myrsa.exe
Vous pouvez le déplacer ou vous le souhaitez.

cd ./bin/Release/net7.0/publish/

./myrsa.exe keygen "je suis une cle"

Les clées sont générées dans le dossier de l'exécutable.

Créer le fichier text.txt avec le texte à crypter.

./myrsa.exe crypt monRsa.pub text.txt -i -o

Le fichier encrypted.txt est généré dans le dossier de l'exécutable.

./myrsa.exe decrypt monRsa.priv encrypted.txt -i -o

Le fichier decrypted.txt est généré dans le dossier de l'exécutable.
