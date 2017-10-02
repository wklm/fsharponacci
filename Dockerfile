FROM microsoft/dotnet
COPY . /app
WORKDIR /app
RUN dotnet publish --self-contained -r linux-x64 -v q 
RUN ["./bin/Debug/netcoreapp2.0/linux-x64/fib"] 