FROM johnnyasantoss/dotnet-mono-docker:dotnet2.0-mono5.2-sdk

WORKDIR /app

COPY ./** /app/

RUN apt-get update && apt-get install -y libgit2-24

RUN ./build.sh -t "pack" -v > dockerbuild.txt 2>&1
